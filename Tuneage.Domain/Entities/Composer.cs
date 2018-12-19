namespace Tuneage.Domain.Entities
{
    public class Composer : Individual
    {
        public virtual Composition[] Compositions
        {
            get;
            set;
        }
    }
}