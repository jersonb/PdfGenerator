using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfGenerator.Models.Body;
using PdfGenerator.Models.Body.Components;
using PdfGenerator.Models.Body.Componets;
using PdfGenerator.Models.Label;
using System.Collections.Generic;
using System.IO;

namespace PdfGenerator.Documents
{
    internal class EspecificDocument : GenericDocument
    {
        #region Properties
        private Client _client;
        private readonly List<BodyElemment> _body;
        #endregion

        internal EspecificDocument(MemoryStream ms, Document doc) : base(ms, doc)
        {
            _body = new List<BodyElemment>();
        }

        internal void SetDocumentTitle(string title)
        {
            pdfElemment.TextCenter(title, BaseFont.HELVETICA_BOLD, 20, _doc.PageSize.Width / 2, 810);
            pdfElemment.AdjustLines(1);
        }

        internal MemoryStream GetPdf()
        {
            HeaderAndFooterStructure();
            ClientStructure();
            PrintBody();


            //  pdfElemment.ShowLines();

            //      pdfElemment.ShowGrid();

            FinishPdf();

            return _ms;
        }

        private void PrintBody()
        {
            if (_body.Count > 0)
            {
                _body.ForEach(x => PrintBody(x));

            }
        }

        private void PrintBody(BodyElemment body)
        {
            PrintBackgrounBody(body);
         
            pdfElemment.TextCenter(body.TitleBody, BaseFont.HELVETICA_BOLD, 18, _doc.PageSize.Width / 2, pdfElemment.NextPosition - 35);

           
        }

        private void PrintBackgrounBody(BodyConfig config)
        {
            if (config.ShowBoarder)
            {
                var spacing = config.Spacing != 0 ? config.Spacing : 1f;
                var boarderWidth = config.BoarderWidth != 0 ? config.BoarderWidth : 1;
                var radius = config.BoardRadius != 0 ? config.BoardRadius : 0;
                var lowerLeftX = config.LowerLeftX != 0 ? config.LowerLeftX : (_doc.RightMargin - spacing);
                var lowerLeftY = config.LowerLeftY != 0 ? config.LowerLeftY : (pdfElemment.NextPosition - 50);
                var widthRectangle = config.WidthRectangle != 0 ? config.WidthRectangle : (_doc.Right - _doc.RightMargin - spacing);
                var heigthRectangle = config.HeigthRectangle != 0 ? config.HeigthRectangle : 50 + spacing;
                var boarderColor = config.BoarderColor ?? BaseColor.WHITE;
                var backgroundColor = config.BackColor ?? BaseColor.GRAY;

                pdfElemment.Rectangle(lowerLeftX, lowerLeftY, widthRectangle, heigthRectangle, boarderWidth, radius, boarderColor, backgroundColor);
                pdfElemment.HDivision(lowerLeftY);
            }
        }

        internal void AddToBody(BodyElemment body)
        {
            _body.Add(body);
        }

        internal void SetClient(Client client)
        {
            this._client = client;
        }

        private void ClientStructure()
        {
            var requester = _client.Requester ?? "Cliente Solicitante";
            var cnpjRequester = _client.CnpjRequester ?? "88.888.888/8888-88";
            var affiliate = _client.Affiliate ?? "Cliente Afiliado";
            var cnpjAffiliate = _client.CnpjAffiliate ?? "88.888.888/8888-88";

            pdfElemment.TextLeft(string.Format("{0} : {1}", LabelClient.CLIENTE_SOLICITANTE, requester), BaseFont.HELVETICA_BOLD, 8, 14, pdfElemment.NextPosition - 15);
            pdfElemment.TextLeft(string.Format("{0} : {1}", LabelClient.CNPJ, cnpjRequester), BaseFont.HELVETICA_BOLD, 8, 14, pdfElemment.NextPosition);
            pdfElemment.TextLeft(string.Format("{0} : {1}", LabelClient.AFILIADO_CONTRATADO, affiliate), BaseFont.HELVETICA_BOLD, 8, 14, pdfElemment.NextPosition);
            pdfElemment.TextLeft(string.Format("{0} : {1}", LabelClient.CNPJ, cnpjAffiliate), BaseFont.HELVETICA_BOLD, 8, 14, pdfElemment.NextPosition);
            pdfElemment.HLine();
        }

    }
}
