using iTextSharp.text;
using PdfGenerator.Documents;
using PdfGenerator.Models.Body.Components;
using PdfGenerator.Models.Body.Componets;
using PdfGenerator.Models.HeaderAndFooter;
using PdfGenerator.Models.Label;
using System.Collections.Generic;
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
            var ms = new MemoryStream();

            using (var doc = new Document())
            {
                var especificDocument = new EspecificDocument(ms, doc);

                var pathImageHeader = @"..\..\..\test\image_header.jpg";
                var imageHeader = Image.GetInstance(File.Open(pathImageHeader, FileMode.Open));
                var header = new HeaderElemment(imageHeader);
                especificDocument.SetHeader(header);

                especificDocument.SetDocumentTitle(LabelDocumentTitle.DETALHAMENTO_SERVICO);

                var client =  Client.Create("Teste1","0000000000000","Teste2","0000000000000");
                especificDocument.SetClient(client);

                var titleBody = PrincipalTitle.Create(LabelBodyTitle.RESUMO_GERAL);
                especificDocument.AddToBody(titleBody);

                var titles = new List<string> { LabelValues.DIAS, LabelValues.VALOR, LabelValues.PORCENTO, LabelValues.DESCONTOS, LabelValues.VALOR_FINAL };
                var content = new List<string> { "DIAS1", "VALOR1", "%1", "DESCONTOS1", "VALOR FINAL1",
                                               "DIAS2", "VALOR2", "%2", "DESCONTOS2", "VALOR FINAL2",
                                               "DIAS3", "VALOR3", "%3", "DESCONTOS3", "VALOR FINAL3",
                                               "DIAS4", "VALOR4", "%4", "DESCONTOS4", "VALOR FINAL4",
                                               "DIAS5", "VALOR45", "%5", "DESCONTOS5", "VALOR FINAL5"};

                var table = TableValues.Create(LabelValueTitle.VALOR_EVENTO,titles,content);
                especificDocument.AddToBody(table);



                var pathImageFooter = @"..\..\..\test\image_footer.jpg";
                var imageFooter = Image.GetInstance(File.Open(pathImageFooter, FileMode.Open));
                var footer = new FooterElemment(imageFooter);
                especificDocument.SetFooter(footer);

                return especificDocument.GetPdf();
            }

        }
    }
}
