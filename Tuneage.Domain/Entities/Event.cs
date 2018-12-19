using System;

namespace Tuneage.Domain.Entities
{
    public class Event
    {
        public virtual int Id
        {
            get;
            set;
        }

        public virtual string Name
        {
            get;
            set;
        }

        public virtual DateTime Date
        {
            get;
            set;
        }

        public virtual decimal Weight
        {
            get;
            set;
        }

        public virtual PerformanceCred[] PerformanceCreds
        {
            get;
            set;
        }
    }
}