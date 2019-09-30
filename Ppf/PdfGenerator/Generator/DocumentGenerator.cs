using iTextSharp.text;
using Org.BouncyCastle.Crypto.Digests;
using PdfGenerator.Documents;
using PdfGenerator.Models.Body.Components;
using PdfGenerator.Models.Body.Componets;
using PdfGenerator.Models.Body.Evento;
using PdfGenerator.Models.Body.Events.Food;
using PdfGenerator.Models.Body.Services;
using PdfGenerator.Models.HeaderAndFooter;
using PdfGenerator.Models.Label;
using System;
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
                var header = HeaderFooterElemment.Create(imageHeader);
                header.Configure("20/11/19", "555555", "Teste de Geração do PDF");

                especificDocument.SetHeader(header);

                especificDocument.SetDocumentTitle(LabelDocumentTitle.DETALHAMENTO_PROPOSTA);

                var client = Client.Create("Teste1", "0000000000000", "Teste2", "0000000000000");
                especificDocument.SetClient(client);

                var titleBody = Title.CreateTitleBody(LabelBodyTitle.RESUMO_GERAL);
                especificDocument.AddTitleBody(titleBody);

                var contentDay = new List<string>
                {
                    "DIAS1", "VALOR1", "%1", "DESCONTOS1", "VALOR FINAL1",
                    "DIAS2", "VALOR2", "%2", "DESCONTOS2", "VALOR FINAL2",
                    "DIAS3", "VALOR3", "%3", "DESCONTOS3", "VALOR FINAL3",
                    "DIAS4", "VALOR4", "%4", "DESCONTOS4", "VALOR FINAL4",
                    "DIAS5", "VALOR45", "%5", "DESCONTOS5", "VALOR FINAL5"
                };


                especificDocument.SetTableValueForDay(contentDay);


                var contentService = new List<string>
                {
                    "SERVIÇO1", "VALOR1", "%1", "DESCONTOS1", "VALOR FINAL1",
                    "SERVIÇO2", "VALOR2", "%2", "DESCONTOS2", "VALOR FINAL2",
                    "SERVIÇO3", "VALOR3", "%3", "DESCONTOS3", "VALOR FINAL3",
                    "SERVIÇO4", "VALOR4", "%4", "DESCONTOS4", "VALOR FINAL4",
                    "SERVIÇO5", "VALOR5", "%5", "DESCONTOS5", "VALOR FINAL5",
                    "SERVIÇO6", "VALOR6", "%6", "DESCONTOS6", "VALOR FINAL6",
                    "SERVIÇO7", "VALOR7", "%7", "DESCONTOS7", "VALOR FINAL7"
                };

                especificDocument.SetTableValueForService(contentService);


                var description = string.Concat("1 CAMA DE CASAL KING SIZE OU 2 CAMAS DE SOLTEIRO (SOLTEIRÃO), TELEFONE NA CABECEIRA DA CAMA,",
                                " RÁDIO RELÓGIO, SMART TV 49’, MESA DE TRABALHO COM TELEFONE, INTERNET WI-FI, MESA DE REFEIÇÃO,",
                             " AR CONDICIONADO, FECHADURA ELETRÔNICA, COFRE ELETRÔNICO, FERRO E TÁBUA DE PASSAR, 1",
                             " AR CONDICIONADO, FECHADURA ELETRÔNICA, COFRE ELETRÔNICO, FERRO E TÁBUA DE PASSAR, 1",
                             " GARRAFA DE ÁGUA MINERAL CORTESIA E MINIBAR.BANHEIRO EQUIPADO COM: TELEFONE, SECADOR DE",
                             " CABELOS, ESPELHO RETRÁTIL DE AUMENTO, VARAL RETRÁTIL E AMENITIES.");

                var service = Services.Create("SUPERIOR", description, "R$46.299,00", "R$701,50 POR QUARTO");

                for (int i = 0; i < 18; i++)
                {
                    service.AddConvenience("CAFÉ DA MANHÃ INCLUSO" + i);
                }


                var acommodation = Accommodation.Create("2 DIÁRIAS", "33 QUARTOS", new List<string> { "1 CAMA CASAL","1 CAMA CASAL" });
                acommodation.SetService(service);

                service.AddImage(Image.GetInstance(new Uri(@"https://abrilviagemeturismo.files.wordpress.com/2016/10/thinkstockphotos-474448950.jpeg?quality=70&strip=info&w=928")));
                //service.AddImage(Image.GetInstance(new Uri(@"https://todosdestinos.com/wp-content/uploads/2018/10/recife.jpg")));
                //service.AddImage(Image.GetInstance(new Uri(@"https://cdn.bloghoteis.com/wp-content/uploads/2017/09/Recife_capa-1200x400.jpg")));
                //service.AddImage(Image.GetInstance(new Uri(@"https://visit.recife.br/wp-content/uploads/2018/01/roteiro-carnaval-topo-mobile.jpg")));
                //service.AddImage(Image.GetInstance(new Uri(@"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcReFNPweLmMk4XMScBUxNlQAngoZCPVwVIuLoq7twN-6o--4jUF")));
                //service.AddImage(Image.GetInstance(new Uri(@"https://www.downtownnogalo.com.br/wp-content/uploads/2018/11/galo.jpg")));

                var lodging = Lodging.Create();
                lodging.AddAcommodation(acommodation);

                especificDocument.AddService(lodging);
                especificDocument.AddService(lodging);


                var schedule = Agenda.Create("08:00", "23:00");
                var schedule2 = Agenda.Create("07:00", "22:00");
                var schedule3 = Agenda.Create("06:00", "20:00");


                var breakFast = Meal.CreateSmallMeal("CAFÉ DA MANHÃ", "OPÇÃO II", 50, "62,00", "3.100,00", @"ÁGUA MINERAL SEM GÁS; CAFÉ; LEITE;CHÁS VARIADOS CHOCOLATE QUENTE;2 TIPOS DE SUCOS");
                var list1 = new List<Food>
                {
                    Food.CreateSmallFood("SALADA DE FRUTAS;"),
                    Food.CreateSmallFood("3 TIPOS DE CEREAIS;"),
                    Food.CreateSmallFood("SALADA DE FRUTAS;"),
                    Food.CreateSmallFood("3 TIPOS DE CEREAIS;")
                };

                breakFast.AddFoods(list1);
                breakFast.SetSchedule(schedule);

                var coffeBreack1 = Meal.CreateSmallMeal("COFFEE BREAK DA MANHÃ", "WELCOME COFFEE - OPÇÃO I", 50, "62,00", "3.100,00", @"ÁGUA MINERAL SEM GÁS; CAFÉ; LEITE;CHÁS VARIADOS CHOCOLATE QUENTE;2 TIPOS DE SUCOS");
                            coffeBreack1.SetSchedule(schedule);
                var list2 = new List<Food>
                {
                    Food.CreateSmallFood("SALADA DE FRUTAS;"),
                    Food.CreateSmallFood("3 TIPOS DE CEREAIS;"),
                    Food.CreateSmallFood("SALADA DE FRUTAS;"),
                    Food.CreateSmallFood("3 TIPOS DE CEREAIS;")
                };

                coffeBreack1.AddFoods(list2);

                var almoco = Meal.CreatePrincialMeal("ALMOÇO", "TESTE SALADAS",50, "62,00", "3.100,00");
                var list3 = new List<Food>
                {
                    Food.CreatePrincipalFood("aaaa1",@"QWERTSALADA SERRANA (MIX DE FOLHAS, FRANGO DESFIADO, QUEIJO, MOLHO DE IOGURTE, LASCAS DE QUEIJO SERRANO E CRÓTONS), SALADA REPUBLICANA (ALFACE AMERICANA, TOMATE, BETERRABA RALADA, MOLHO MOSTARDA, MEL E GERGELIM NEGRO), SALADA PORTUGUESA (BATATA, SALSA, OVOS, AZEITONA PRETA, PIMENTA REINO E PIMENTÕES ASSADOS), SALADA COLESLAU (REPOLHO VERDE, REPOLHO ROXO, CENOURA, SALSA, MAIONESE E SUCO DE LIMÃO), SALADA DE BETERRABA ASSADA (BETERRABA ASSADA E LAMINADA, RICOTA, IOGURTE, ERVAS FRESCAS, SUCO DE LIMÃO E AZEITE DE OLIVA), SALADA SICILIANA (BERINJELA, PIMENTÕES COLORIDOS, CEBOLA, ABOBRINHA, AZEITONA VERDE, UVA PASSA PRETA, OLIVA E MANJERICÃO), SALADA DE GRÃO DE BICO (GRÃO DE BICO E VINAGRETE), SALADA FLORES DA SERRA (ABACATE, TOMATE CEREJA, ERVAS FRESCA, SUCO DE LIMÃO E AZEITE DE OLIVA), ROSBIFE AO M"),
                    Food.CreatePrincipalFood("aaaa2",@"QWERTSALADA SERRANA (MIX DE FOLHAS, FRANGO DESFIADO, QUEIJO, MOLHO DE IOGURTE, LASCAS DE QUEIJO SERRANO E CRÓTONS), SALADA REPUBLICANA (ALFACE AMERICANA, TOMATE, BETERRABA RALADA, MOLHO MOSTARDA, MEL E GERGELIM NEGRO), SALADA PORTUGUESA (BATATA, SALSA, OVOS, AZEITONA PRETA, PIMENTA REINO E PIMENTÕES ASSADOS), SALADA COLESLAU (REPOLHO VERDE, REPOLHO ROXO, CENOURA, SALSA, MAIONESE E SUCO DE LIMÃO), SALADA DE BETERRABA ASSADA (BETERRABA ASSADA E LAMINADA, RICOTA, IOGURTE, ERVAS FRESCAS, SUCO DE LIMÃO E AZEITE DE OLIVA), SALADA SICILIANA (BERINJELA, PIMENTÕES COLORIDOS, CEBOLA, ABOBRINHA, AZEITONA VERDE, UVA PASSA PRETA, OLIVA E MANJERICÃO), SALADA DE GRÃO DE BICO (GRÃO DE BICO E VINAGRETE), SALADA FLORES DA SERRA (ABACATE, TOMATE CEREJA, ERVAS FRESCA, SUCO DE LIMÃO E AZEITE DE OLIVA), ROSBIFE AO M"),
                   
                };

                almoco.AddFoods(list3);
                almoco.SetSchedule(schedule);


                var custom1 = Meal.CreatePrincialMeal("ALMOÇO", "TESTE SALADAS", 50, "62,00", "3.100,00");
                var list4 = new List<Food>
                {
                    Food.CreateCustomFood("aaaa1",@"QWERTSALADA SERRANA (MIX DE FOLHAS, FRANGO DESFIADO, QUEIJO, MOLHO DE IOGURTE QWERTSALADA SERRANA (MIX DE FOLHAS, FRANGO DESFIADO, QUEIJO, MOLHO DE IOGURTE,","5","5","25"),
                    Food.CreateCustomFood("aaaa1",@"QWERTSALADA SERRANA (MIX DE FOLHAS, FRANGO DESFIADO, QUEIJO, MOLHO DE IOGURTE QWERTSALADA SERRANA (MIX DE FOLHAS, FRANGO DESFIADO, QUEIJO, MOLHO DE IOGURTE,","5","5","25"),
                     Food.CreateCustomFood("aaaa1",@"QWERTSALADA SERRANA (MIX DE FOLHAS, FRANGO DESFIADO, QUEIJO, MOLHO DE IOGURTE QWERTSALADA SERRANA (MIX DE FOLHAS, FRANGO DESFIADO, QUEIJO, MOLHO DE IOGURTE,","5","5","25"),
                    Food.CreateCustomFood("aaaa1",@"QWERTSALADA SERRANA (MIX DE FOLHAS, FRANGO DESFIADO, QUEIJO, MOLHO DE IOGURTE QWERTSALADA SERRANA (MIX DE FOLHAS, FRANGO DESFIADO, QUEIJO, MOLHO DE IOGURTE,","5","5","25"),

                };

                custom1.AddFoods(list4);


                 var custom2 = Meal.CreatePrincialMeal("ALMOÇO", "TESTE SALADAS", 50, "62,00", "3.100,00");
                var list5 = new List<Food>
                {
                    Food.CreateCustomFood("aaaa1",@"QWERTSALADA SERRANA (MIX DE FOLHAS, FRANGO DESFIADO, QUEIJO, MOLHO DE IOGURTE QWERTSALADA SERRANA (MIX DE FOLHAS, FRANGO DESFIADO, QUEIJO, MOLHO DE IOGURTE,","5","5","25"),
                    Food.CreateCustomFood("aaaa1",@"QWERTSALADA SERRANA (MIX DE FOLHAS, FRANGO DESFIADO, QUEIJO, MOLHO DE IOGURTE QWERTSALADA SERRANA (MIX DE FOLHAS, FRANGO DESFIADO, QUEIJO, MOLHO DE IOGURTE,","5","5","25"),
                   
                };

                custom2.AddFoods(list5);



                var eventDetail = EventDetail.Create("138.039,90", breakFast, coffeBreack1, almoco, coffeBreack1, almoco, coffeBreack1);
                eventDetail.AddCustomMeal(custom1, custom2);

                var dayEvent = DaysEvent.Create(1, "26/08/2019", 50, schedule);

                var room = Room.Create(5, "BOSSA NOVA", "ESCOLAR", "101.00", "2,61", 50, "833,75");
                //room.AddImage(Image.GetInstance(new Uri(@"https://abrilviagemeturismo.files.wordpress.com/2016/10/thinkstockphotos-474448950.jpeg?quality=70&strip=info&w=928")));
                room.AddImage(Image.GetInstance(new Uri(@"https://todosdestinos.com/wp-content/uploads/2018/10/recife.jpg")));

                room.AddSchedule(schedule);
                room.AddSchedule(schedule2);
                room.AddSchedule(schedule3);
                dayEvent.AddRoom(room);

                eventDetail.AddDayEvent(dayEvent);

                var agua = GenericItem.Create("ÁGUA GALÃO 20L", "64,00");
                var cafe = GenericItem.Create("GARRAFA DE CAFÉ 1,8L", "64,00");
                var suco = GenericItem.Create("JARRA DE SUCO 2L", "64,00");
                eventDetail.AddWhaterAndCoffe(agua);
                eventDetail.AddWhaterAndCoffe(cafe);
                eventDetail.AddWhaterAndCoffe(suco);      
                eventDetail.AddWhaterAndCoffe(agua);
                eventDetail.AddWhaterAndCoffe(cafe);
                eventDetail.AddWhaterAndCoffe(suco);

                var proj = GenericItem.Create("PROJETOR ATÉ 100 PESSOAS", "10,00","1","10,00");
                eventDetail.AddEquipments(proj);

                especificDocument.SetDetailEvent(eventDetail);

                var pathImageFooter = @"..\..\..\test\image_footer.jpg";
                var imageFooter = Image.GetInstance(File.Open(pathImageFooter, FileMode.Open));
                var footer = HeaderFooterElemment.Create(imageFooter);
                especificDocument.SetFooter(footer);

                return especificDocument.StructureDocument();
            }

        }
    }
}
