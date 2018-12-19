using System;

namespace Tuneage.Domain.Entities
{
    public class SalesRank
    {
        public virtual int Id
        {
            get;
            set;
        }

        public virtual int Rank
        {
            get;
            set;
        }

        public virtual DateTime Date
        {
            get;
            set;
        }
    }
}