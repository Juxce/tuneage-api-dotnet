namespace Tuneage.Domain.Entities
{
    public class Label
    {
        public virtual int LabelId
        {
            get;
            set;
        }

        public virtual string Name
        {
            get;
            set;
        }

        public virtual string WebsiteUrl
        {
            get;
            set;
        }

        //public virtual Release[] Releases
        //{
        //    get;
        //    set;
        //}
    }
}