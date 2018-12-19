namespace Tuneage.Domain.Entities
{
    public class Track
    {
        public virtual int Id
        {
            get;
            set;
        }

        public virtual int ReleaseId
        {
            get;
            set;
        }

        public virtual int RecordingId
        {
            get;
            set;
        }

        public virtual int Sequence
        {
            get;
            set;
        }

        public virtual int Length
        {
            get;
            set;
        }

        public virtual string TrackSubtitle
        {
            get;
            set;
        }

        public virtual Release Release
        {
            get;
            set;
        }

        public virtual Recording Recording
        {
            get;
            set;
        }
    }
}