using System;
using System.Collections.Generic;
using CSSynth.Midi;
using UnityEngine;

namespace MagicTiles.Scripts.MIDI
{
    using System.Linq;
    using BaseDuet.Scripts.Data.SessionData;
    using BaseDuet.Scripts.Notes;
    using Cysharp.Threading.Tasks;
    using GameCore.Core.AssetsManager;
    using MagicTiles.Scripts.Models;
    using MagicTiles.Scripts.Models.Static;
    using Random = UnityEngine.Random;

    public class MidiGenerator
    {
        private readonly GlobalDataController globalDataController;
        private readonly IAssetManager        assetManager;
        private          MidiTrack            mainTrack;
        private          MidiTrack            supportTrack;
        private          MidiTrack            motifTrack;
        private          MidiTrack            relationTrack;
        private          MidiTrack            obstacleTrack;

        private Dictionary<int, float> pulseTempo = new Dictionary<int, float>();

        private static readonly string[] MajorKey = new[] { "Cb", "Gb", "Db", "Ab", "Eb", "Bb", "F", "C", "G", "D", "A", "E", "B", "F#", "C#" };
        private static readonly string[] MinorKey = new[] { "Ab", "Eb", "Bb", "F", "C", "G", "D", "A", "E", "B", "F#", "C#", "G#", "D#", "A#" };

        private static readonly float[] PossiblePosition = new[] { -3.6f, -1.2f, 1.2f, 3.6f };

        private const string TRACK_MAIN            = "main";
        private const string TRACK_SUPPORT         = "support";
        private const string TRACK_MOTIF           = "motif_main";
        private const string TRACK_RELATION        = "relation";
        private const string TRACK_OBSTACLE        = "obstabcle";
        private const int    NOTE_MAIN             = 96;
        private const int    NOTE_SUPPORT          = 84;
        private const int    SLIDE_OFFSET          = 5;
        private const int    MicrosecondsPerSecond = 1000000;
        private const int    NOTE_ENERGY           = 5;
        private const float  INTENTIONAL_DELAY     = 0;
        private const float  OBSTACLE_DELAY        = 0.4f;
        private const float  NEEDED_DISTANCE       = 0.3f;

        private List<NoteData> notesList     = new List<NoteData>();
        private List<NoteData> obstaclesList = new List<NoteData>();
        private bool           hasObstacle;

        public MidiGenerator(GlobalDataController globalDataController, IAssetManager assetManager)
        {
            this.globalDataController = globalDataController;
            this.assetManager         = assetManager;
        }

        public async UniTask<List<NoteModel>> GetNoteModels(byte[] midiContent, int seed, string songMode, bool hasObstacle = true)
        {
            this.hasObstacle = hasObstacle;
            ESongMode eSongMode;
            Enum.TryParse(songMode, out eSongMode);
            this.obstaclesList.Clear();
            this.notesList.Clear();
            this.GenerateNotes(midiContent);

            return this.GenerateNoteModels(seed, eSongMode);
        }

        private void GenerateNotes(byte[] midiContent)
        {
            this.obstacleTrack = null;
            if (midiContent == null || midiContent.Length <= 0)
            {
                Debug.LogError("MIDI file is empty");

                return;
            }

            MidiFile songFile = null;
            try
            {
                songFile = new MidiFile(midiContent, null);
            }
            catch (Exception e)
            {
                Debug.LogError("Midi Failed to Load!: " + e?.ToString());

                return;
            }

            // Get all track for processing
            for (int i = 0; i < songFile.Tracks.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(songFile.Tracks[i].trackName))
                {
                    switch (songFile.Tracks[i].trackName.Trim().ToLower())
                    {
                        case TRACK_MAIN:     this.mainTrack     = songFile.Tracks[i]; break;
                        case "fl keys":      this.mainTrack     = songFile.Tracks[i]; break;
                        case TRACK_SUPPORT:  this.supportTrack  = songFile.Tracks[i]; break;
                        case TRACK_MOTIF:    this.motifTrack    = songFile.Tracks[i]; break;
                        case TRACK_RELATION: this.relationTrack = songFile.Tracks[i]; break;
                        case TRACK_OBSTACLE: this.obstacleTrack = songFile.Tracks[i]; break;
                    }
                }
            }

            this.MidiEventToList(this.mainTrack.midiEvents, songFile, this.notesList);
            if (this.obstacleTrack != null) this.MidiEventToList(this.obstacleTrack.midiEvents, songFile, this.obstaclesList);
        }

