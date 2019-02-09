﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tuneage.Data.Orm.EF.DataContexts;

namespace Tuneage.Data.Migrations
{
    [DbContext(typeof(TuneageDataContext))]
    [Migration("20190209054734_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Tuneage.Domain.Entities.Artist", b =>
                {
                    b.Property<int>("ArtistId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ArtistSubtype")
                        .IsRequired();

                    b.Property<bool>("IsBand");

                    b.Property<bool>("IsPrinciple")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int?>("PrincipalArtistId");

                    b.HasKey("ArtistId");

                    b.ToTable("Artists");

                    b.HasDiscriminator<string>("ArtistSubtype").HasValue("Artist");
                });

            modelBuilder.Entity("Tuneage.Domain.Entities.ArtistVariousArtistsRelease", b =>
                {
                    b.Property<int>("ArtistId");

                    b.Property<int>("VariousArtistsReleaseId");

                    b.Property<int?>("ArtistId1");

                    b.HasKey("ArtistId", "VariousArtistsReleaseId");

                    b.HasIndex("ArtistId1");

                    b.HasIndex("VariousArtistsReleaseId");

                    b.ToTable("ArtistVariousArtistsReleases");
                });

            modelBuilder.Entity("Tuneage.Domain.Entities.Label", b =>
                {
                    b.Property<int>("LabelId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<string>("WebsiteUrl");

                    b.HasKey("LabelId");

                    b.ToTable("Labels");
                });

            modelBuilder.Entity("Tuneage.Domain.Entities.Release", b =>
                {
                    b.Property<int>("ReleaseId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ArtistId");

                    b.Property<bool>("IsByVariousArtists");

                    b.Property<int>("LabelId");

                    b.Property<string>("ReleaseType")
                        .IsRequired();

                    b.Property<DateTime?>("ReleasedOn");

                    b.Property<string>("Title");

                    b.Property<int>("YearReleased");

                    b.HasKey("ReleaseId");

                    b.HasIndex("LabelId");

                    b.ToTable("Releases");

                    b.HasDiscriminator<string>("ReleaseType").HasValue("Release");
                });

            modelBuilder.Entity("Tuneage.Domain.Entities.AliasedArtist", b =>
                {
                    b.HasBaseType("Tuneage.Domain.Entities.Artist");

                    b.HasDiscriminator().HasValue("Alias");
                });

            modelBuilder.Entity("Tuneage.Domain.Entities.Band", b =>
                {
                    b.HasBaseType("Tuneage.Domain.Entities.Artist");

                    b.HasDiscriminator().HasValue("Band");
                });

            modelBuilder.Entity("Tuneage.Domain.Entities.SoloArtist", b =>
                {
                    b.HasBaseType("Tuneage.Domain.Entities.Artist");

                    b.HasDiscriminator().HasValue("Solo");
                });

            modelBuilder.Entity("Tuneage.Domain.Entities.SingleArtistRelease", b =>
                {
                    b.HasBaseType("Tuneage.Domain.Entities.Release");

                    b.HasIndex("ArtistId");

                    b.HasDiscriminator().HasValue("SA");
                });

            modelBuilder.Entity("Tuneage.Domain.Entities.VariousArtistsRelease", b =>
                {
                    b.HasBaseType("Tuneage.Domain.Entities.Release");

                    b.HasIndex("ArtistId");

                    b.HasDiscriminator().HasValue("VA");
                });

            modelBuilder.Entity("Tuneage.Domain.Entities.ArtistVariousArtistsRelease", b =>
                {
                    b.HasOne("Tuneage.Domain.Entities.Artist", "Artist")
                        .WithMany()
                        .HasForeignKey("ArtistId1");

                    b.HasOne("Tuneage.Domain.Entities.VariousArtistsRelease", "VariousArtistRelease")
                        .WithMany("ArtistVariousArtistsReleases")
                        .HasForeignKey("VariousArtistsReleaseId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Tuneage.Domain.Entities.Release", b =>
                {
                    b.HasOne("Tuneage.Domain.Entities.Label", "Label")
                        .WithMany()
                        .HasForeignKey("LabelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Tuneage.Domain.Entities.SingleArtistRelease", b =>
                {
                    b.HasOne("Tuneage.Domain.Entities.Artist", "Artist")
                        .WithMany()
                        .HasForeignKey("ArtistId");
                });

            modelBuilder.Entity("Tuneage.Domain.Entities.VariousArtistsRelease", b =>
                {
                    b.HasOne("Tuneage.Domain.Entities.Artist", "Artist")
                        .WithMany()
                        .HasForeignKey("ArtistId");
                });
#pragma warning restore 612, 618
        }
    }
}