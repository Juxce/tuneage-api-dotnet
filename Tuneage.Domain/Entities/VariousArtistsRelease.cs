namespace Tuneage.Domain.Entities
{
    public class VariousArtistsRelease : Release
    {
        public virtual Artist[] Artists
        {
            get;
            set;
        }
    }
}
