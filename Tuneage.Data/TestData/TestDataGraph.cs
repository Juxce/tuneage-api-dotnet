using System.Collections.Generic;
using Tuneage.Domain.Entities;

namespace Tuneage.Data.TestData
{
    public class TestDataGraph
    {
        public class Labels
        {
            public static readonly Label Label01 = new Label { LabelId = 1, Name = "Mercury", WebsiteUrl = "www.islandrecords.com/labels/mercury" };
            public static readonly Label Label02 = new Label { LabelId = 2, Name = "Atlantic", WebsiteUrl = "www.alanticrecords.com" };
            public static readonly Label Label03 = new Label { LabelId = 3, Name = "Capitol", WebsiteUrl = "www.capitolrecords.com" };
            public static readonly Label Label04 = new Label { LabelId = 4, Name = "[adult swim]", WebsiteUrl = "www.adultswim.com" };
            public static readonly Label Label05 = new Label { LabelId = 5, Name = "Fat Wreck Chords", WebsiteUrl = "www.fatwreck.com" };
            public static readonly Label Label06 = new Label { LabelId = 6, Name = "Blue Note", WebsiteUrl = "www.bluenote.com" };
            public static readonly Label Label07 = new Label { LabelId = 7, Name = "ABB", WebsiteUrl = "www.abbrecords.com" };
            public static readonly Label Label08 = new Label { LabelId = 8, Name = "Lookout!", WebsiteUrl = "www.lookoutrecords.com" };
            public static readonly Label Label09 = new Label { LabelId = 9, Name = "A.O.I.", WebsiteUrl = "www.wearedelasoul.com" };
            public static readonly Label Label10 = new Label { LabelId = 10, Name = "ABC", WebsiteUrl = "en.wikipedia.org/wiki/ABC_Records" };
            public static readonly Label Label11 = new Label { LabelId = 11, Name = "Asylum", WebsiteUrl = "www.asylumrecords.com" };
            public static readonly Label Label12 = new Label { LabelId = 12, Name = "75 Girls", WebsiteUrl = "www.discogs.com/label/27733-75-Girls-Records-and-Tapes" };
            public static readonly Label Label13 = new Label { LabelId = 13, Name = "300 Entertainment", WebsiteUrl = "www.300ent.com" };
            public static readonly Label Label14 = new Label { LabelId = 14, Name = "Rawkus", WebsiteUrl = "en.wikipedia.org/wiki/Rawkus_Records" };
            public static readonly Label Label15 = new Label { LabelId = 15, Name = "Stones Throw", WebsiteUrl = "www.stonesthrow.com" };
            public static readonly Label Label16 = new Label { LabelId = 16, Name = "Epitaph", WebsiteUrl = "www.epitaph.com" };

            public static readonly List<Label> LabelsRaw = new List<Label>
            {
                Label01, Label02, Label03, Label04, Label05, Label06, Label07, Label08, Label09, Label10, Label11, Label12, Label13, Label14, Label15, Label16
            };

            public static readonly List<Label> LabelsAlphabetizedByLabelName = new List<Label>
            {
                Label04, Label13, Label12, Label09, Label07, Label10, Label11, Label02, Label06, Label03, Label16, Label05, Label08, Label01, Label14, Label15
            };

            public static readonly Label ExistingLabel = Label04;

            public static readonly Label NewLabel = new Label { Name = "Learning Curve", WebsiteUrl = "www.learningcurverecords.com" };

            public static readonly Label UpdatedLabel = new Label { LabelId = 6, Name = "Blue Note MODIFIED", WebsiteUrl = "www.bluenoteMODIFIED.com" };

            public const int NonExistentLabelId = 8675309;
        }

