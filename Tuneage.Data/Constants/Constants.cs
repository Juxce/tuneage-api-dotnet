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

    public class Explainations
    {
        public const string DbConcurrencyExceptionFromInMemoryDb =
            @"This test yields an internal server error ""DbConcurrencyException: Attempted to update or delete an entity that does not exist
            in the store."" but it is unclear why. It can be verified through debugging that the parent entity, along with all related entities,
            indeed do exist in the db, and on the data context. This error only happens when editing types that are built from subtypes, like
            Arist (constructed via SoloArtist, Band, or AliasedArtist) and Release (constructed via SingleArtistRelease or VariousArtistsRelease).
            The collection is of the parent type, but shows subtypes inside at runtime, as expected, but it doesn't seem like the in-memory
            database acts like the actual MSSQL database does in these instances at runtime. These types of edits work fine when manually testing
            both API and MVC endpoints for these cases, but fail when done in integration tests using the in-memory database. The error happens
            for both API tests that use HttpClient.PutAsync() as well as MVC tests that use HttpClient.PostAsync(). Shrug. UPDATE! Via further
            debugging, it seems that the standard Put methods on the controllers using the base type are the source of this problem. For example,
            when an update is called on a SoloArtist, it will be cast to Artist inside the controller method, and this seems to be what is creating
            errors with InMemoryDb, as it expects that ID to be a SoloArtist, and will error when these don't match. So, PERHAPS the right
            solution here is to create multiple routes, and matching controller methods, for Puts, one for each sub-type that would need to be
            updated.";
    }
}
