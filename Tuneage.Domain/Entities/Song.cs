namespace Tuneage.Domain.Entities
{
    public class Song
    {
        public virtual int Id
        {
            get;
            set;
        }

        public virtual int ArtistId
        {
            get;
            set;
        }

        public virtual int CompositionId
        {
            get;
            set;
        }

        public virtual string TitleOverride
        {
            get;
            set;
        }

        public virtual string SubtitleOverride
        {
            get;
            set;
        }

        public virtual decimal CreditScoreSum
        {
            get;
            set;
        }

        public virtual Cred[] Creds
        {
            get;
            set;
        }

        public virtual CoveredByCred[] CoveredByCreds
        {
            get;
            set;
        }

        public virtual Artist Artist
        {
            get;
            set;
        }

        public virtual Composition Composition
        {
            get;
            set;
        }
    }
}