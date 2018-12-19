namespace Tuneage.Domain.Entities
{
    public class PrincipleArtist : ConceptualArtist
    {
        public virtual AliasedArtist[] AliasedArtists
        {
            get;
            set;
        }
    }
}
