namespace Tuneage.Domain.Entities
{
    public class AliasedArtist : ConceptualArtist
    {
        public virtual PrincipleArtist PrincipleArtist
        {
            get;
            set;
        }
    }
}
