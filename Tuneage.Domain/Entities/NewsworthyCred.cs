using System;

namespace Tuneage.Domain.Entities
{
    public abstract class NewsworthyCred : Cred
    {
        public virtual int SourceId
        {
            get;
            set;
        }

        public virtual DateTime OccuredOn
        {
            get;
            set;
        }

        public virtual Source Source
        {
            get;
            set;
        }
    }
}