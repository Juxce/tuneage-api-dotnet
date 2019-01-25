﻿using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tuneage.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    ArtistId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    IsBand = table.Column<bool>(nullable: false),
                    IsPrinciple = table.Column<bool>(nullable: false),
                    CredScoreSum = table.Column<decimal>(nullable: false),
                    PopulismType = table.Column<string>(nullable: false),
                    PrincipleArtistId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.ArtistId);
                });

            migrationBuilder.CreateTable(
                name: "Labels",
                columns: table => new
                {
                    LabelId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    WebsiteUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Labels", x => x.LabelId);
                });

            migrationBuilder.CreateTable(
                name: "PrimaryCredTypes",
                columns: table => new
                {
                    PrimaryCredTypeId = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Weight = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimaryCredTypes", x => x.PrimaryCredTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Releases",
                columns: table => new
                {
                    ReleaseId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    YearReleased = table.Column<int>(nullable: false),
                    ReleasedOn = table.Column<DateTime>(nullable: false),
                    LabelId = table.Column<int>(nullable: false),
                    IsByVariousArtists = table.Column<bool>(nullable: false),
                    ReleaseType = table.Column<string>(nullable: false),
                    ArtistId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Releases", x => x.ReleaseId);
                    table.ForeignKey(
                        name: "FK_Releases_Labels_LabelId",
                        column: x => x.LabelId,
                        principalTable: "Labels",
                        principalColumn: "LabelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Releases_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "ArtistId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtistVariousArtistsReleases",
                columns: table => new
                {
                    ArtistId = table.Column<int>(nullable: false),
                    VariousArtistsReleaseId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistVariousArtistsReleases", x => new { x.ArtistId, x.VariousArtistsReleaseId });
                    table.ForeignKey(
                        name: "FK_ArtistVariousArtistsReleases_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "ArtistId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArtistVariousArtistsReleases_Releases_VariousArtistsReleaseId",
                        column: x => x.VariousArtistsReleaseId,
                        principalTable: "Releases",
                        principalColumn: "ReleaseId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtistVariousArtistsReleases_VariousArtistsReleaseId",
                table: "ArtistVariousArtistsReleases",
                column: "VariousArtistsReleaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Releases_LabelId",
                table: "Releases",
                column: "LabelId");

            migrationBuilder.CreateIndex(
                name: "IX_Releases_ArtistId",
                table: "Releases",
                column: "ArtistId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtistVariousArtistsReleases");

            migrationBuilder.DropTable(
                name: "PrimaryCredTypes");

            migrationBuilder.DropTable(
                name: "Releases");

            migrationBuilder.DropTable(
                name: "Labels");

            migrationBuilder.DropTable(
                name: "Artists");
        }
    }
}
