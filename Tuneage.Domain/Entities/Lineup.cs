using System;

namespace Tuneage.Domain.Entities
{
    public class Lineup
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

        public virtual int BandId
        {
            get;
            set;
        }
        
        public virtual Individual[] Members
        {
            get;
            set;
        }

        public virtual DateTime[] FormationDates
        {
            get;
            set;
        }

        public virtual Band Band
        {
            get;
            set;
        }
    }
}