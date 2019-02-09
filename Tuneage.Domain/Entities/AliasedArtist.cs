namespace Tuneage.Domain.Entities
{
    public class AliasedArtist : ConceptualArtist
    {
        public virtual PrincipalArtist PrincipalArtist
        {
            get;
            set;
        }
    }
}
