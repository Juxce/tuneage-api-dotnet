namespace Tuneage.Domain.Entities
{
    public abstract class Artist
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

        public virtual bool IsBand
        {
            get;
            set;
        }

        public virtual bool IsPrinciple
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

        public virtual ArtistSaidCred[] ArtistSaidCreds
        {
            get;
            set;
        }

        public virtual Song[] Songs
        {
            get;
            set;
        }

        public virtual Recording[] Recordings
        {
            get;
            set;
        }

        public virtual SingleArtistRelease[] SingleArtistReleases
        {
            get;
            set;
        }

        public virtual VariousArtistsRelease[] VariousArtistsReleases
        {
            get;
            set;
        }
    }
}