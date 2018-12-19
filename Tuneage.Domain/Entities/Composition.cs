namespace Tuneage.Domain.Entities
{
    public class Composition
    {
        public virtual int Id
        {
            get;
            set;
        }

        public virtual string Title
        {
            get;
            set;
        }

        public virtual string Subtitle
        {
            get;
            set;
        }

        public virtual Composer[] Composers
        {
            get;
            set;
        }

        public virtual decimal CredScoreSum
        {
            get;
            set;
        }

        public virtual Cred[] Creds
        {
            get;
            set;
        }
    }
}