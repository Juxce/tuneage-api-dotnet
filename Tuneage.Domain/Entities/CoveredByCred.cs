namespace Tuneage.Domain.Entities
{
    public class CoveredByCred : Cred
    {
        public virtual int SongId
        {
            get;
            set;
        }

        public virtual Song Song
        {
            get;
            set;
        }
    }
}