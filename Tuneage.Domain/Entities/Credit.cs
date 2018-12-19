namespace Tuneage.Domain.Entities
{
    public class Credit
    {
        public virtual int Id
        {
            get;
            set;
        }

        public virtual int RecordingId
        {
            get;
            set;
        }

        public virtual int IndividualId
        {
            get;
            set;
        }

        public virtual int InstrumentId
        {
            get;
            set;
        }

        public virtual bool IsFeaturedPerformer
        {
            get;
            set;
        }

        public virtual Recording Recording
        {
            get;
            set;
        }

        public virtual Individual Individual
        {
            get;
            set;
        }

        public virtual Instrument Instrument
        {
            get;
            set;
        }
    }
}