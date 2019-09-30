using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfGenerator.Models.Body;
using PdfGenerator.Models.Body.Components;
using PdfGenerator.Models.Label;
using System.Collections.Generic;
using System.IO;

namespace PdfGenerator.Documents
{
    internal class EspecificDocument : GenericDocument
    {
        #region Properties
        private string NameDocument;
        private Client _client;
        private readonly List<BodyElemment> _bodyTitles;
        private List<string> ContentForServices { get; set; }
        private List<string> ContentForDays { get; set; }

        #endregion

        internal EspecificDocument(MemoryStream ms, Document doc) : base(ms, doc)
        {
            _bodyTitles = new List<BodyElemment>();
            ContentForServices = new List<string>();
            ContentForDays = new List<string>();
        }
        /// <summary>
        /// Este método monta a estrutura do PDF na ordem
        /// </summary>
        internal MemoryStream StructureDocument()
        {
            DocumentTitleStructure();
            HeaderAndFooterStructure();
            ClientStructure();

            PrintTitleBody(0);

            PrintTableValueForDay();
            PrintTableValueForService();

            FinishPdf();

            return _ms;
        }
        /// <summary>
        /// ATENÇÃO: Este método deve ser reavaliado para refatoração.
        /// Insere o conteúdo do resumo dos valores por dia. 
        /// Nenhum valor deve vir nulo.
        /// </summary>
        /// <param name="content"></param>
        internal void SetTableValueForDay(List<string> content)
        {
            this.ContentForDays.AddRange(content);
        }

        /// <summary>
        /// ATENÇÃO: Este método deve ser reavaliado para refatoração.
        /// Insere o conteúdo do resumo dos valores por serviço. 
        /// Nenhum valor deve vir nulo.
        /// </summary>
        /// <param name="content"></param>
        internal void SetTableValueForService(List<string> content)
        {
            this.ContentForServices.AddRange(content);
        }

        private void PrintTableValueForDay()
        {
            PageBreak();
            var titles = new List<string> { LabelValues.DIAS, LabelValues.VALOR, LabelValues.PORCENTO, LabelValues.DESCONTOS, LabelValues.VALOR_FINAL };

            pdfElemment.Table(LabelNameTableValue.VALOR_DIA, titles, ContentForDays);

        }

        private void PrintTableValueForService()
        {
            PageBreak();
            var titles = new List<string> { LabelValues.ITEM, LabelValues.VALOR, LabelValues.PORCENTO, LabelValues.DESCONTOS, LabelValues.VALOR_FINAL };

            pdfElemment.Table(LabelNameTableValue.VALOR_SERVICO, titles, ContentForServices);
        }

        /// <summary>
        /// Adiciona Titulos 
        /// </summary>
        /// <param name="body"></param>
        internal void AddTitleBody(BodyElemment body)
        {
            _bodyTitles.Add(body);
        }

        /// <summary>
        /// Adiciona um nome ao Documento, será o título superior. 
        /// Deve ser utilizada a classe LabelDocumentTitle
        /// </summary>
        /// <param name="title"></param>
        internal void SetDocumentTitle(string title)
        {
            this.NameDocument = title;
        }

        /// <summary>
        /// Utilizar este método dentro de StructureDocument() para indicar a escrita do nome do documento.
        /// Deve ser o primeiro dentro de StructureDocument().
        /// </summary>
        private void DocumentTitleStructure()
        {
            PageBreak();
            pdfElemment.TextCenter(NameDocument, BaseFont.HELVETICA_BOLD, 20, _doc.PageSize.Width / 2, 810);
            pdfElemment.AdjustLines(1);
        }

        /// <summary>
        /// Seleciona o cliente para o Documento
        /// </summary>
        /// <param name="client"></param>
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

        /// <summary>
        ///  ATENÇÃO: esta foi a melhor forma encontrada até então, caso seja verificado melhor forma, este deve ser refatorado.
        ///  
        /// Este método serve para possibilitar que váriso titulos sejam inseridos e depois escritos no PDF através de seu index.
        /// </summary>
        /// <param name="index"></param>
        private void PrintTitleBody(int index)
        {
            if (_bodyTitles.Count > 0)
            {
                PrintTitleBody(_bodyTitles[index]);
            }
        }

        /// <summary>
        /// Os títulos devem ser utilizados quando forem frases centralizadas. 
        /// Com fonte de tamanho grande 
        /// </summary>
        /// <param name="body"></param>
        private void PrintTitleBody(BodyElemment body)
        {
            PageBreak();
            PrintBackgrounBody(body);

            pdfElemment.TextCenter(body.TitleBody, BaseFont.HELVETICA_BOLD, 18, _doc.PageSize.Width / 2, pdfElemment.NextPosition - 35);

        }

        /// <summary>
        /// Este método irá configurar a barra que servirá de mooldura para os títulos caso o atributo.
        /// Caso a Propriedade BodyConfi.ShowBoarder==true.
        /// Possui valores padrão de cor de fundo ceinza, borda branca e o tamanho ocupa toda a largura da página,
        /// caso estes valores venham nulos.
        /// </summary>
        /// <param name="config"> </param>
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

        /// <summary>
        /// ATENÇÃO: Este método ainda precisa ser validado com todos os elementos dos documentos.
        ///
        /// Utilizar este métodono início de todos os métodos que sejam responsáveis por escrever algo no PDF
        /// </summary>
        private void PageBreak()
        {
            if (pdfElemment.Lines < -5)
            {
                NewPage();
                pdfElemment.ResetLines();
            }
        }

    }
}
