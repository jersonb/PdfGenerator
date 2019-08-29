using iTextSharp.text;

namespace PdfGenerator.Models
{
    internal class HeaderFooterConfig
    {
        public HeaderFooterConfig(Image logo)
        {
            Logo = logo;
        }

        public HeaderFooterConfig()
        {
        }

        public Image Logo { get; private set; }
        public BaseColor BackColor { get;  set; }
        public BaseColor BoardeColor { get;  set; }
        public string Title { get;  set; }
        public string OtherInformation { get; set; }
        public int PageNUmber { get; private set; }

        public bool ShowBoarder { get; set; }
        public float Spacing { get; set; }
        public int BoarderWidth { get; set; }
        public float BoardRadius { get; set; }
        public float LowerLeftX { get; set; }
        public float LowerLeftY { get; set; }
        public float WidthRectangle { get; set; }
        public float HeigthRectangle { get; set; }

       
    }
}
