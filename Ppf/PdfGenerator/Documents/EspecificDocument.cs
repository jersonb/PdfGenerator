using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfGenerator.Models.Body;
using PdfGenerator.Models.Body.Components;
using PdfGenerator.Models.Body.Componets;
using PdfGenerator.Models.Label;
using System;
using System.Collections.Generic;
using System.IO;

namespace PdfGenerator.Documents
{
    public class EspecificDocument : GenericDocument
    {
        #region Properties
        private string NameDocument;
        private Client _client;
        private readonly List<Title> _bodyTitles;
        private List<string> ContentForServices { get; set; }
        private List<string> ContentForDays { get; set; }

        #endregion

        public EspecificDocument(MemoryStream ms, Document doc) : base(ms, doc)
        {
            _bodyTitles = new List<Title>();
            ContentForServices = new List<string>();
            ContentForDays = new List<string>();
        }
        /// <summary>
        /// Este método monta a estrutura do PDF na ordem
        /// </summary>
        public MemoryStream StructureDocument()
        {
            DocumentTitleStructure();
            HeaderAndFooterStructure();
            ClientStructure();

            PrintTitleBody(0);

            PrintTableValueForDay();
            PrintTableValueForService();

            PrintService();

            FinishPdf();

            return _ms;
        }

        private void PrintService()
        {
            PageBreak();
            var acommodation = Services.CreateAccommodation();
            acommodation.Name = "SUPERIOR";
            acommodation.Description = string.Concat("1 CAMA DE CASAL KING SIZE OU 2 CAMAS DE SOLTEIRO (SOLTEIRÃO), TELEFONE NA CABECEIRA DA CAMA,",
                                          " RÁDIO RELÓGIO, SMART TV 49’, MESA DE TRABALHO COM TELEFONE, INTERNET WI-FI, MESA DE REFEIÇÃO,",
                                       " AR CONDICIONADO, FECHADURA ELETRÔNICA, COFRE ELETRÔNICO, FERRO E TÁBUA DE PASSAR, 1",
                                       " GARRAFA DE ÁGUA MINERAL CORTESIA E MINIBAR.BANHEIRO EQUIPADO COM: TELEFONE, SECADOR DE",
                                       " CABELOS, ESPELHO RETRÁTIL DE AUMENTO, VARAL RETRÁTIL E AMENITIES.");
            pdfElemment.NextLine();
            pdfElemment.TextLeft(acommodation.TitleBody, BaseFont.HELVETICA, 10, 12, pdfElemment.NextPosition);
            pdfElemment.TextLeft(acommodation.Name, BaseFont.HELVETICA, 8, 12, pdfElemment.NextPosition);

            pdfElemment.TextLeftColumn(acommodation.Description);
            var itens = new string[] { "R$46.299,00", "R$701,50 POR QUARTO", "2 DIÁRIAS", "33 QUARTOS" };
            pdfElemment.TextRightBorder(itens);

            pdfElemment.TextLeft(acommodation.Name, BaseFont.HELVETICA, 8, 12, pdfElemment.NextPosition);
        }

        /// <summary>
        /// ATENÇÃO: Este método deve ser reavaliado para refatoração.
        /// Insere o conteúdo do resumo dos valores por dia. 
        /// Nenhum valor deve vir nulo.
        /// </summary>
        /// <param name="content"></param>
        public void SetTableValueForDay(List<string> content)
        {
            this.ContentForDays.AddRange(content);
        }

        /// <summary>
        /// ATENÇÃO: Este método deve ser reavaliado para refatoração.
        /// Insere o conteúdo do resumo dos valores por serviço. 
        /// Nenhum valor deve vir nulo.
        /// </summary>
        /// <param name="content"></param>
        public void SetTableValueForService(List<string> content)
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
        public void AddTitleBody(Title body)
        {
            _bodyTitles.Add(body);
        }

        /// <summary>
        /// Adiciona um nome ao Documento, será o título superior. 
        /// Deve ser utilizada a classe LabelDocumentTitle
        /// </summary>
        /// <param name="title"></param>
        public void SetDocumentTitle(string title)
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
            pdfElemment.TextCenter(NameDocument, BaseFont.HELVETICA_BOLD, 20, pdfElemment.CenterX(), 810);
            pdfElemment.AdjustLines(2);
        }

        /// <summary>
        /// Seleciona o cliente para o Documento
        /// </summary>
        /// <param name="client"></param>
        public void SetClient(Client client)
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
        /// <param name="titleBody"></param>
        private void PrintTitleBody(Title titleBody)
        {
            PageBreak();
            PrintBackgrounBody(titleBody);
            pdfElemment.TextCenter(titleBody.TitleBody, BaseFont.HELVETICA_BOLD, 18, pdfElemment.CenterX(), pdfElemment.NextPosition - 35);

        }

        /// <summary>
        /// Este método irá configurar a barra que servirá de mooldura para os títulos caso o atributo.
        /// Caso a Propriedade BodyConfi.ShowBoarder==true.
        /// Possui valores padrão de cor de fundo ceinza, borda branca e o tamanho ocupa toda a largura da página,
        /// caso estes valores venham nulos.
        /// </summary>
        /// <param name="config"> </param>
        private void PrintBackgrounBody(BodyElemment config)
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
