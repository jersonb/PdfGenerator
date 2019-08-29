using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfGenerator.Models;
using PdfGenerator.Util;
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
            using (var doc = new Document(PageSize.A4))
            {
                doc.SetMargins(45, 45, 60, 45);
                var ms = new MemoryStream();

                var pdf = PdfWriter.GetInstance(doc, ms);
                pdf.CloseStream = false;
                doc.Open();

                var pathLogoHeader = @"..\..\..\test\image_header.jpg";

                var helper = new PdfHelper(doc, pdf);
                var headerFooterConfig = new HeaderFooterConfig(Image.GetInstance(File.Open(pathLogoHeader, FileMode.Open)));

                helper.NewPage(headerFooterConfig, headerFooterConfig);
                helper.NewPage(headerFooterConfig, headerFooterConfig);
               
                //util.AddGrid();


                doc.Close();
                var msInfo = ms.ToArray();
                ms.Write(msInfo, 0, msInfo.Length);

                ms.Position = 0;
                return ms;
            }
        }
    }
}
