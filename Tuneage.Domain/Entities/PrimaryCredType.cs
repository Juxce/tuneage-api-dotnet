namespace Tuneage.Domain.Entities
{
    public class PrimaryCredType
    {
        public virtual string PrimaryCredTypeId
        {
            get;
            set;
        }
        
        public virtual string Description
        {
            get;
            set;
        }

        public virtual decimal Weight
        {
            get;
            set;
        }

        //public virtual Cred[] Creds
        //{
        //    get;
        //    set;
        //}
    }
}