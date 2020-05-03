using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace PdfGenerator.Pdf
{
    public class TextBody : PdfDocument
    {
        private TextBody(MemoryStream memoryStream, Document document) : base(memoryStream, document)
        {
        }

        public static TextBody GetInstance(MemoryStream memoryStream, Document document)
            => new TextBody(memoryStream, document);


        public void TextSetPositionX(string text, int sizeFont, float positionX, BaseColor color = null, string nameFont = BaseFont.HELVETICA, int align = Element.ALIGN_LEFT)
            => Text(text, sizeFont, positionX, PositionY, color, nameFont, align);


        public void TextCenter(string text, int sizeFont, float positionY, BaseColor color = null, string nameFont = BaseFont.HELVETICA)
            => Text(text, sizeFont, CenterX, positionY, color, nameFont, Element.ALIGN_CENTER);


        public void TextLeft(string text, int sizeFont, float positionX, float positionY, BaseColor color = null, string nameFont = BaseFont.HELVETICA)
            => Text(text, sizeFont, positionX, positionY, color, nameFont,Element.ALIGN_LEFT);
          

        public void TextRigth(string text, int sizeFont, float positionX, float positionY, BaseColor color = null, string nameFont = BaseFont.HELVETICA)
            => Text(text, sizeFont, positionX, positionY, color, nameFont, Element.ALIGN_RIGHT);


        public void TextContinuos(string text, int sizeFont, BaseColor color = null, string nameFont = BaseFont.HELVETICA, int align = Element.ALIGN_LEFT)
        {
            Text(text, sizeFont, Document.Left, null, color, nameFont, align);
            GetNextPosition();
        }


        private void Text(string text, int sizeFont, float positionX, float? positionY, BaseColor color, string nameFont, int align)
        {
            CheckPosition(30);
            var font = GetFont(nameFont);
            ContentByte.SetColorFill(color ?? BaseColor.Black);
            ContentByte.SetFontAndSize(font, sizeFont);
            ContentByte.BeginText();
            ContentByte.ShowTextAligned(align, text, positionX, positionY ?? PositionY, 0);
            ContentByte.EndText();
        }


        private BaseFont GetFont(string font)
            => BaseFont.CreateFont(font, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

    }
}