        public class Artists
        {
            public static readonly SoloArtist    Artist01 = new SoloArtist    { ArtistId = 1, Name = "Chuck D" };
            public static readonly SoloArtist    Artist02 = new SoloArtist    { ArtistId = 2, Name = "John Mellencamp" };
            public static readonly Band          Artist03 = new Band          { ArtistId = 3, Name = "Bad Religion" };
            public static readonly Band          Artist04 = new Band          { ArtistId = 4, Name = "The Beat Junkies" };
            public static readonly SoloArtist    Artist05 = new SoloArtist    { ArtistId = 5, Name = "Eminem" };
            public static readonly Band          Artist06 = new Band          { ArtistId = 6, Name = "The High & Mighty" };
            public static readonly SoloArtist    Artist07 = new SoloArtist    { ArtistId = 7, Name = "Pharoahe Monch" };
            public static readonly SoloArtist    Artist08 = new SoloArtist    { ArtistId = 8, Name = "R.A. The Rugged Man" };
            public static readonly SoloArtist    Artist09 = new SoloArtist    { ArtistId = 9, Name = "J-Live" };
            public static readonly SoloArtist    Artist10 = new SoloArtist    { ArtistId = 10, Name = "Kid Capri" };
            public static readonly Band          Artist11 = new Band          { ArtistId = 11, Name = "Medina Green" };
            public static readonly SoloArtist    Artist12 = new SoloArtist    { ArtistId = 12, Name = "Marley Marl" };
            public static readonly SoloArtist    Artist13 = new SoloArtist    { ArtistId = 13, Name = "Sir Menelik" };
            public static readonly Band          Artist14 = new Band          { ArtistId = 14, Name = "Reflection Eternal" };
            public static readonly Band          Artist15 = new Band          { ArtistId = 15, Name = "Dilated Peoples" };
            public static readonly SoloArtist    Artist16 = new SoloArtist    { ArtistId = 16, Name = "Thirstin Howl III" };
            public static readonly Band          Artist17 = new Band          { ArtistId = 17, Name = "Company Flow" };
            public static readonly SoloArtist    Artist18 = new SoloArtist    { ArtistId = 18, Name = "Q-Tip" };
            public static readonly SoloArtist    Artist19 = new SoloArtist    { ArtistId = 19, Name = "Common" };
            public static readonly SoloArtist    Artist20 = new SoloArtist    { ArtistId = 20, Name = "Diamond D" };
            public static readonly SoloArtist    Artist21 = new SoloArtist    { ArtistId = 21, Name = "Mos Def" };
            public static readonly SoloArtist    Artist22 = new SoloArtist    { ArtistId = 22, Name = "Shabaam Sahdeeq" };
            public static readonly SoloArtist    Artist23 = new SoloArtist    { ArtistId = 23, Name = "Talib Kweli" };
            public static readonly Band          Artist24 = new Band          { ArtistId = 24, Name = "Black Star" };
            public static readonly SoloArtist    Artist25 = new SoloArtist    { ArtistId = 25, Name = "Madlib" };
            public static readonly AliasedArtist Artist26 = new AliasedArtist { ArtistId = 26, Name = "Quasimoto", PrincipleArtistId = 25, IsBand = false };
            public static readonly SoloArtist    Artist27 = new SoloArtist    { ArtistId = 27, Name = "King T" };
            public static readonly Band          Artist28 = new Band          { ArtistId = 28, Name = "The Beach Boys" };
            public static readonly SoloArtist    Artist29 = new SoloArtist    { ArtistId = 29, Name = "Homeboy Sandman" };
            public static readonly Band          Artist30 = new Band          { ArtistId = 30, Name = "Yes" };
            public static readonly Band          Artist31 = new Band          { ArtistId = 31, Name = "ASTR" };
            public static readonly Band          Artist32 = new Band          { ArtistId = 32, Name = "Danger Doom" };
            public static readonly SoloArtist    Artist33 = new SoloArtist    { ArtistId = 33, Name = "Too $hort" };
            public static readonly Band          Artist34 = new Band          { ArtistId = 34, Name = "NOFX" };
            public static readonly Band          Artist35 = new Band          { ArtistId = 35, Name = "Lagwagon" };
            public static readonly Band          Artist36 = new Band          { ArtistId = 36, Name = "Eagles" };
            public static readonly SoloArtist    Artist37 = new SoloArtist    { ArtistId = 37, Name = "John Coltrane" };
            public static readonly Band          Artist38 = new Band          { ArtistId = 38, Name = "Steely Dan" };
            public static readonly Band          Artist39 = new Band          { ArtistId = 39, Name = "Little Brother" };
            public static readonly Band          Artist40 = new Band          { ArtistId = 40, Name = "De La Soul" };
            public static readonly Band          Artist41 = new Band          { ArtistId = 41, Name = "Operation Ivy" };
            public static readonly Band          Artist42 = new Band          { ArtistId = 42, Name = "Green Day" };
            public static readonly AliasedArtist Artist43 = new AliasedArtist { ArtistId = 43, Name = "Foxboro Hottubs", PrincipleArtistId = 42, IsBand = true };

