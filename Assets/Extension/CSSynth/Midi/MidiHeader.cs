using System.Collections.Generic;

namespace CSSynth.Midi
{
    public class MidiHeader
    {
        //--Variables
        public int deltaTicksPerQuarterNote;
        public MidiHelper.MidiFormat MidiFormat;
        public MidiHelper.MidiTimeFormat TimeFormat;

        public List<KeySignature> keySignatures;
        public List<Meta> metas;
        public string name;
        public string copyRight;
        public List<TimeSignature> timeSignatures;
        public List<Tempo> tempos;

        //--Public Methods
        public MidiHeader()
        {
            keySignatures = new List<KeySignature>();
            metas = new List<Meta>();
            timeSignatures = new List<TimeSignature>();
            tempos = new List<Tempo>();
        }
        public void setMidiFormat(int format)
        {
            if (format == 0)
                MidiFormat = MidiHelper.MidiFormat.SingleTrack;
            else if (format == 1)
                MidiFormat = MidiHelper.MidiFormat.MultiTrack;
            else if (format == 2)
                MidiFormat = MidiHelper.MidiFormat.MultiSong;
        }
    }

    public class KeySignature
    {
        public string key;
        public string scale;
        public float ticks;
    }
    public class Meta
    {
        public string text;
        public string type;
        public float ticks;
    }

    public class Tempo
    {
        public float bpm;
        public float ticks;
        public float time;
    }

    public class TimeSignature
    {
        public List<int> timeSignature;
        public float ticks;
        public float measures;
    }
}
