using iTextSharp.text;
using PdfGenerator.Documents;
using PdfGenerator.Models.Boddy.Components;
using PdfGenerator.Models.HeaderAndFooter;
using System.IO;


namespace PdfGenerator.Generator
{
    public class DocumentGenerator
    {
        protected DocumentGenerator()
        {
        }

        public static MemoryStream Generate()
        {
            var ms = new MemoryStream();

            using (var doc = new Document())
            {
                var especificDocument = new EspecificDocument(ms, doc);

                var pathImageHeader = @"..\..\..\test\image_header.jpg";
                var imageHeader = Image.GetInstance(File.Open(pathImageHeader, FileMode.Open));
                var header = new HeaderElemment(imageHeader);
                especificDocument.SetHeader(header);

                var client = new Client();
                especificDocument.SetClient(client);

                var pathImageFooter = @"..\..\..\test\image_footer.jpg";
                var imageFooter = Image.GetInstance(File.Open(pathImageFooter, FileMode.Open));
                var footer = new FooterElemment(imageFooter);
                especificDocument.SetFooter(footer);

                return especificDocument.GetPdf();
            }

        }
    }
}
