using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Announcements
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string CompanyName { get; set; }
        public string Catagory { get; set; }
        public string SubCatagory { get; set; }
        public string XBRLLink { get; set; }
        public string PDFLink { get; set; }
        public string AnnouncementDetails { get; set; }
        public DateTime? AnnounementDate { get; set; }
        public DateTime AnnounementCreatedDate { get; set; }

    }
}
