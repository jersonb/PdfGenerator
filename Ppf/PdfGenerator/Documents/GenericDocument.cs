using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfGenerator.Interface;
using PdfGenerator.Models.HeaderAndFooter;
using PdfGenerator.Models.Label;
using PdfGenerator.Util;
using System;
using System.IO;

namespace PdfGenerator.Documents
{
    internal class GenericDocument : IGenericStructure
    {
        #region Propreties
        protected readonly MemoryStream _ms;
        protected readonly Document _doc;
        protected readonly PdfHelper pdfElemment;
       
        private HeaderElemment _header;
        private FooterElemment _footer;
        #endregion

        public GenericDocument(MemoryStream ms, Document doc)
        {
            _ms = ms;
            _doc = doc;
            _doc.SetPageSize(PageSize.A4);
            _doc.SetMargins(10, 10, 120, 50);
            pdfElemment = new PdfHelper(_doc, _ms);
        }

        internal void HeaderAndFooterStructure()
        {
            pdfElemment.InitialPosition = 770;

            StructureHeader();
            StructureFooter();
        }

        internal void NewPage()
        {
            pdfElemment.NextPage();
            HeaderAndFooterStructure();
          
        }

        internal void FinishPdf()
        {
            _doc.Close();
            var msInfo = _ms.ToArray();
            _ms.Write(msInfo, 0, msInfo.Length);

            _ms.Position = 0;
        }

        #region Header
        public void SetHeader(HeaderElemment header)
        {
            this._header = header;
        }

        //private void BoarderHeader()
        //{
        //    if (this._header.ShowBoarder)
        //    {
        //        var spacing = this._header.Spacing != 0 ? this._header.Spacing : 1f;
        //        var boarderWidth = this._header.BoarderWidth != 0 ? this._header.BoarderWidth : 1;
        //        var radius = this._header.BoardRadius != 0 ? this._header.BoardRadius : 0;
        //        var lowerLeftX = this._header.LowerLeftX != 0 ? this._header.LowerLeftX : (_doc.RightMargin - spacing);
        //        var lowerLeftY = this._header.LowerLeftY != 0 ? this._header.LowerLeftY : (_doc.PageSize.Height - _doc.TopMargin - spacing);
        //        var widthRectangle = this._header.WidthRectangle != 0 ? this._header.WidthRectangle : (_doc.Right - _doc.RightMargin - spacing);
        //        var heigthRectangle = this._header.HeigthRectangle != 0 ? this._header.HeigthRectangle : (_doc.TopMargin - spacing);
        //        var boarderColor = this._header.BackColor ?? BaseColor.BLACK;

        //        pdfElemment.Rectangle(lowerLeftX, lowerLeftY, widthRectangle, heigthRectangle, boarderWidth, radius, boarderColor);
        //    }
        //}

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
            var requestId = this._header.RequestId ?? Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6).ToUpper();
            var nameEvent = this._header.NameEvent;

            pdfElemment.TextLeft(string.Format("{0} : {1}", LabelHeader.DATA_DA_SOLICITACAO, requestDate), BaseFont.HELVETICA_BOLD, 8, 14, pdfElemment.InitialPosition);
            pdfElemment.TextLeft(string.Format("{0} : {1}", LabelHeader.ID_DA_SOLICITACAO, requestId), BaseFont.HELVETICA_BOLD, 8, 14, pdfElemment.NextPosition);
            pdfElemment.TextLeft(string.Format("{0} : {1}", LabelHeader.NOME_DO_EVENTO, nameEvent), BaseFont.HELVETICA_BOLD, 8, 14, pdfElemment.NextPosition);
            pdfElemment.HLine();
        }

        private void StructureHeader()
        {
            BasicDocumentData();
            //BoarderHeader();
            LogoImageHeader();
          
        }
        #endregion

        #region Footer
        public void SetFooter(FooterElemment footer)
        {
            this._footer = footer;
        }

        private void StructureFooter()
        {
            //BoarderFooter();
            ImageFooter();
        }

        private void ImageFooter()
        {
            if (this._footer.Logo != null)
            {
                pdfElemment.AddImage(this._footer.Logo, -25, 0);
            }
        }

        //private void BoarderFooter()
        //{
        //    if (this._footer.ShowBoarder)
        //    {
        //        var spacing = this._footer.Spacing != 0 ? this._footer.Spacing : 1f;
        //        var boarderWidth = this._footer.BoarderWidth != 0 ? this._footer.BoarderWidth : 1;
        //        var radius = this._footer.BoardRadius != 0 ? this._footer.BoardRadius : 0;
        //        var lowerLeftX = this._footer.LowerLeftX != 0 ? this._footer.LowerLeftX : (_doc.RightMargin - spacing);
        //        var lowerLeftY = spacing;
        //        var widthRectangle = this._footer.WidthRectangle != 0 ? this._footer.WidthRectangle : (_doc.Right - _doc.RightMargin - spacing);
        //        var heigthRectangle = this._footer.HeigthRectangle != 0 ? this._footer.HeigthRectangle : (_doc.BottomMargin - spacing);
        //        var boarderColor = this._footer.BackColor ?? BaseColor.BLACK;

        //        pdfElemment.Rectangle(lowerLeftX, lowerLeftY, widthRectangle, heigthRectangle, boarderWidth, radius, boarderColor);

        //    }
        //}
        #endregion

    }
}
