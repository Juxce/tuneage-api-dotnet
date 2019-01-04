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
            new Label() { LabelId = 3, Name = "Capitol", WebsiteUrl = "www.capitolrecords.com" }
        };

        public static List<Label> LabelsAlphabetizedByLabelName = new List<Label>()
        {
            LabelsRaw[2], LabelsRaw[3], LabelsRaw[1]
        };
    }
}
