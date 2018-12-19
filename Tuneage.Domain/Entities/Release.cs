using System;

namespace Tuneage.Domain.Entities
{
    public abstract class Release
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

        public virtual int YearReleased
        {
            get;
            set;
        }

        public virtual DateTime ReleasedOn
        {
            get;
            set;
        }

        public virtual int LabelId
        {
            get;
            set;
        }

        public virtual bool IsByVariousArtists
        {
            get;
            set;
        }

        public virtual Track[] Tracks
        {
            get;
            set;
        }

        public virtual Cred[] Creds
        {
            get;
            set;
        }

        public virtual decimal CreditScoreSum
        {
            get;
            set;
        }

        public virtual Label Label
        {
            get;
            set;
        }
    }
}
