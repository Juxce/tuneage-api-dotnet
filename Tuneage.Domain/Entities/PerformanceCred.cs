namespace Tuneage.Domain.Entities
{
    public class PerformanceCred : NewsworthyCred
    {
        public virtual int EventId
        {
            get;
            set;
        }

        public virtual Event Event
        {
            get;
            set;
        }
    }
}
