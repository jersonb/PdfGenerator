using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace PdfGenerator.Pdf
{
    public abstract class PdfDocument
    {

        public float PositionY { get; private set; } = 770;

        public float CenterX
           => Document.PageSize.Width / 2;

        public Document Document { get; }

        public PdfContentByte ContentByte { get; }

        protected PdfDocument(MemoryStream memoryStream, Document document)
        {
            Document = document;
            var pdfWriter = PdfWriter.GetInstance(document, memoryStream);
            pdfWriter.CloseStream = false;
            Document.Open();
            ContentByte = pdfWriter.DirectContent;
        }


        public void CloseDocument()
           => Document.Close();


        public float GetNextPosition()
        {
            SetAtualPosition(PositionY - 15);
            return PositionY;
        }

        protected void SetAtualPosition(float position)
        {
            PositionY = position;
            ContentByte.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 12);
            ContentByte.BeginText();
            ContentByte.ShowTextAligned(Element.ALIGN_LEFT, " ", 25, PositionY, 0);
            ContentByte.EndText();
        }

        protected void CheckPosition(float minPosition)
        {
            if (minPosition > (PositionY - Document.BottomMargin))
            {
                ContentByte.ClosePath();
                ContentByte.NewPath();
                Document.NewPage();
                SetAtualPosition(770);
            }
        }

    }
}
