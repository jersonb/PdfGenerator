using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfGenerator.Models.Body;
using System.IO;

namespace PdfGenerator.Documents
{
    public class EspecificDocument : GenericDocument
    {
       
        private string _nameDocument;
     

        private EspecificDocument(MemoryStream ms, Document doc) : base(ms, doc)
        {
        }

        public static EspecificDocument Generate(MemoryStream ms, Document doc)
            => new EspecificDocument(ms, doc);

        /// <summary>
        /// Este método monta a estrutura do PDF na ordem
        /// </summary>
        public MemoryStream StructureDocument()
        {
            DocumentTitleStructure();
            HeaderAndFooterStructure();

            FinishPdf();

            return _ms;
        }


        /// <summary>
        /// Adiciona um nome ao Documento, será o título superior. 
        /// Deve ser utilizada a classe LabelDocumentTitle
        /// </summary>
        /// <param name="title"></param>
        public void SetDocumentTitle(string title)
        {
            this._nameDocument = title;
        }

        /// <summary>
        /// Utilizar este método dentro de StructureDocument() para indicar a escrita do nome do documento.
        /// Deve ser o primeiro dentro de StructureDocument().
        /// </summary>
        private void DocumentTitleStructure()
        {
            pdfElemment.TextCenter(_nameDocument, BaseFont.HELVETICA_BOLD, 20, pdfElemment.CenterX(), 810);
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
                var lowerLeftX = config.LowerLeftX != 0 ? config.LowerLeftX : _doc.RightMargin - spacing;
                var lowerLeftY = config.LowerLeftY != 0 ? (config.LowerLeftY + pdfElemment.NextPosition) : pdfElemment.NextPosition;
                var widthRectangle = config.WidthRectangle != 0 ? config.WidthRectangle : (_doc.Right - _doc.RightMargin - spacing);
                var heigthRectangle = config.HeigthRectangle != 0 ? config.HeigthRectangle : 50 + spacing;
                var boarderColor = config.BoarderColor ?? BaseColor.WHITE;
                var backgroundColor = config.BackColor ?? BaseColor.GRAY;
                var opacity = config.BackOpacity;

                pdfElemment.Rectangle(lowerLeftX, lowerLeftY, widthRectangle, heigthRectangle, boarderWidth, radius, boarderColor, backgroundColor, opacity);

                if (config.ShowLine)
                    pdfElemment.HDivision();
            }
        }

        private void PageBreak(float minHigth)
        {
            pdfElemment.CheckPosition(minHigth);
            PageBreak();
        }

        /// <summary>
        /// ATENÇÃO: Este método ainda precisa ser validado com todos os elementos dos documentos.
        ///
        /// Utilizar este métodono início de todos os métodos que sejam responsáveis por escrever algo no PDF
        /// </summary>
        private void PageBreak()
        {
            if (pdfElemment.PageBreak)
            {
                NewPage();
            }
        }

    }
}
