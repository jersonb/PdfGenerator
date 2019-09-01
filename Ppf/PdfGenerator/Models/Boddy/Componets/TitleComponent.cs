using iTextSharp.text;

namespace PdfGenerator.Models.Boddy.Componets
{
    internal class TitleComponent : BoddyElemment
    {
        public TitleComponent(string title)
        {
            this.TitleBoddy = title;
            this.BackColor = BaseColor.GRAY;
            this.BoarderColor = BaseColor.WHITE;
            this.Spacing = 2;
            this.HeigthRectangle = 40;
            this.BoardRadius = 1f;
            
        }

    }
}
