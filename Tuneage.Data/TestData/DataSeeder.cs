using System.Threading.Tasks;
using Tuneage.Data.Repositories.Sql.EfCore;

namespace Tuneage.Data.TestData
{
    public class DataSeeder
    {
        private readonly ILabelRepository _labelRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly IReleaseRepository _releaseRepository;

        public DataSeeder(ILabelRepository labelRepository, IArtistRepository artistRepository, IReleaseRepository releaseRepository)
        {
            _labelRepository = labelRepository;
            _artistRepository = artistRepository;
            _releaseRepository = releaseRepository;
        }

        public async Task Seed(bool isIntegrationTest)
        {
            bool removeIdsSoValuesCanBeGenerated = !isIntegrationTest;

            foreach (var label in TestDataGraph.Labels.LabelsRaw)
            {
                if (removeIdsSoValuesCanBeGenerated) label.LabelId = 0;
                await _labelRepository.Create(label);
            }

            foreach (var artist in TestDataGraph.Artists.ArtistsRaw)
            {
                if (removeIdsSoValuesCanBeGenerated) artist.ArtistId = 0;
                await _artistRepository.Create(artist);
            }

            foreach (var release in TestDataGraph.Releases.ReleasesRaw)
            {
                if (removeIdsSoValuesCanBeGenerated) release.ReleaseId = 0;
                await _releaseRepository.Create(release);
            }
        }
    }
}
