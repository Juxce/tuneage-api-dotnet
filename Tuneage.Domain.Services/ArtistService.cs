using Tuneage.Data.Constants;
using Tuneage.Domain.Entities;

namespace Tuneage.Domain.Services
{
    public interface IArtistService
    {
        Artist TransformArtistForCreation(Artist newArtist);
        //Artist TransformArtistForUpdate(Artist modifiedArtist);
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
                        ArtistId = newArtist.ArtistId, Name = newArtist.Name, IsBand = true, IsPrinciple = newArtist.IsPrinciple
                    };
                else
                    transformedArtist = new SoloArtist()
                    {
                        ArtistId = newArtist.ArtistId, Name = newArtist.Name, IsBand = false, IsPrinciple = newArtist.IsPrinciple
                    };
            }

            return transformedArtist;
        }

        //public virtual Artist TransformArtistForUpdate(Artist modifiedArtist)
        //{

        //    Artist transformedArtist = null;

        //    switch (modifiedArtist.GetType().ToString())
        //    {
        //        case ArtistTypes.SoloArtist:
        //            transformedArtist = new SoloArtist()
        //            {
        //                ArtistId = modifiedArtist.ArtistId, Name = modifiedArtist.Name, IsBand = modifiedArtist.IsBand,
        //                IsPrinciple = modifiedArtist.IsPrinciple, PrincipalArtistId = modifiedArtist.PrincipalArtistId
        //            };
        //            break;
        //        case ArtistTypes.Band:
        //            transformedArtist = new Band()
        //            {
        //                ArtistId = modifiedArtist.ArtistId, Name = modifiedArtist.Name, IsBand = modifiedArtist.IsBand,
        //                IsPrinciple = modifiedArtist.IsPrinciple, PrincipalArtistId = modifiedArtist.PrincipalArtistId
        //            };
        //            break;
        //        case ArtistTypes.AliasedArtist:
        //            transformedArtist = new AliasedArtist()
        //            {
        //                ArtistId = modifiedArtist.ArtistId, Name = modifiedArtist.Name, IsBand = modifiedArtist.IsBand,
        //                IsPrinciple = modifiedArtist.IsPrinciple, PrincipalArtistId = modifiedArtist.PrincipalArtistId
        //            };
        //            break;
        //    }

        //    return transformedArtist;
        //}
    }
}