            public static readonly List<Artist> ArtistsRaw = new List<Artist>
            {
                Artist01, Artist02, Artist03, Artist04, Artist05, Artist06, Artist07, Artist08, Artist09, Artist10,
                Artist11, Artist12, Artist13, Artist14, Artist15, Artist16, Artist17, Artist18, Artist19, Artist20,
                Artist21, Artist22, Artist23, Artist24, Artist25, Artist26, Artist27, Artist28, Artist29, Artist30,
                Artist31, Artist32, Artist33, Artist34, Artist35, Artist36, Artist37, Artist38, Artist39, Artist40,
                Artist41, Artist42, Artist43
            };

            public static readonly List<Artist> ArtistsAlphabetizedByArtistName = new List<Artist>
            {
                Artist31, Artist03, Artist24, Artist01, Artist19, Artist17, Artist32, Artist40, Artist20, Artist15,
                Artist36, Artist05, Artist43, Artist42, Artist29, Artist09, Artist37, Artist02, Artist10, Artist27,
                Artist35, Artist39, Artist25, Artist12, Artist11, Artist21, Artist34, Artist41, Artist07, Artist18,
                Artist26, Artist08, Artist14, Artist22, Artist13, Artist38, Artist23, Artist28, Artist04, Artist06,
                Artist16, Artist33, Artist30
            };

            public static readonly Artist ExistingArtist = Artist34;

            public static readonly Artist NewBand = new Artist { ArtistId = 44, Name = "People Under The Stairs", IsBand = true, IsPrinciple = true };

            public static readonly Artist NewSoloArtist = new Artist { ArtistId = 45, Name = "Frank Zappa", IsBand = false, IsPrinciple = true };

            public static readonly Artist NewAliasedArtist = new Artist { ArtistId = 46, Name = "Crustified Dibbs", IsBand = false, IsPrinciple = false, PrincipleArtistId = 8 };

            public static readonly Artist UpdatedSoloArtist = new SoloArtist() { ArtistId = 18, Name = "Q-Tip UPDATED", IsBand = true, IsPrinciple = false };

            public static readonly Artist UpdatedBand = new Band { ArtistId = 17, Name = "Company Flow UPDATED", IsBand = false, IsPrinciple = false };

            public static readonly Artist UpdatedAlias = new AliasedArtist() { ArtistId = 26, Name = "Quasimoto UPDATED", IsBand = true, IsPrinciple = true, PrincipleArtistId = null };

            public const int NonExistentArtistId = 8675309;
        }

