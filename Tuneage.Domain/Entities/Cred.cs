namespace Tuneage.Domain.Entities
{
    public abstract class Cred
    {
        public virtual int Id
        {
            get;
            set;
        }

        public virtual string Description
        {
            get;
            set;
        }

        public virtual decimal CredScore
        {
            get;
            set;
        }

        public virtual Recording[] Recordings
        {
            get;
            set;
        }

        public virtual Artist[] Artists
        {
            get;
            set;
        }

        public virtual Song[] Songs
        {
            get;
            set;
        }

        public virtual Composition[] Compositions
        {
            get;
            set;
        }

        public virtual Release[] Releases
        {
            get;
            set;
        }
    }
}