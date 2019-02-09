namespace Tuneage.Domain.Entities
{
    public class PrincipalArtist : ConceptualArtist
    {
        public virtual AliasedArtist[] AliasedArtists
        {
            get;
            set;
        }
    }
}
