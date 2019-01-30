namespace Tuneage.Domain.Entities
{
    public class Artist
    {
        public virtual int ArtistId
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

        public virtual int? PrincipleArtistId
        {
            get;
            set;
        }

        //public virtual decimal CredScoreSum
        //{
        //    get;
        //    set;
        //}

        //public virtual Cred[] Creds
        //{
        //    get;
        //    set;
        //}

        //public virtual ArtistSaidCred[] ArtistSaidCreds
        //{
        //    get;
        //    set;
        //}

        //public virtual Song[] Songs
        //{
        //    get;
        //    set;
        //}

        //public virtual Recording[] Recordings
        //{
        //    get;
        //    set;
        //}

        public virtual SingleArtistRelease[] SingleArtistReleases
        {
            get;
            set;
        }

        public virtual ArtistVariousArtistsRelease[] ArtistVariousArtistsReleases
        {
            get;
            set;
        }
    }
}