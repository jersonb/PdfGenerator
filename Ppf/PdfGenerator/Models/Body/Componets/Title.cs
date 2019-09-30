using iTextSharp.text;

namespace PdfGenerator.Models.Body.Componets
{
    public class Title : BodyElemment
    {
        public string NameFont { get; private set; }
        public int SizeFont { get;private set; }
        public float PositionX { get; private set; }
        public float PositionY { get; private set; }

        public static Title CreateTitleBody(string title)
        {
            return new Title()
            {
                TitleBody = title,
                BackColor = BaseColor.GRAY,
                BoarderColor = BaseColor.WHITE,
                Spacing = 2,
                HeigthRectangle = 40,
                BoardRadius = 1f,
            };
        }

       

    }
}
