using iTextSharp.text;
using PdfGenerator.Pdf;
using System.IO;
using Xunit;

namespace PdfGenerator.Test
{
    public class TextTest
    {
        private const string Path = @"..\..\..\test\";

        public TextTest()
        {
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
        }

        [Fact]
        public void TextAlignTest()
        {
            var pathFile = Path + "TextAlign.pdf";
            try
            {
                if (File.Exists(pathFile))
                {
                    File.Delete(pathFile);
                }

                using var memoryStream = new MemoryStream();
                var document = new Document(new Rectangle(0, 0, 0, 0));
                document.SetPageSize(PageSize.A4);

                var textBody = TextBody.GetInstance(memoryStream, document);

                textBody.TextCenter("Text center position 1", 12, textBody.PositionY);
                textBody.TextLeft("Text left position 1", 12, textBody.Document.Left, textBody.PositionY);
                textBody.TextRigth("Text rigth position 1", 12, textBody.Document.Right, textBody.PositionY);

                textBody.CloseDocument();

                var msInfo = memoryStream.ToArray();
                memoryStream.Write(msInfo, 0, msInfo.Length);

                memoryStream.Position = 0;

                using var fileStream = new FileStream(pathFile, FileMode.Create);
                memoryStream.CopyTo(fileStream);
                fileStream.CopyTo(memoryStream);
                fileStream.Close();

                Assert.True(File.Exists(pathFile));
            }
            catch
            {
                throw;
            }

        }

        [Fact]
        public void TextPositionXTest()
        {
            var pathFile = Path + "TextPositionX.pdf";

            var positionsX = new[] { 20, 40, 60, 80, 100, 120, 140, 160, 180, 200, 220, 240, 260 };
            try
            {
                if (File.Exists(pathFile))
                {
                    File.Delete(pathFile);
                }

                using var memoryStream = new MemoryStream();
                var document = new Document(new Rectangle(0, 0, 0, 0));
                document.SetPageSize(PageSize.A4);

                var textBody = TextBody.GetInstance(memoryStream, document);

                foreach (var position in positionsX)
                {

                    textBody.TextSetPositionX($"Text position x = {position}", 12, position);
                    _ = textBody.GetNextPosition();
                }


                textBody.CloseDocument();

                var msInfo = memoryStream.ToArray();
                memoryStream.Write(msInfo, 0, msInfo.Length);

                memoryStream.Position = 0;

                using var fileStream = new FileStream(pathFile, FileMode.Create);
                memoryStream.CopyTo(fileStream);
                fileStream.CopyTo(memoryStream);
                fileStream.Close();

                Assert.True(File.Exists(pathFile));
            }
            catch
            {
                throw;
            }

        }

         [Fact]
        public void TextContinuosTest()
        {
            var pathFile = Path + "TextContinuos.pdf";

            try
            {
                if (File.Exists(pathFile))
                {
                    File.Delete(pathFile);
                }

                using var memoryStream = new MemoryStream();
                var document = new Document(new Rectangle(0, 0, 0, 0));
                document.SetPageSize(PageSize.A4);

                var textBody = TextBody.GetInstance(memoryStream, document);

                for (int i = 0; i < 500; i++)
                {
                   
                    textBody.TextContinuos($"Text Continuos {i}", 12);
                }

                textBody.CloseDocument();

                var msInfo = memoryStream.ToArray();
                memoryStream.Write(msInfo, 0, msInfo.Length);

                memoryStream.Position = 0;

                using var fileStream = new FileStream(pathFile, FileMode.Create);
                memoryStream.CopyTo(fileStream);
                fileStream.CopyTo(memoryStream);
                fileStream.Close();

                Assert.True(File.Exists(pathFile));
            }
            catch
            {
                throw;
            }

        }


    }
}
