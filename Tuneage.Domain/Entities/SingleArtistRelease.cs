namespace Tuneage.Domain.Entities
{
    public class SingleArtistRelease : Release
    {
        public virtual int ArtistId
        {
            get;
            set;
        }

        public virtual Artist Artist
        {
            get;
            set;
        }
    }
}
