using iTextSharp.text;
using System.Collections.Generic;
using System.IO;

namespace PdfGenerator.Pdf
{
    public class ImageBody : PdfDocument
    {
        private ImageBody(MemoryStream memoryStream, Document document) : base(memoryStream, document)
        {
        }


        public static ImageBody GetInstance(MemoryStream memoryStream, Document document)
            => new ImageBody(memoryStream, document);

   
        public void AddImage(Image image, float absoluteX, float absoluteY, float width, float height)
        {
            image.ScaleToFit(width, height);
            image.SetAbsolutePosition(absoluteX, absoluteY);

            Document.Add(image);
        }


        public void AddImage(Image image, float absoluteX, float absoluteY)
            => AddImage(image, absoluteX, absoluteY, image.Width, image.Height);


        public void ImagesInLine(List<Image> images)
        {
            int i = 0;
            CheckPosition(200);

            SetAtualPosition(PositionY - 90);

            images.ForEach(image =>
            {
                AddImage(image, GetPositionByIndex(i++), PositionY, (Document.PageSize.Width / 4) - 10, 200);

                if (i.Equals(4))
                {
                    i = 0;
                    SetAtualPosition(PositionY - 120);
                }
            });


            CheckPosition(200);

            SetAtualPosition(PositionY);
        }

        private float GetPositionByIndex(int index)
            => index switch
            {
                0 => Document.Left,
                1 => (Document.PageSize.Width / 4) + 5,
                2 => ((Document.PageSize.Width / 4) * index) + 3,
                3 => (Document.PageSize.Width / 4) * index,
                _ => 0
            };

    }
}