        private void MidiEventToList(MidiEvent[] midiEvents, MidiFile songFile, List<NoteData> notesList)
        {
            List<MidiEvent> tempoEvents = songFile.getAllMidiEventsofType(MidiHelper.MidiChannelEvent.None, MidiHelper.MidiMetaEvent.None);
            for (int i = 0; i < tempoEvents.Count; i++)
            {
                if (tempoEvents[i].midiMetaEvent == MidiHelper.MidiMetaEvent.Tempo)
                {
                    var mpq   = (uint)tempoEvents[i].Parameters[0];
                    var bpm   = (uint)Mathf.RoundToInt((MidiHelper.MicroSecondsPerMinute / mpq) * (Mathf.Pow(2, songFile.Denominator) / 4.0f));
                    var pulse = (float)mpq / MicrosecondsPerSecond / songFile.MidiHeader.deltaTicksPerQuarterNote;
                    this.pulseTempo[i] = pulse;
                    //Debug.LogError(string.Format("xxx midiStart: {0}, MSPQ: {1}, BPM: {2}, Pulse: {3}",
                    //    tempoEvents[i].deltaTimeFromStart, mpq, bpm, pulse));
                }
            }

            Dictionary<byte, Stack<MidiEvent>> notesArray = new Dictionary<byte, Stack<MidiEvent>>();
            for (int i = 0; i < midiEvents.Length; i++)
            {
                //only care for channel events
                if (midiEvents[i].isChannelEvent())
                {
                    MidiEvent evt  = midiEvents[i];
                    byte      note = evt.parameter1;

                    //check for event's type, if is note_on push into timing table
                    if (evt.midiChannelEvent == MidiHelper.MidiChannelEvent.Note_On)
                    {
                        if (notesArray.ContainsKey(note))
                        {
                            notesArray[note].Push(evt);
                        }
                        else
                        {
                            Stack<MidiEvent> noteTiming = new Stack<MidiEvent>();
                            noteTiming.Push(evt);
                            notesArray.Add(note, noteTiming);
                        }
                    } //if is note_off, calculate note duration and add to song data
                    else if (evt.midiChannelEvent == MidiHelper.MidiChannelEvent.Note_Off)
                    {
                        MidiEvent onEvt = notesArray[note].Pop();

                        float pulse; // = 0;
                        //float noteTime = GetNoteTime(onEvt, tempoEvents, out pulse, deltaTickPerQuarterNote);
                        float noteTime = this.GetNoteTimeByDelta(onEvt.deltaTimeFromStart, tempoEvents, out pulse);

                        //print(string.Format("Note time: {0}, note time in beat: {1}", onEvt.deltaTimeFromStart * pulse, noteTime));
                        //how long the note should be displayed

                        int   midiDuration = (int)(evt.deltaTimeFromStart - onEvt.deltaTimeFromStart);
                        float realDuration = midiDuration * pulse;

                        NoteData n = new NoteData();
                        n.onTick       = onEvt.deltaTimeFromStart;
                        n.offTick      = evt.deltaTimeFromStart;
                        n.midiTimeOn   = (int)onEvt.deltaTimeFromStart;
                        n.velocity     = (int)onEvt.parameter2;
                        n.midiDuration = midiDuration;
                        n.realDuration = realDuration;
                        n.nodeID       = note;
                        n.timeAppear   = noteTime;

                        n.laneIndex = note;

                        n.noteOrder = this.notesList.Count;
                        //Debug.Log(string.Format("--Added note {0} at {1}, duration {2}, order: {3}", n.nodeID, n.timeAppear, n.duration, n.noteOrder));
                        notesList.Add(n);
                    }
                }
            }
        }

