namespace Tuneage.Domain.Entities
{
    public class ArtistVariousArtistsRelease
    {
        public int ArtistId
        {
            get;
            set;
        }

        public Artist Artist
        {
            get;
            set;
        }

        public int VariousArtistsReleaseId
        {
            get;
            set;
        }

        public VariousArtistsRelease VariousArtistRelease
        {
            get;
            set;
        }
    }
}
