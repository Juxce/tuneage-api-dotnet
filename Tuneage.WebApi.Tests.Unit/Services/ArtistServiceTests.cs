using System;
using System.Collections.Generic;
using System.Text;
using Tuneage.Data.Repositories.Sql.EfCore;
using Tuneage.Domain.Entities;
using Xunit;

namespace Tuneage.WebApi.Tests.Unit.Services
{
    public interface IArtistService
    {
        Artist TransformArtistForSave(Artist sourceArtist);
    }

    public class ArtistService : IArtistService
    {
        public Artist TransformArtistForSave(Artist sourceArtist)
        {
            // Bulk of logic here?!
            return new Artist();
        }
    }
}
