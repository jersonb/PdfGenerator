using iTextSharp.text;

namespace PdfGenerator.Models.Body
{
    public abstract class BodyElemment 
    {
        public string TitleBody { get; set; }

        public BaseColor BoarderColor { get; set; }
        public BaseColor BackColor { get; set; }

        public bool ShowBoarder { get; set; } = true;

        public float Spacing { get; set; }
        public int BoarderWidth { get; set; }
        public float BoardRadius { get; set; }
        public float LowerLeftX { get; set; }
        public float LowerLeftY { get; set; }
        public float WidthRectangle { get; set; }
        public float HeigthRectangle { get; set; }
    }
}