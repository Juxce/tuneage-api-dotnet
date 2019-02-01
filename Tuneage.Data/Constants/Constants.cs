using System;
using System.Collections.Generic;
using System.Text;

namespace Tuneage.Data.Constants
{
    public class ArtistTypes
    {
        public const string SoloArtist = "Tuneage.Domain.Entities.SoloArtist";
        public const string Band = "Tuneage.Domain.Entities.Band";
        public const string AliasedArtist = "Tuneage.Domain.Entities.AliasedArtist";
    }
    public class ReleaseTypes
    {
        public const string SingleArtistRelease = "Tuneage.Domain.Entities.SingleArtistRelease";
        public const string VariousArtistsRelease = "Tuneage.Domain.Entities.VariousArtistsRelease";
    }
}
