namespace Tuneage.Domain.Entities
{
    public class RecordingType
    {
        public virtual int Id
        {
            get;
            set;
        }

        public virtual int Description
        {
            get;
            set;
        }

        public virtual Recording[] Recordings
        {
            get;
            set;
        }
    }
}