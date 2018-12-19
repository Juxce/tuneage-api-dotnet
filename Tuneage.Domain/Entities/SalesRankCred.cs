namespace Tuneage.Domain.Entities
{
    public class SalesRankCred : NewsworthyCred
    {
        public virtual SalesRank[] SalesRanks
        {
            get;
            set;
        }
    }
}
