using Tuneage.Domain.Entities;

namespace Tuneage.Domain.Services
{
    public interface IArtistService
    {
        Artist TransformArtistForCreation(Artist newArtist);
        SoloArtist TransformSoloArtistForUpdate(SoloArtist preExistingArtist, Artist modifiedArtist);
        Band TransformBandForUpdate(Band preExistingArtist, Artist modifiedArtist);
        AliasedArtist TransformAliasForUpdate(AliasedArtist preExistingArtist, Artist modifiedArtist);
    }

    public class ArtistService : IArtistService
    {
        public virtual Artist TransformArtistForCreation(Artist newArtist)
        {
            Artist transformedArtist;

            if (!newArtist.IsPrinciple)
                transformedArtist = new AliasedArtist()
                    {
                        ArtistId = newArtist.ArtistId, Name = newArtist.Name, IsBand = newArtist.IsBand,
                        IsPrinciple = newArtist.IsPrinciple, PrincipalArtistId = newArtist.PrincipalArtistId
                    };
            else
            {
                if (newArtist.IsBand)
                    transformedArtist = new Band()
                    {
                        ArtistId = newArtist.ArtistId, Name = newArtist.Name, IsBand = true, IsPrinciple = true
                    };
                else
                    transformedArtist = new SoloArtist()
                    {
                        ArtistId = newArtist.ArtistId, Name = newArtist.Name, IsBand = false, IsPrinciple = true
                    };
            }

            return transformedArtist;
        }

        public virtual SoloArtist TransformSoloArtistForUpdate(SoloArtist preExistingArtist, Artist modifiedArtist)
        {
            preExistingArtist.Name = modifiedArtist.Name;
            preExistingArtist.IsBand = modifiedArtist.IsBand;
            preExistingArtist.IsPrinciple = modifiedArtist.IsPrinciple;
            preExistingArtist.PrincipalArtistId = modifiedArtist.PrincipalArtistId;

            return preExistingArtist;
        }

        public virtual Band TransformBandForUpdate(Band preExistingArtist, Artist modifiedArtist)
        {
            preExistingArtist.Name = modifiedArtist.Name;
            preExistingArtist.IsBand = modifiedArtist.IsBand;
            preExistingArtist.IsPrinciple = modifiedArtist.IsPrinciple;
            preExistingArtist.PrincipalArtistId = modifiedArtist.PrincipalArtistId;

            return preExistingArtist;
        }

        public virtual AliasedArtist TransformAliasForUpdate(AliasedArtist preExistingArtist, Artist modifiedArtist)
        {
            preExistingArtist.Name = modifiedArtist.Name;
            preExistingArtist.IsBand = modifiedArtist.IsBand;
            preExistingArtist.IsPrinciple = modifiedArtist.IsPrinciple;
            preExistingArtist.PrincipalArtistId = modifiedArtist.PrincipalArtistId;

            return preExistingArtist;
        }
    }
}
