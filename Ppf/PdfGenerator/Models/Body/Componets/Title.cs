using iTextSharp.text;

namespace PdfGenerator.Models.Body.Componets
{
    public class Title : BodyElemment
    {
        public string NameFont { get; private set; }
        public BaseColor ColorFont { get; private set; }
        public int SizeFont { get; private set; }
        public float PositionX { get; private set; }
        public float PositionY { get; private set; }

        public static Title CreateTitleBody(string title)
           => new Title()
           {
               TitleBody = title,
               BackColor = BaseColor.ORANGE,
               BoarderColor = BaseColor.WHITE,
               ColorFont = BaseColor.GRAY,
               Spacing = 2,
               HeigthRectangle = 40,
               BoardRadius = 1f,
               MinSizeHeight = 50f,
               SizeFont = 18,
               BackOpacity = 0.4f,
           };

        public static Title CreateTitleBodyDetail(string title)
            => new Title()
            {
                TitleBody = title,
                BackColor = BaseColor.ORANGE,
                BoarderColor = BaseColor.WHITE,
                ColorFont = BaseColor.GRAY,
                Spacing = 2,
                HeigthRectangle = 40,
                BoardRadius = 1f,
                MinSizeHeight = 50f,
                SizeFont = 18,
                BackOpacity = 0.4f,
                ShowLine = false
            };


        public static Title CreateTitleValueEvent(string title)
            => new Title()
            {
                TitleBody = title,
                BackColor = BaseColor.GRAY,
                BoarderColor = BaseColor.WHITE,
                ColorFont = BaseColor.BLACK,
                WidthRectangle = 300,
                LowerLeftY = 2,
                Spacing = -140,
                HeigthRectangle = 30,
                BoardRadius = 3f,
                MinSizeHeight = 20f,
                SizeFont = 12,
                ShowLine = false,
                BackOpacity = 0.4f,
            };

        public static Title CreateTitleEventDay(string title)
           => new Title()
           {
               TitleBody = title,
               BackColor = BaseColor.GRAY,
               BoarderColor = BaseColor.WHITE,
               ColorFont = BaseColor.BLACK,
               Spacing = 2,
               LowerLeftY = 4,
               HeigthRectangle = 30,
               BoardRadius = 1f,
               MinSizeHeight = 20f,
               SizeFont = 12,
               ShowLine = false,
               BackOpacity = 0.4f,
           };

    }
}
