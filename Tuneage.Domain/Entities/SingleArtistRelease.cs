namespace Tuneage.Domain.Entities
{
    public class SingleArtistRelease : Release
    {
        public virtual Artist Artist
        {
            get;
            set;
        }
    }
}
