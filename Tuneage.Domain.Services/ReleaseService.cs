using Tuneage.Domain.Entities;

namespace Tuneage.Domain.Services
{
    public interface IReleaseService
    {
        Release TransformReleaseForCreation(Release newRelease);
        SingleArtistRelease TransformSingleArtistReleaseForUpdate(SingleArtistRelease preExistingRelease, Release modifiedRelease);
        VariousArtistsRelease TransformVariousArtistsReleaseForUpdate(VariousArtistsRelease preExistingRelease, Release modifiedRelease);
    }

    public class ReleaseService : IReleaseService
    {
        public virtual Release TransformReleaseForCreation(Release newRelease)
        {
            Release transformedRelease;

            if (newRelease.IsByVariousArtists)
                transformedRelease = new VariousArtistsRelease
                {
                    ReleaseId = newRelease.ReleaseId,
                    LabelId = newRelease.LabelId,
                    Title = newRelease.Title,
                    YearReleased = newRelease.YearReleased,
                    IsByVariousArtists = true
                };
            else
                transformedRelease = new SingleArtistRelease
                {
                    ReleaseId = newRelease.ReleaseId,
                    LabelId = newRelease.LabelId,
                    Title = newRelease.Title,
                    YearReleased = newRelease.YearReleased,
                    ArtistId = newRelease.ArtistId,
                    IsByVariousArtists = false
                };

            return transformedRelease;
        }

        public virtual SingleArtistRelease TransformSingleArtistReleaseForUpdate(SingleArtistRelease preExistingRelease,
            Release modifiedRelease)
        {
            preExistingRelease.LabelId = modifiedRelease.LabelId;
            preExistingRelease.Title = modifiedRelease.Title;
            preExistingRelease.YearReleased = modifiedRelease.YearReleased;
            preExistingRelease.ArtistId = modifiedRelease.ArtistId;
            preExistingRelease.IsByVariousArtists = modifiedRelease.IsByVariousArtists;

            return preExistingRelease;
        }

        public virtual VariousArtistsRelease TransformVariousArtistsReleaseForUpdate(VariousArtistsRelease preExistingRelease,
            Release modifiedRelease)
        {
            preExistingRelease.LabelId = modifiedRelease.LabelId;
            preExistingRelease.Title = modifiedRelease.Title;
            preExistingRelease.YearReleased = modifiedRelease.YearReleased;
            preExistingRelease.IsByVariousArtists = modifiedRelease.IsByVariousArtists;
            preExistingRelease.ArtistId = modifiedRelease.ArtistId;

            return preExistingRelease;
        }
    }
}
