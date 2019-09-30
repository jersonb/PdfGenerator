using iTextSharp.text;

namespace PdfGenerator.Models.HeaderAndFooter
{
    public class HeaderFooterElemment
    {
        private HeaderFooterElemment(Image logo)
        {
            this.Logo = logo;
        }

        public void  Configure(string requestDate, string requestId, string nameEvent)
        {
            this.RequestDate = requestDate;
            this.RequestId = requestId;
            this.NameEvent = nameEvent;
        }

        public static HeaderFooterElemment Create(Image logo) => new HeaderFooterElemment(logo);

        

        public string RequestDate { get; private set; } 
        public string RequestId { get;private set; }
        public string NameEvent { get;private set; }
        public Image Logo { get; private set; }
    }
}
