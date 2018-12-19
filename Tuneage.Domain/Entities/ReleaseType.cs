namespace Tuneage.Domain.Entities
{
    public class ReleaseType
    {
        public virtual string Id
        {
            get;
            set;
        }

        public virtual string Description
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
