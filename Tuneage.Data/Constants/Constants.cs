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

    public class DefaultValues
    {
        public const string VariousArtistsDisplayName = "<Various Artists>";
        public const string ArtistListDefaultForNoSelection = "--- None ---";
    }

    public class ErrorMessages
    {
        public const string ArtistIdForUpdateDoesNotExist = "The ID of the Artist in the update request does not exist.";
        public const string ReleaseIdForUpdateDoesNotExist = "The ID of the Release in the update request does not exist.";
        public const string DbUpdateConcurrencyExceptionDoesNotExist = "DbUpdateConcurrencyException: Attempted to update or delete an entity that does not exist in the store.";
        public const string ArgumentNullException = "ArgumentNullException: Value cannot be null.";
        public const string ArgumentExceptionSameKeyAlreadyAdded = "ArgumentException: An item with the same key has already been added.";
    }

    public class ViewData
    {
        public const string DefaultIndexPageTitle = "<title>Index - Tuneage.WebApi</title>";
        public const string DefaultDetailsPageTitle = "<title>Details - Tuneage.WebApi</title>";
        public const string DefaultCreatePageTitle = "<title>Create - Tuneage.WebApi</title>";
        public const string DefaultDeletePageTitle = "<title>Delete - Tuneage.WebApi</title>";
    }
}
