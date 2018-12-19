namespace Tuneage.Domain.Entities
{
    public class Instrument
    {
        public virtual int Id
        {
            get;
            set;
        }

        public virtual string Name
        {
            get;
            set;
        }

        public virtual Credit[] Credits
        {
            get;
            set;
        }
    }
}