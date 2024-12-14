namespace CSSynth.Midi
{
    public class MidiEvent
    {
        //delta time in sample 
        public uint sampleTime;
        //delta time in midi ticks
        public uint deltaTime;
        public ulong deltaTimeFromStart;

        public MidiHelper.MidiMetaEvent midiMetaEvent;
        public MidiHelper.MidiChannelEvent midiChannelEvent;
        public object[] Parameters;
        public byte parameter1;
        public byte parameter2;
        public byte channel;
        //--Public Methods
        public MidiEvent()
        {
            this.Parameters = new object[5];
            this.midiMetaEvent = MidiHelper.MidiMetaEvent.None;
            this.midiChannelEvent = MidiHelper.MidiChannelEvent.None;
        }
        public bool isMetaEvent()
        {
            return midiChannelEvent == MidiHelper.MidiChannelEvent.None;
        }
        public bool isChannelEvent()
        {
            return midiMetaEvent == MidiHelper.MidiMetaEvent.None;
        }
        public MidiHelper.ControllerType GetControllerType()
        {
            if (midiChannelEvent != MidiHelper.MidiChannelEvent.Controller)
                return MidiHelper.ControllerType.None;
            switch (parameter1)
            {
                case 1:
                    return MidiHelper.ControllerType.Modulation;
                case 7:
                    return MidiHelper.ControllerType.MainVolume;
                case 10:
                    return MidiHelper.ControllerType.Pan;
                case 64:
                    return MidiHelper.ControllerType.DamperPedal;
                case 121:
                    return MidiHelper.ControllerType.ResetControllers;
                case 123:
                    return MidiHelper.ControllerType.AllNotesOff;
                default:
                    return MidiHelper.ControllerType.Unknown;
            }
        }

		public override string ToString ()
		{
			string str = "[";
			for (int i = 0; i < Parameters.Length; i++) {
				str += ", " + Parameters [i]; 
			}
			str += "]";

			return string.Format ("[MidiEvent] sampleTime: {0} deltaTime: {1} deltaTimeFromStart: {2} midiMetaEvent: {3}" +
				" midiChannelEvent: {4} parameter1: {5} parameter2: {6} channel: {7} parameters: {8}", sampleTime, deltaTime, deltaTimeFromStart
				, midiMetaEvent, midiChannelEvent, parameter1, parameter2, channel, str);
		}
    }
}
