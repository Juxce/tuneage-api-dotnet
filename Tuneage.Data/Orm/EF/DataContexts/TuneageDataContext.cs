using Microsoft.EntityFrameworkCore;
using Tuneage.Domain.Entities;

namespace Tuneage.Data.Orm.EF.DataContexts
{
    public class TuneageDataContext : DbContext
    {
        public TuneageDataContext(DbContextOptions<TuneageDataContext> options) : base(options)
        {
        }

        public virtual DbSet<Label> Labels { get; set; }
        public virtual DbSet<PrimaryCredType> PrimaryCredTypes { get; set; }
        //public virtual DbSet<NewsworthyCredType> NewsworthyCredTypes { get; set; }
        //public virtual DbSet<RecordingType> RecordingTypes { get; set; }
        //public virtual DbSet<ConceptualArtistType> ConceptualArtistTypes { get; set; }
        //public virtual DbSet<PopulismArtistType> PopulismArtistTypes { get; set; }
        //public virtual DbSet<ReleaseType> ReleaseTypes { get; set; }
        //public virtual DbSet<Source> Sources { get; set; }
        //public virtual DbSet<SalesRank> SalesRanks { get; set; }
        //public virtual DbSet<Event> Events { get; set; }
        //public virtual DbSet<Instrument> Instruments { get; set; }
        //public virtual DbSet<Individual> Individuals { get; set; }
        //public virtual DbSet<Composer> Composers { get; set; }
        //public virtual DbSet<Composition> Compositions { get; set; }
        //public virtual DbSet<Recording> Recordings { get; set; }
        //public virtual DbSet<Credit> Credits { get; set; }
        //public virtual DbSet<Song> Songs { get; set; }
        //public virtual DbSet<Artist> Artists { get; set; }
        //public virtual DbSet<PopulismArtist> PopulismArtists { get; set; }
        //public virtual DbSet<ConceptualArtist> ConceptualArtists { get; set; }
        //public virtual DbSet<Band> Bands { get; set; }
        //public virtual DbSet<SoloArtist> SoloArtists { get; set; }
        //public virtual DbSet<PrincipleArtist> PrincipleArtists { get; set; }
        //public virtual DbSet<AliasedArtist> AliasedArtists { get; set; }
        //public virtual DbSet<Lineup> Lineups { get; set; }
        //public virtual DbSet<Cred> Creds { get; set; }
        //public virtual DbSet<CoveredByCred> CoveredByCreds { get; set; }
        //public virtual DbSet<NewsworthyCred> NewsworthyCreds { get; set; }
        //public virtual DbSet<ArtistSaidCred> ArtistSaidCreds { get; set; }
        //public virtual DbSet<OutperformCred> OutperformCreds { get; set; }
        //public virtual DbSet<AwardCred> AwardCreds { get; set; }
        //public virtual DbSet<PositiveReviewCred> PositiveReviewCreds { get; set; }
        //public virtual DbSet<WeeksOnChartCred> WeeksOnChartCreds { get; set; }
        //public virtual DbSet<SalesRankCred> SalesRankCreds { get; set; }
        //public virtual DbSet<PerformanceCred> PerformanceCreds { get; set; }
        //public virtual DbSet<Release> Releases { get; set; }
        //public virtual DbSet<SingleArtistRelease> SingleArtistReleases { get; set; }
        //public virtual DbSet<VariousArtistsRelease> VariousArtistsReleases { get; set; }
        //public virtual DbSet<Track> Tracks { get; set; }



        /*
         * Code First domain classes follow Code First naming conventions to define foreign keys and navigation properties
         * for defining most relationships, but some relationship intentions cannot be determined automatically because
         * of things like circular references, so the OnModelCreating method can be overridden to make these relationships
         * explicit when building the database, helping to avoid serialization errors.
         * 
         * Here, we use the fluent API to configure some of our classes so that Code First can map the relationship between
         * all of our tables properly. This fluent API is an alternative to EF's Data Annotations approach.
         * https://docs.microsoft.com/en-us/ef/ef6/modeling/code-first/fluent/relationships
         * https://docs.microsoft.com/en-us/ef/ef6/modeling/code-first/data-annotations
        */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*
             * 12.18.18 - Previous EF6 relationship code has been copied over, but commented out for now, as we focus
             * solely on Label first. Syntax has changed for much of this. Reference these links when time is right:
             *
             * https://docs.microsoft.com/en-us/ef/core/modeling/index
             * https://docs.microsoft.com/en-us/ef/core/modeling/relationships
             */



            ////// Circular relationships
            ////modelBuilder.Entity<SoloArtist>().HasOptional(s => s.Individual).WithOptionalPrincipal();
            ////modelBuilder.Entity<Song>().HasRequired(s => s.Artist).WithMany(a => a.Songs).WillCascadeOnDelete(false);
            ////modelBuilder.Entity<Track>().HasRequired(t => t.Release).WithMany(r => r.Tracks).WillCascadeOnDelete(false);



            ////// Hierarchical type relationships (Table Per Hierarchy Inheritance)
            ////modelBuilder.Entity<Release>()
            ////    .Map<SingleArtistRelease>(m => m.Requires("ReleaseTypeId").HasValue("SINGLE"))
            ////    .Map<VariousArtistsRelease>(m => m.Requires("ReleaseTypeId").HasValue("VARIOUS"));

            ////modelBuilder.Entity<PopulismArtist>()
            ////    .Map<SoloArtist>(m => m.Requires("PopulismArtistTypeId").HasValue("SOLO").IsOptional())
            ////    .Map<Band>(m => m.Requires("PopulismArtistTypeId").HasValue("BAND"));
            ////modelBuilder.Entity<ConceptualArtist>()
            ////    .Map<AliasedArtist>(m => m.Requires("ConceptualArtistTypeId").HasValue("ALIAS").IsOptional())
            ////    .Map<PrincipleArtist>(m => m.Requires("ConceptualArtistTypeId").HasValue("PRINCIPLE"));

            ////modelBuilder.Entity<Cred>()
            ////    .Map<CoveredByCred>(m => m.Requires("PrimaryCredTypeId").HasValue("COVEREDBY").IsOptional())
            ////    .Map<NewsworthyCred>(m => m.Requires("PrimaryCredTypeId").HasValue("NEWS"));
            ////modelBuilder.Entity<NewsworthyCred>()
            ////    .Map<ArtistSaidCred>(m => m.Requires("NewsworthyCredTypeId").HasValue("ARTISTSAID").IsOptional())
            ////    .Map<OutperformCred>(m => m.Requires("NewsworthyCredTypeId").HasValue("OUTPERFORM"))
            ////    .Map<AwardCred>(m => m.Requires("NewsworthyCredTypeId").HasValue("AWARD"))
            ////    .Map<PositiveReviewCred>(m => m.Requires("NewsworthyCredTypeId").HasValue("POSITIVEREVIEW"))
            ////    .Map<WeeksOnChartCred>(m => m.Requires("NewsworthyCredTypeId").HasValue("WEEKSONCHART"))
            ////    .Map<SalesRankCred>(m => m.Requires("NewsworthyCredTypeId").HasValue("SALESRANK"))
            ////    .Map<PerformanceCred>(m => m.Requires("NewsworthyCredTypeId").HasValue("PERFORMANCE"));



            base.OnModelCreating(modelBuilder);
        }
    }
}
