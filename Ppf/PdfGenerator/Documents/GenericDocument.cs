using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfGenerator.Models.HeaderAndFooter;
using PdfGenerator.Models.Label;
using PdfGenerator.Util;
using System;
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

        public GenericDocument(MemoryStream ms, Document doc)
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


        private void BasicDocumentData()
        {
            var requestDate = this._header.RequestDate ?? DateTime.Now.ToString("dd/MM/yyyy");
            var requestId = this._header.RequestId ?? "000000";
            var nameEvent = this._header.NameEvent;

            pdfElemment.TextLeft(string.Format("{0} : {1}", LabelHeader.DATA_DA_SOLICITACAO, requestDate), BaseFont.HELVETICA_BOLD, 8, 14, pdfElemment.NextPosition);
            pdfElemment.TextLeft(string.Format("{0} : {1}", LabelHeader.ID_DA_SOLICITACAO, requestId), BaseFont.HELVETICA_BOLD, 8, 14, pdfElemment.NextPosition);
            pdfElemment.TextLeft(string.Format("{0} : {1}", LabelHeader.NOME_DO_EVENTO, nameEvent), BaseFont.HELVETICA_BOLD, 8, 14, pdfElemment.NextPosition);

        }

        private void StructureHeader()
        {
            BasicDocumentData();
         
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
           
            ImageFooter();
        }

        private void ImageFooter()
        {
            if (this._footer.Logo != null)
            {
                pdfElemment.AddImage(this._footer.Logo, -25, 0);
            }
        }

      
        #endregion

    }
}
