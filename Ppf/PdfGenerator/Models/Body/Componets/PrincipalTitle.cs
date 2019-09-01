using iTextSharp.text;

namespace PdfGenerator.Models.Body.Componets
{
    internal class PrincipalTitle : BodyElemment
    {
       
        private PrincipalTitle(string title)
        {
            this.TitleBody = title;
            this.BackColor = BaseColor.GRAY;
            this.BoarderColor = BaseColor.WHITE;
            this.Spacing = 2;
            this.HeigthRectangle = 40;
            this.BoardRadius = 1f;
        }

        public static PrincipalTitle Create(string title)
        {
            return new PrincipalTitle( title);
        }
    }
}
