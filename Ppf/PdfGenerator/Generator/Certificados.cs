using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfGenerator.Util;
using System.IO;

namespace PdfGenerator.Generator
{
   public static class Certificados
    {
        public static MemoryStream Generate(string nameCertify,string pathPhoto)
        {
            PdfHelper pdfElemment;
            var ms = new MemoryStream();

            using (var doc = new Document())
            {
                doc.SetPageSize(PageSize.A4.Rotate());
                doc.SetMargins(0, 0, 0, 0);
                pdfElemment = new PdfHelper(doc, ms);
                var image = Image.GetInstance(pathPhoto);
                pdfElemment.AddImage(image, 0, 0, doc.PageSize.Width, doc.PageSize.Height + 3);

                pdfElemment.TextCenter(nameCertify, BaseFont.HELVETICA_BOLD, 25, pdfElemment.CenterX(), 350f);

                doc.Close();
                var msInfo = ms.ToArray();
                ms.Write(msInfo, 0, msInfo.Length);

                ms.Position = 0;
                return ms;
            }
        }
    }
}