        private List<NoteModel> GenerateNoteModels(int seed, ESongMode eSongMode)
        {
            var noteModels         = new List<NoteModel>();
            var normalNoteSprite   = this.assetManager.Load<Sprite>("");
            var obstacleNoteSprite = this.assetManager.Load<Sprite>("");
            var noteAudioClip      = this.assetManager.Load<AudioClip>("");
            Random.InitState(seed);

            NoteDatasToNoteModels(this.notesList, false);

            noteModels = noteModels.OrderBy(x => x.TimeAppear).ThenBy(x => x.PositionX).ToList();

            for (var i = 0; i < noteModels.Count; i++)
            {
                noteModels[i].Process = 1f * i / noteModels.Count;
            }

            noteModels[^1].Process = 1;
            var lastAppear = 0f;
            var cache      = new List<NoteModel>();

            if (false)
            {
                var obstacleRule = StaticObstacleRule.ObstacleRules[eSongMode];
                var percent      = 1.5f * obstacleRule.CountCompareToNote / 100;
                for (var i = 0; i < noteModels.Count - 1; i++)
                {
                    if (noteModels[i].Process <= obstacleRule.SpawnFrom) continue;
                    if (!Mathf.Approximately(noteModels[i].TimeAppear, noteModels[i + 1].TimeAppear))
                    {
                        if (Random.value < percent) AddToCache(noteModels[i], GetNewPos(noteModels[i].PositionX));
                    }
                    else
                    {
                        if (Random.value < percent)
                        {
                            AddToCache(noteModels[i], GetNewPos(noteModels[i].PositionX, noteModels[i + 1].PositionX));
                            i++;
                        }
                    }
                }

                foreach (var noteModel in cache)
                {
                    noteModel.IsObstacle   = true;
                    noteModel.NoteSprite   = obstacleNoteSprite;
                    noteModel.IsStrong     = false;
                    noteModel.IsMoodChange = false;
                    noteModel.Process      = 0;

                    noteModels.Add(noteModel);
                }

                noteModels = noteModels.OrderBy(x => x.TimeAppear).ToList();

                var temp = noteModels.Where(x => x.PositionX is > -3.7f and < -3.5f).ToList();

                for (var i = 1; i < temp.Count - 1; i++)
                {
                    var current = temp[i];
                    if (!current.IsObstacle) continue;
                    var prev = temp[i - 1];
                    var next = temp[i + 1];
                    if (prev.IsObstacle && next.IsObstacle) continue;
                    if ((!prev.IsObstacle && Mathf.Abs(prev.PositionX - current.PositionX) < 0.1f && Mathf.Abs(prev.TimeAppear - current.TimeAppear) < NEEDED_DISTANCE) || (!next.IsObstacle && Mathf.Abs(next.PositionX - current.PositionX) < 0.1f && Mathf.Abs(next.TimeAppear - current.TimeAppear) < NEEDED_DISTANCE))
                    {
                        noteModels.Remove(current);
                    }
                }

                temp = noteModels.Where(x => x.PositionX is > -1.3f and < -1.1f).ToList();

                for (var i = 1; i < temp.Count - 1; i++)
                {
                    var current = temp[i];
                    if (!current.IsObstacle) continue;
                    var prev = temp[i - 1];
                    var next = temp[i + 1];
                    if (prev.IsObstacle && next.IsObstacle) continue;
                    if ((!prev.IsObstacle && Mathf.Abs(prev.PositionX - current.PositionX) < 0.1f && Mathf.Abs(prev.TimeAppear - current.TimeAppear) < NEEDED_DISTANCE) || (!next.IsObstacle && Mathf.Abs(next.PositionX - current.PositionX) < 0.1f && Mathf.Abs(next.TimeAppear - current.TimeAppear) < NEEDED_DISTANCE))
                    {
                        noteModels.Remove(current);
                    }
                }

                temp = noteModels.Where(x => x.PositionX is > 1.1f and < 1.3f).ToList();

                for (var i = 1; i < temp.Count - 1; i++)
                {
                    var current = temp[i];
                    if (!current.IsObstacle) continue;
                    var prev = temp[i - 1];
                    var next = temp[i + 1];
                    if (prev.IsObstacle && next.IsObstacle) continue;
                    if ((!prev.IsObstacle && Mathf.Abs(prev.PositionX - current.PositionX) < 0.1f && Mathf.Abs(prev.TimeAppear - current.TimeAppear) < NEEDED_DISTANCE) || (!next.IsObstacle && Mathf.Abs(next.PositionX - current.PositionX) < 0.1f && Mathf.Abs(next.TimeAppear - current.TimeAppear) < NEEDED_DISTANCE))
                    {
                        noteModels.Remove(current);
                    }
                }

                temp = noteModels.Where(x => x.PositionX is > 3.5f and < 3.7f).ToList();

                for (var i = 1; i < temp.Count - 1; i++)
                {
                    var current = temp[i];
                    if (!current.IsObstacle) continue;
                    var prev = temp[i - 1];
                    var next = temp[i + 1];
                    if (prev.IsObstacle && next.IsObstacle) continue;
                    if ((!prev.IsObstacle && Mathf.Abs(prev.PositionX - current.PositionX) < 0.1f && Mathf.Abs(prev.TimeAppear - current.TimeAppear) < NEEDED_DISTANCE) || (!next.IsObstacle && Mathf.Abs(next.PositionX - current.PositionX) < 0.1f && Mathf.Abs(next.TimeAppear - current.TimeAppear) < NEEDED_DISTANCE))
                    {
                        noteModels.Remove(current);
                    }
                }
            }
            else
                NoteDatasToNoteModels(this.obstaclesList, true);

            // noteModels.ForEach(x => x.PositionX *= this.globalDataController.NoteMargin);
            noteModels = noteModels.OrderBy(x => x.TimeAppear).ToList();

            return noteModels;

            void AddToCache(NoteModel noteModel, float newPos)
            {
                if (noteModel.TimeAppear - lastAppear < OBSTACLE_DELAY) return;
                lastAppear = noteModel.TimeAppear;
                cache.Add(new(noteModel) { PositionX = newPos });
            }

            float GetNewPos(params float[] positions)
            {
                var res = positions[0];
                while (positions.Contains(res))
                {
                    res = PossiblePosition[Random.Range(0, PossiblePosition.Length)];
                }

                return res;
            }

            void NoteDatasToNoteModels(List<NoteData> notes, bool isObstalce)
            {
                var noteSprite         = isObstalce ? obstacleNoteSprite : normalNoteSprite;
                var listMoodchangeTime = notes.Where(x => x.velocity < 10).Select(x => x.timeAppear);
                for (var i = 0; i < notes.Count; i++)
                {
                    var note = notes[i];
                    if (note.laneIndex is < 96 or > 104) continue;
                    var duration     = Mathf.Max(note.realDuration - 0.2f, 0);
                    var positionX    = Mathf.Clamp(note.laneIndex - 98.5f, -2.5f, 2.5f);
                    var isStrong     = note.velocity > 101.6f;
                    var isMoodChange = listMoodchangeTime.Any(x => x == note.timeAppear) && !isObstalce;
                    positionX = positionX == 0 ? 0.5f : positionX;
                    positionX = positionX > 0 ? positionX + 0.5f : positionX - 0.5f;
                    //TODO Change distance between note
                    // positionX *= this.globalDataController.NoteMargin;
                    // positionX *= 1.2f;
                    var additionalDuration = 0f;
                    if (duration > 0)
                    {
                        while (duration >= 0)
                        {
                            ELongNote eLongNote = additionalDuration == 0 ? ELongNote.Head : ELongNote.Body;
                            var noteModel = new NoteModel(i, noteSprite, noteAudioClip, 0,
                                note.timeAppear + additionalDuration, 0, positionX,
                                note.velocity, 0, isStrong, isObstalce, eLongNote == ELongNote.Head ? isMoodChange : false, eLongNote);
                            noteModels.Add(noteModel);
                            duration           -= 0.05f;
                            additionalDuration += 0.05f;
                        }

                        noteModels[^1].ELongNote = ELongNote.Tail;
                    }
                    else
                    {
                        var noteModel = new NoteModel(i, noteSprite, noteAudioClip, 0,
                            note.timeAppear + additionalDuration, 0, positionX,
                            note.velocity, 0, isStrong, isObstalce, isMoodChange);
                        noteModels.Add(noteModel);
                    }
                }
            }
        }

        private float GetNoteTimeByDelta(ulong delta, List<MidiEvent> tempos, out float secondPerPulse)
        {
            float retVal                      = 0f;
            ulong remainingDeltaTimeFromStart = delta;
            secondPerPulse = 0f;
            bool highestTempo = true;
            for (int i = tempos.Count - 1; i >= 0; i--)
            {
                if (tempos[i].midiMetaEvent == MidiHelper.MidiMetaEvent.Tempo)
                {
                    if (delta >= tempos[i].deltaTimeFromStart)
                    {
                        float tempSecondPerPulse = this.pulseTempo[i];
                        if (highestTempo)
                        {
                            secondPerPulse = tempSecondPerPulse;
                            highestTempo   = false;
                        }

                        ulong temp = remainingDeltaTimeFromStart - tempos[i].deltaTimeFromStart;
                        retVal                      += (temp * tempSecondPerPulse);
                        remainingDeltaTimeFromStart =  tempos[i].deltaTimeFromStart;
                    }
                }
            }

            return retVal;
        }
    }
}