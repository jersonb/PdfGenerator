using iTextSharp.text;

namespace PdfGenerator.Models.HeaderAndFooter
{
    internal abstract class HeaderFooterConfig 
    {
        protected HeaderFooterConfig(Image logo)
        {
            Logo = logo;
        }

        public Image Logo { get; private set; }
       
    }
}
