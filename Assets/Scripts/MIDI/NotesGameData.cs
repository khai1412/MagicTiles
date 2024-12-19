using System;
using System.Collections.Generic;

public enum NoteEventData
{
    Normal           = 0,
    NoteBegin_Track1 = 1,
    NoteBegin_Track2 = 2,
    NoteBegin_Track3 = 3,
    NoteEnd_Track1   = 4,
    NoteEnd_Track2   = 5,
    NoteEnd_Track3   = 6,
}

[Serializable]
public class NoteData
{
    public int   noteEvent    = -1; // so le = begin, so chan = end
    public float timeKeepSong = 0.2f;
    public int   indexTrack   = 0; // moi bai gom 3 track
    public float speedBegin   = 1;

    public float timeAppear;
    public float timeAppearScaleApplied;

    public ulong onTick;
    public ulong offTick;

    public int          nodeID;
    public float        realDuration         = 0f;
    public float        duration             = 0f;
    public float        durationScaleApplied = 0f;
    public int          velocity;
    public int          laneIndex;
    public int          noteOrder;
    public int          mood = -1;
    public int          laneSlider;
    public List<int>    segmentLane           = null;
    public List<int>    segmentMidi           = null;
    public List<float>  segmentDurationScaled = null;
    public NoteDataType type                  = NoteDataType.Single;
    public int          midiDuration;
    public int          midiTimeOn;
    public bool         forceLanePos;
    public int          numberOfTap;

    public override string ToString()
    {
        return string.Format("[NoteData]" + " indexTrack: " + this.indexTrack + " noteOrder: " + this.noteOrder + " timeAppear: " + this.timeAppear + " timeAppearScaleApplied: " + this.timeAppearScaleApplied + " duration: " + this.duration + " durationScaleApplied: " + this.durationScaleApplied + " noteEvent: " + this.noteEvent + " speedBegin: " + this.speedBegin + " type: " + this.type + " nodeID: " + this.nodeID + " stringIndex: " + this.laneIndex + " timeKeepSong: " + this.timeKeepSong);
        /*return string.Format("[NoteData] noteEvent: {0} timeKeepSong: {1} indexTrack: {2} speedBegin: {3} timeAppear: {4}" +
                             " timeAppearScaleApplied: {5} nodeID: {6} duration: {7} stringIndex: {8} noteOrder: {9} type: {10} isLastNote: {11}"
            , noteEvent, timeKeepSong, indexTrack, speedBegin, timeAppear, timeAppearScaleApplied, nodeID, duration, stringIndex
            , noteOrder, type, isLastNote);*/
    }

    public NoteData Clone()
    {
        var res = new NoteData
        {
            noteEvent              = this.noteEvent,
            timeKeepSong           = this.timeKeepSong,
            indexTrack             = this.indexTrack,
            speedBegin             = this.speedBegin,
            timeAppear             = this.timeAppear,
            timeAppearScaleApplied = this.timeAppearScaleApplied,
            nodeID                 = this.nodeID,
            duration               = this.duration,
            durationScaleApplied   = this.durationScaleApplied,
            velocity               = this.velocity,
            laneIndex              = this.laneIndex,
            noteOrder              = this.noteOrder,
            mood                   = this.mood,
            laneSlider             = this.laneSlider,
            segmentLane            = this.segmentLane != null ? new List<int>(this.segmentLane) : null,
            segmentMidi            = this.segmentMidi != null ? new List<int>(this.segmentMidi) : null,
            segmentDurationScaled  = this.segmentDurationScaled != null ? new List<float>(this.segmentDurationScaled) : null,
            type                   = this.type,
            midiDuration           = this.midiDuration,
            midiTimeOn             = this.midiTimeOn,
            forceLanePos           = this.forceLanePos,
            numberOfTap            = this.numberOfTap,
            realDuration           = this.realDuration,
        };
        return res;
    }
}

#if UNITY_EDITOR
[Serializable]
#endif
public class NoteSfx
{
    public string trackName;
    public float  timeAppear;
    public float  timeSyncMusic;
}

public enum Difficulty
{
    SuperEasy,
    Easy,
    Normal,
    Advanced,
    SuperHard,
    NUM_TYPE,
}

public enum NoteDataType
{
    Single,
    Multi,
    Slash,
    Shake,
    Dual_Single,
    Dual_Multi,
    Hold,
    Slide,
    Dual_Slide,
    Multi_Sfx,
    Dual_Multi_Sfx,
    Companion_Tile,
    Big_Tile,

    //IMPORTANT: Insert new type above this line
    Red_Tile_Single,
    Red_Tile_Multi,
    Red_Tile_Dual_Single,
    Red_Tile_Dual_Multi,
}

public enum GameMode
{
    None,
    Normal,
    Holder,
    Slider,
    Both,
}