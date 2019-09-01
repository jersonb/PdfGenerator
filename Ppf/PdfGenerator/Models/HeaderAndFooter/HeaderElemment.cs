using iTextSharp.text;

namespace PdfGenerator.Models.HeaderAndFooter
{
    internal class HeaderElemment: HeaderFooterConfig
    {
        public HeaderElemment(Image logo) : base(logo)
        {
           
        }

        public string RequestDate { get; set; }
        public string RequestId { get; set; }
        public string NameEvent { get; set; }
    }
}
