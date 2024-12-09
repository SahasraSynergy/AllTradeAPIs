namespace TradingAPIs
{
    public class ScrapBSE
    {
        public DateOnly Date { get; set; }

        public string? Summary { get; set; }
        public string? Details { get; set; }
        public string? Announcements { get; set; }


    }
    // Define a model for announcements
    public class Announcement
    {
        public string Date { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
    }
}
// Model for storing XBRL data
public class XbrlData
{
    public string EntityIdentifier { get; set; } // Example data extracted from XBRL
    public string RawXml { get; set; } // Raw XML content
}