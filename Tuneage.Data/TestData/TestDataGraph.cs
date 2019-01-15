using System.Collections.Generic;
using Tuneage.Domain.Entities;

namespace Tuneage.Data.TestData
{
    public class TestDataGraph
    {
        private static readonly Label Label1 = new Label() { LabelId = 1, Name = "Mercury", WebsiteUrl = "www.islandrecords.com/labels/mercury" };
        private static readonly Label Label2 = new Label() { LabelId = 2, Name = "Atlantic", WebsiteUrl = "www.alanticrecords.com" };
        private static readonly Label Label3 = new Label() { LabelId = 3, Name = "Capitol", WebsiteUrl = "www.capitolrecords.com" };
        private static readonly Label Label4 = new Label() { LabelId = 4, Name = "Fat Wreck Chords", WebsiteUrl = "www.fatwreck.com" };
        private static readonly Label Label5 = new Label() { LabelId = 5, Name = "Blue Note", WebsiteUrl = "www.bluenote.com" };
        private static readonly Label Label6 = new Label() { LabelId = 6, Name = "Asylum", WebsiteUrl = "www.asylumrecords.com" };

        public static List<Label> LabelsRaw = new List<Label>()
        {
            Label1, Label2, Label3, Label4, Label5, Label6
        };

        public static List<Label> LabelsAlphabetizedByLabelName = new List<Label>()
        {
            Label6, Label2, Label5, Label3, Label4, Label1
        };

        public static Label LabelExisting = Label4;

        public static Label LabelNew = new Label() { Name = "Learning Curve", WebsiteUrl = "www.learningcurverecords.com/"};

        public static Label LabelUpdated = new Label() { LabelId = 5, Name = "Blue Note MODIFIED", WebsiteUrl = "www.bluenoteMODIFIED.com" };

        public const int LabelIdNonExistent = 8675309;
    }
}
