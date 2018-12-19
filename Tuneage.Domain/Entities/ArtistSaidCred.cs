namespace Tuneage.Domain.Entities
{
    public class ArtistSaidCred : NewsworthyCred
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