        public class Releases
        {
            public static readonly SingleArtistRelease   Release01 = new SingleArtistRelease       { ReleaseId = 1, LabelId = 1, Title = "Autobiography Of Mistachuck", YearReleased = 1996, ArtistId = 1 };
            public static readonly SingleArtistRelease   Release02 = new SingleArtistRelease       { ReleaseId = 2, LabelId = 1, Title = "Dance Naked", YearReleased = 1994, ArtistId = 2 };
            public static readonly SingleArtistRelease   Release03 = new SingleArtistRelease       { ReleaseId = 3, LabelId = 2, Title = "The New America", YearReleased = 2000, ArtistId = 3 };
            public static readonly VariousArtistsRelease Release04 = new VariousArtistsRelease     { ReleaseId = 4, LabelId = 14, Title = "Rawkus Presents Soundbombing II", YearReleased = 1999, IsByVariousArtists = true, };
            public static readonly SingleArtistRelease   Release05 = new SingleArtistRelease       { ReleaseId = 5, LabelId = 3, Title = "Tha Triflin' Album", YearReleased = 1993, ArtistId = 27 };
            public static readonly SingleArtistRelease   Release06 = new SingleArtistRelease       { ReleaseId = 6, LabelId = 3, Title = "Surfin' Safari", YearReleased = 1962, ArtistId = 28 };
            public static readonly SingleArtistRelease   Release07 = new SingleArtistRelease       { ReleaseId = 7, LabelId = 15, Title = "The Unseen", YearReleased = 2000, ArtistId = 26 };
            public static readonly SingleArtistRelease   Release08 = new SingleArtistRelease       { ReleaseId = 8, LabelId = 15, Title = "Veins", YearReleased = 2017, ArtistId = 29 };
            public static readonly SingleArtistRelease   Release09 = new SingleArtistRelease       { ReleaseId = 9, LabelId = 2, Title = "The Yes Album", YearReleased = 1971, ArtistId = 30 };
            public static readonly SingleArtistRelease   Release10 = new SingleArtistRelease       { ReleaseId = 10, LabelId = 13, Title = "Homecoming [EP]", YearReleased = 2015, ArtistId = 31 };
            public static readonly SingleArtistRelease   Release11 = new SingleArtistRelease       { ReleaseId = 11, LabelId = 4, Title = "Occult Hymn [EP]", YearReleased = 2006, ArtistId = 32 };
            public static readonly SingleArtistRelease   Release12 = new SingleArtistRelease       { ReleaseId = 12, LabelId = 12, Title = "Don't Stop Rappin'", YearReleased = 1983, ArtistId = 33 };
            public static readonly SingleArtistRelease   Release13 = new SingleArtistRelease       { ReleaseId = 13, LabelId = 16, Title = "Punk In Drublic", YearReleased = 1994, ArtistId = 34 };
            public static readonly SingleArtistRelease   Release14 = new SingleArtistRelease       { ReleaseId = 14, LabelId = 5, Title = "Hoss", YearReleased = 1995, ArtistId = 35 };
            public static readonly SingleArtistRelease   Release15 = new SingleArtistRelease       { ReleaseId = 15, LabelId = 5, Title = "The War On Errorism", YearReleased = 2003, ArtistId = 34 };
            public static readonly SingleArtistRelease   Release16 = new SingleArtistRelease       { ReleaseId = 16, LabelId = 11, Title = "The Long Run", YearReleased = 1979, ArtistId = 36 };
            public static readonly SingleArtistRelease   Release17 = new SingleArtistRelease       { ReleaseId = 17, LabelId = 6, Title = "Blue Train", YearReleased = 1957, ArtistId = 37 };
            public static readonly SingleArtistRelease   Release18 = new SingleArtistRelease       { ReleaseId = 18, LabelId = 10, Title = "Aja", YearReleased = 1977, ArtistId = 38 };
            public static readonly SingleArtistRelease   Release19 = new SingleArtistRelease       { ReleaseId = 19, LabelId = 7, Title = "The Listening", YearReleased = 2002, ArtistId = 39 };
            public static readonly SingleArtistRelease   Release20 = new SingleArtistRelease       { ReleaseId = 20, LabelId = 9, Title = "And The Anonymous Nobody", YearReleased = 2016, ArtistId = 40 };
            public static readonly SingleArtistRelease   Release21 = new SingleArtistRelease       { ReleaseId = 21, LabelId = 8, Title = "Energy", YearReleased = 1989, ArtistId = 41 };

            public static readonly List<Release> ReleasesRaw = new List<Release>
            {
                Release01, Release02, Release03, Release04, Release05, Release06, Release07, Release08, Release09, Release10,
                Release11, Release12, Release13, Release14, Release15, Release16, Release17, Release18, Release19, Release20,
                Release21
            };

            public static readonly List<Release> ReleasesAlphabetizedByTitle = new List<Release>
            {
                Release18, Release20, Release01, Release17, Release02, Release12, Release21, Release10, Release14, Release11,
                Release13, Release04, Release06, Release05, Release19, Release16, Release03, Release07, Release15, Release09,
                Release08
            };

            public static readonly Release ExistingRelease = Release05;

            public static readonly Release NewSingleArtistRelease =
                new Release { ReleaseId = 22, LabelId = 16, Title = "Pump Up The Valuum", YearReleased = 2000, IsByVariousArtists = false, ArtistId = 34 };

            public static readonly Release NewVariousArtistsRelease =
                new Release { ReleaseId = 23, LabelId = 5, Title = "Life In The Fat Lane", YearReleased = 2000, IsByVariousArtists = true };

            public static readonly SingleArtistRelease UpdatedSingleArtistRelease = 
                new SingleArtistRelease { ReleaseId = 9, LabelId = 5, Title = "The Yes Album UPDATED", YearReleased = 1777, ArtistId = 20 };

