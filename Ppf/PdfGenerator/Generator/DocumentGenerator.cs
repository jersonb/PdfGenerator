using iTextSharp.text;
using PdfGenerator.Documents;
using PdfGenerator.Models.HeaderAndFooter;
using PdfGenerator.Models.Label;
using System.IO;

namespace PdfGenerator.Generator
{
    public static class DocumentGenerator
    {
        public static MemoryStream Generate()
        {
            var ms = new MemoryStream();

            using (var doc = new Document())
            {
                var especificDocument = EspecificDocument.Generate(ms, doc);

                var pathImageHeader = @"..\..\..\test\image_header.jpg";
                var imageHeader = Image.GetInstance(File.Open(pathImageHeader, FileMode.Open));
                var header = HeaderFooterElemment.Create(imageHeader);
                header.Configure("20/11/19", "555555", "Teste de Geração do PDF");

                especificDocument.SetHeader(header);

                especificDocument.SetDocumentTitle(LabelDocumentTitle.TEST_PDF);

                var pathImageFooter = @"..\..\..\test\image_footer.jpg";
                var imageFooter = Image.GetInstance(File.Open(pathImageFooter, FileMode.Open));
                var footer = HeaderFooterElemment.Create(imageFooter);
                especificDocument.SetFooter(footer);

                return especificDocument.StructureDocument();
            }

        }
    }
}
