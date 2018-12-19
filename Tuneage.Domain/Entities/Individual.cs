namespace Tuneage.Domain.Entities
{
    public class Individual
    {
        public virtual int Id
        {
            get;
            set;
        }

        public virtual string FullName
        {
            get;
            set;
        }

        public virtual bool IsComposer
        {
            get;
            set;
        }

        public virtual SoloArtist SoloArtist
        {
            get;
            set;
        }

        public virtual Lineup[] Lineups
        {
            get;
            set;
        }
    }
}