using System.Collections.Generic;
using Tuneage.Domain.Entities;

namespace Tuneage.Data.TestData
{
    public class TestDataGraph
    {
        public static List<Label> LabelsRaw = new List<Label>()
        {
            new Label() { LabelId = 1, Name = "Mercury", WebsiteUrl = "www.islandrecords.com/labels/mercury" },
            new Label() { LabelId = 2, Name = "Atlantic", WebsiteUrl = "www.alanticrecords.com" },
            new Label() { LabelId = 3, Name = "Capitol", WebsiteUrl = "www.capitolrecords.com" },
<<<<<<< HEAD
            new Label() { LabelId = 4, Name = "Fat Wreck Chords", WebsiteUrl = "www.fatwreck.com" }
=======
            new Label() { LabelId = 4, Name = "Fat Wreck Chords", WebsiteUrl = "www.fatwreck.com" },
            new Label() { LabelId = 5, Name = "Blue Note", WebsiteUrl = "www.bluenote.com" },
            new Label() { LabelId = 6, Name = "Asylum", WebsiteUrl = "www.asylumrecords.com" }
>>>>>>> Add Test Data
        };

        public static List<Label> LabelsAlphabetizedByLabelName = new List<Label>()
        {
<<<<<<< HEAD
            LabelsRaw[2], LabelsRaw[3], LabelsRaw[4], LabelsRaw[1]
=======
            LabelsRaw[6], LabelsRaw[2], LabelsRaw[5], LabelsRaw[3], LabelsRaw[4], LabelsRaw[1]
>>>>>>> Add Test Data
        };
    }
}
