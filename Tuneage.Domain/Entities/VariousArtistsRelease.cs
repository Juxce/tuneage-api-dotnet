namespace Tuneage.Domain.Entities
{
    public class VariousArtistsRelease : Release
    {
        public virtual ArtistVariousArtistsRelease[] ArtistVariousArtistsReleases
        {
            get;
            set;
        }
    }
}
