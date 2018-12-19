namespace Tuneage.Domain.Entities
{
    public class Source
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

        public virtual decimal Weight
        {
            get;
            set;
        }

        public virtual NewsworthyCred[] NewsworthyCreds
        {
            get;
            set;
        }
    }
}