            public static readonly VariousArtistsRelease UpdatedVariousArtistsRelease =
                new VariousArtistsRelease { ReleaseId = 4, LabelId = 13, Title = "Soundbombing II UPDATED", YearReleased = 1777 };

            public const int NonExistentReleaseId = 666;
        }

        public class ArtistVariousArtistReleases
        {
            private static int _va1 = Releases.Release04.ReleaseId;

            public static readonly ArtistVariousArtistsRelease AVAR01 = new ArtistVariousArtistsRelease { VariousArtistsReleaseId = _va1, ArtistId = Artists.Artist04.ArtistId };
            public static readonly ArtistVariousArtistsRelease AVAR02 = new ArtistVariousArtistsRelease { VariousArtistsReleaseId = _va1, ArtistId = Artists.Artist05.ArtistId };
            public static readonly ArtistVariousArtistsRelease AVAR03 = new ArtistVariousArtistsRelease { VariousArtistsReleaseId = _va1, ArtistId = Artists.Artist06.ArtistId };
            public static readonly ArtistVariousArtistsRelease AVAR04 = new ArtistVariousArtistsRelease { VariousArtistsReleaseId = _va1, ArtistId = Artists.Artist07.ArtistId };
            public static readonly ArtistVariousArtistsRelease AVAR05 = new ArtistVariousArtistsRelease { VariousArtistsReleaseId = _va1, ArtistId = Artists.Artist08.ArtistId };
            public static readonly ArtistVariousArtistsRelease AVAR06 = new ArtistVariousArtistsRelease { VariousArtistsReleaseId = _va1, ArtistId = Artists.Artist09.ArtistId };
            public static readonly ArtistVariousArtistsRelease AVAR07 = new ArtistVariousArtistsRelease { VariousArtistsReleaseId = _va1, ArtistId = Artists.Artist10.ArtistId };
            public static readonly ArtistVariousArtistsRelease AVAR08 = new ArtistVariousArtistsRelease { VariousArtistsReleaseId = _va1, ArtistId = Artists.Artist11.ArtistId };
            public static readonly ArtistVariousArtistsRelease AVAR09 = new ArtistVariousArtistsRelease { VariousArtistsReleaseId = _va1, ArtistId = Artists.Artist12.ArtistId };
            public static readonly ArtistVariousArtistsRelease AVAR10 = new ArtistVariousArtistsRelease { VariousArtistsReleaseId = _va1, ArtistId = Artists.Artist13.ArtistId };
            public static readonly ArtistVariousArtistsRelease AVAR11 = new ArtistVariousArtistsRelease { VariousArtistsReleaseId = _va1, ArtistId = Artists.Artist14.ArtistId };
            public static readonly ArtistVariousArtistsRelease AVAR12 = new ArtistVariousArtistsRelease { VariousArtistsReleaseId = _va1, ArtistId = Artists.Artist15.ArtistId };
            public static readonly ArtistVariousArtistsRelease AVAR13 = new ArtistVariousArtistsRelease { VariousArtistsReleaseId = _va1, ArtistId = Artists.Artist16.ArtistId };
            public static readonly ArtistVariousArtistsRelease AVAR14 = new ArtistVariousArtistsRelease { VariousArtistsReleaseId = _va1, ArtistId = Artists.Artist17.ArtistId };
            public static readonly ArtistVariousArtistsRelease AVAR15 = new ArtistVariousArtistsRelease { VariousArtistsReleaseId = _va1, ArtistId = Artists.Artist18.ArtistId };
            public static readonly ArtistVariousArtistsRelease AVAR16 = new ArtistVariousArtistsRelease { VariousArtistsReleaseId = _va1, ArtistId = Artists.Artist19.ArtistId };
            public static readonly ArtistVariousArtistsRelease AVAR17 = new ArtistVariousArtistsRelease { VariousArtistsReleaseId = _va1, ArtistId = Artists.Artist20.ArtistId };
            public static readonly ArtistVariousArtistsRelease AVAR18 = new ArtistVariousArtistsRelease { VariousArtistsReleaseId = _va1, ArtistId = Artists.Artist21.ArtistId };
            public static readonly ArtistVariousArtistsRelease AVAR19 = new ArtistVariousArtistsRelease { VariousArtistsReleaseId = _va1, ArtistId = Artists.Artist22.ArtistId };
        }
    }
}
