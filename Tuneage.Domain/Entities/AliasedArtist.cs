namespace Tuneage.Domain.Entities
{
    public class AliasedArtist : ConceptualArtist
    {
        public virtual int PrincipleArtistId
        {
            get;
            set;
        }

        public virtual PrincipleArtist PrincipleArtist
        {
            get;
            set;
        }
    }
}
