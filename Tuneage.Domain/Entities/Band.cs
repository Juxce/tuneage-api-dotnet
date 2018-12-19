namespace Tuneage.Domain.Entities
{
    public class Band : PopulismArtist
    {
        public virtual Lineup[] Lineups
        {
            get;
            set;
        }
    }
}