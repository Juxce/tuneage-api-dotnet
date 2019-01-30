namespace Tuneage.Domain.Entities
{
    public class VariousArtistsRelease : Release
    {
        // TODO: This type should go away after bringing Tracks/Recordings/Songs into the domain,
        // TODO: at which time a collection of Artists can take its place, and derive through the
        // TODO: songs associated with the recordings associated with the release's tracks
        public virtual ArtistVariousArtistsRelease[] ArtistVariousArtistsReleases
        {
            get;
            set;
        }
    }
}
