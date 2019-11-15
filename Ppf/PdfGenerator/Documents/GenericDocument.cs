using iTextSharp.text;
using PdfGenerator.Models.HeaderAndFooter;
using PdfGenerator.Util;
using System.IO;

namespace PdfGenerator.Documents
{
    public class GenericDocument
    {
        #region Propreties
        protected readonly MemoryStream _ms;
        protected readonly Document _doc;
        protected readonly PdfHelper pdfElemment;

        private HeaderFooterElemment _header;
        private HeaderFooterElemment _footer;
        #endregion

        protected GenericDocument(MemoryStream ms, Document doc)
        {
            _ms = ms;
            _doc = doc;
            _doc.SetPageSize(PageSize.A4);
            _doc.SetMargins(10, 10, 120, 50);
            pdfElemment = new PdfHelper(_doc, _ms);
        }

        public void HeaderAndFooterStructure()
        {
            StructureHeader();
            StructureFooter();
        }

        public void NewPage()
        {
            pdfElemment.NextPage();
            HeaderAndFooterStructure();
        }

        public void FinishPdf()
        {
            _doc.Close();
            var msInfo = _ms.ToArray();
            _ms.Write(msInfo, 0, msInfo.Length);

            _ms.Position = 0;
        }

        #region Header
        public void SetHeader(HeaderFooterElemment header)
        {
            this._header = header;
        }


        private void LogoImageHeader()
        {
            if (this._header.Logo != null)
            {
                pdfElemment.AddImage(this._header.Logo, 0, 790);
            }
        }

        private void StructureHeader()
        {
            LogoImageHeader();

        }
        #endregion

        #region Footer
        public void SetFooter(HeaderFooterElemment footer)
        {
            this._footer = footer;
        }

        private void StructureFooter()
        {
            if (this._footer.Logo != null)
            {
                pdfElemment.AddImage(this._footer.Logo, -25, 0);
            }
        }

        #endregion

    }
}
