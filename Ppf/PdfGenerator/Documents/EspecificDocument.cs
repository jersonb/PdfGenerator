using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfGenerator.Models.Body;
using PdfGenerator.Models.Body.Components;
using PdfGenerator.Models.Body.Componets;
using PdfGenerator.Models.Body.Evento;
using PdfGenerator.Models.Body.Events.Food;
using PdfGenerator.Models.Body.Services;
using PdfGenerator.Models.Label;
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
        private readonly List<Lodging> _lodging;
        private EventDetail _eventDetails;

        private List<string> ContentForServices { get; set; }
        private List<string> ContentForDays { get; set; }

        #endregion

        public EspecificDocument(MemoryStream ms, Document doc) : base(ms, doc)
        {
            _bodyTitles = new List<Title>();
            _lodging = new List<Lodging>();

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

            PrintEventDetail();

            FinishPdf();

            return _ms;
        }

        private void PrintEventDetail()
        {
            PageBreak(200);

            pdfElemment.HDivision();

            var tValue = Title.CreateTitleValueEvent(string.Concat("VALOR DO EVENTO R$ ", _eventDetails.Value));

            PrintTitleBody(tValue);

            var tEvent = Title.CreateTitleBodyDetail("DETALHES DO EVENTO");

            PrintTitleBody(tEvent);

            PrintDays();


            PrintFoods();
            PrintExtra();
            PrintTableValueForService2();
            var tValueFinal = Title.CreateTitleValueEvent(string.Concat("VALOR FINAL: R$ ", _eventDetails.Value));

            PrintTitleBody(tValueFinal);
            pdfElemment.HDivision();

            PageBreak();
        }

        private void PrintDays()
        {
            if (!_eventDetails.Days.Count.Equals(0))
            {
                PageBreak();
                PrintDays(_eventDetails.Days);
                PageBreak();
            }
        }

        private void PrintExtra()
        {
            if (!_eventDetails.WhaterAndCoffe.Count.Equals(0))
            {
                PrintExtraWhaterAndCoffe(_eventDetails.WhaterAndCoffe);
            }

            if (!_eventDetails.Equipments.Count.Equals(0))
            {
                PrintExtraEquipments(_eventDetails.Equipments);
            }
        }


        private void PrintExtraWhaterAndCoffe(List<GenericItem> whaterAndCoffe)
        {
            PageBreak(100);
            pdfElemment.HDivision();
            pdfElemment.TextLeft(".ÁGUA E CAFÉ NA SALA",12);
            whaterAndCoffe.ForEach(x =>
            {
                PageBreak(20);
                pdfElemment.SetAtualPosition(pdfElemment.NextPosition);
                pdfElemment.TextPositionX(x.Name,8,15);
                pdfElemment.TextPositionX(string.Format("R${0}", x.Value),8,300);
                PageBreak();
            });

            pdfElemment.TextLeft("*SERÃO SERVIDOS DURANTE TODO O PERÍODO DO EVENTO COM COBRANÇA POSTERIOR.",7,BaseFont.HELVETICA_OBLIQUE);
            PageBreak();
        }

        private void PrintExtraEquipments(List<GenericItem> equipments)
        {
            PageBreak(100);
            pdfElemment.HDivision();
            pdfElemment.TextLeft(".EQUIPAMENTOS", 12);
            equipments.ForEach(x =>
            {
                pdfElemment.SetAtualPosition(pdfElemment.NextPosition);
                pdfElemment.TextPositionX(x.Name, 8, 15);
                pdfElemment.TextPositionX(string.Format("QUANT.: {0}", x.Quant)  , 8, 300);
                pdfElemment.TextPositionX(string.Format("UNIT.: R${0}", x.Value)  , 8, 400);
                pdfElemment.TextPositionX(string.Format("R${0}", x.Total), 8, 500);
            });
            PageBreak();
        }



        private void PrintDays(DaysEvent day)
        {
            PageBreak(200);
            var tDays = Title.CreateTitleEventDay(string.Format("{0}º DIA - {1}", day.Order, day.Date));
            PageBreak(tDays.MinSizeHeight);

            AddTitleBody(tDays);
            PrintTitleBody(tDays);

            PageBreak(30);
            pdfElemment.TextLeft(string.Format("PERÍODO DO EVENTO: {0} DÁS {1} ÀS {2}", day.Date, day.Schedule.Beginning, day.Schedule.End), 8);
            pdfElemment.TextLeft(string.Concat("QUANTIDADE DE PESSOAS: ", day.QuantPeople), 8);

            PageBreak(200);
            PrintRooms(day.Rooms);

            PageBreak();
        }

        private void PrintFoods()
        {
            PageBreak();
            pdfElemment.HDivision();
            pdfElemment.SetAtualPosition(pdfElemment.NextPosition);
            pdfElemment.TextLeft(".ALIMENTOS", 12);

            PrintSmallFood(_eventDetails.BreakFast);
            PrintSmallFood(_eventDetails.CoffeBreak1);
            PrintPrincipalFood(_eventDetails.Lunch);
            PrintSmallFood(_eventDetails.CoffeBreak2);
            PrintPrincipalFood(_eventDetails.Dinner);

            PrintCustomFood(_eventDetails.CustomFoods);
            PrintCustomDrink(_eventDetails.CustomDrinks);

            PageBreak();
        }

        private void PrintCustomFood(Meal customFoods)
        {
            if (customFoods != null)
            {
                PageBreak();
                pdfElemment.HDivision();
                pdfElemment.TextLeft(".ALIMENTOS CUSTOMIZADOS", 10);
                HeaderFood(customFoods);
                PageBreak();
                PrintCustomMeal(customFoods.Foods);
            }
        }

        private void PrintCustomDrink(Meal customDrinks)
        {
            if (customDrinks != null)
            {
                PageBreak();
                pdfElemment.HDivision();
                pdfElemment.TextLeft(".BEBIDAS CUSTOMIZADAS", 10);
                HeaderFood(customDrinks);
                PageBreak();
                PrintCustomMeal(customDrinks.Foods);
            }
        }

        private void PrintCustomMeal(List<Food> foods)
        {
            foods.ForEach(x =>
            {
                PageBreak(50);
                pdfElemment.SetAtualPosition(pdfElemment.NextPosition);
                pdfElemment.TextPositionX(x.Name, 8, 15f);
                pdfElemment.TextPositionX(string.Format("QUANT.: {0}", x.Quant), 8, 300f);
                pdfElemment.TextPositionX(string.Format("UNIT.: R${0}", x.UnitValue), 8, 400f);
                pdfElemment.TextPositionX(string.Format("R${0}", x.Total), 8, 500f);
                pdfElemment.SetAtualPosition(pdfElemment.NextPosition);
                pdfElemment.BigTextLeftColumn(x.Description, 0.5f);
                pdfElemment.SetAtualPosition(pdfElemment.NextPosition);
                PageBreak();
            });
        }

        private void PrintPrincipalFood(Meal principalFood)
        {
            if (principalFood != null)
            {
                HeaderFood(principalFood);
                PageBreak();
                pdfElemment.SetAtualPosition(pdfElemment.NextPosition);
                pdfElemment.TextPositionX(string.Format("INÍCIO: {0} HS", principalFood.Schedule.Beginning), 8, 400);
                pdfElemment.TextPositionX(string.Format("TÉRMINO: {0} HS", principalFood.Schedule.End), 8, 500);

                if (!principalFood.Foods.Count.Equals(0))
                    principalFood.Foods.ForEach(x => PrintPrincipalFood(x.Name, x.Description));


                if (!string.IsNullOrEmpty(principalFood.Drinks))
                {
                    pdfElemment.TextLeft("BEBIDAS", 8);

                    pdfElemment.BigTextLeftColumn(principalFood.Drinks, 0.4f);
                }


                PageBreak();
            }
        }

        private void PrintPrincipalFood(string name, string plate)
        {
            PageBreak(100);
            if (!string.IsNullOrEmpty(plate))
            {
                pdfElemment.TextLeft(name, 8);
                pdfElemment.BigTextLeftColumn(plate, 0.6f);
                pdfElemment.SetAtualPosition(pdfElemment.NextPosition);
            }
            PageBreak();
        }

        private void PrintSmallFood(Meal smallFood)
        {
            if (smallFood != null)
            {
                HeaderFood(smallFood);
                PageBreak();
                pdfElemment.SetAtualPosition(pdfElemment.NextPosition);
                pdfElemment.TextPositionX("DESCRIÇÃO", 8, 12);
                pdfElemment.TextPositionX(string.Format("INÍCIO: {0} HS", smallFood.Schedule.Beginning), 8, 400);
                pdfElemment.TextPositionX(string.Format("TÉRMINO: {0} HS", smallFood.Schedule.End), 8, 500);

                if (!smallFood.Foods.Count.Equals(0))
                {
                    PageBreak(150);

                    pdfElemment.TextLeft("ALIMENTOS", 8);
                    smallFood.Foods.ForEach(x => pdfElemment.TextLeft(x.Description, 8));
                    PageBreak();
                }

                if (!string.IsNullOrEmpty(smallFood.Drinks))
                {
                    PageBreak(60);
                    pdfElemment.SetAtualPosition(pdfElemment.NextPosition);
                    pdfElemment.TextLeft("BEBIDAS", 8);

                    pdfElemment.BigTextLeftColumn(smallFood.Drinks, 0.4f);
                }


                PageBreak();
                pdfElemment.SetAtualPosition(pdfElemment.NextPosition);

            }
        }
        private void HeaderFood(Meal food)
        {
            PageBreak(100);
            pdfElemment.HDivision();
            pdfElemment.SetAtualPosition(pdfElemment.Position - 15);
            pdfElemment.TextPositionX(string.Format("{0}: {1}", food.Type, food.Name), 8, 12);
            pdfElemment.TextPositionX(string.Format("PESSOAS: {0}", food.Quant), 8, 300);
            pdfElemment.TextPositionX(string.Format("UNIT.: R$ {0}", food.UnitValue), 8, 400);
            pdfElemment.TextPositionX(string.Format("R$ {0}", food.Total), 8, 500);
            pdfElemment.HDivision();
            PageBreak();
        }


        private void PrintDays(List<DaysEvent> days)
            => days.ForEach(x => PrintDays(x));


        private void PrintRooms(List<Room> rooms)
            => rooms.ForEach(x => PrintRoom(x));


        private void PrintRoom(Room room)
        {
            PageBreak(50);
            pdfElemment.HDivision();
            pdfElemment.TextLeft(".SALA", 10);
            pdfElemment.TextLeft(string.Format(".{0} - {1}", room.Order, room.Name), 7);

            PrintAgendas(room.Schedules);

            PrintDescriptionRoom(room);

            PrintImages(room.Images);
            PageBreak();
        }

        private void PrintDescriptionRoom(Room room)
        {
            PageBreak(30);
            pdfElemment.HDivision();
            pdfElemment.SetAtualPosition(pdfElemment.NextPosition);
            pdfElemment.TextPositionX(string.Format("FORMATO: {0}", room.Format), 8, 15);
            pdfElemment.TextPositionX(string.Format("ÁREA: {0} M²", room.Area), 8, 150);
            pdfElemment.TextPositionX(string.Format("PÉ DIREITO: {0} M", room.HighCeiling), 8, 300);
            pdfElemment.TextPositionX(string.Format("CONVIDADOS: {0}", room.QuantGuest), 8, 400);
            pdfElemment.TextPositionX(string.Format("R$ {0}", room.Price), 8, 500);
            PageBreak();
        }

        private void PrintAgendas(List<Agenda> schedules)
            => schedules.ForEach(x => PrintAgenda(x));


        private void PrintAgenda(Agenda schedule)
        {
            PageBreak(30);
            pdfElemment.SetAtualPosition(pdfElemment.NextPosition + 15);
            TextEspecificScheduleBeginning(schedule.Beginning);
            TextEspecificScheduleEnd(schedule.End);
            PageBreak();
        }

        private void TextEspecificScheduleBeginning(string schedule)
            => pdfElemment.TextPositionX(string.Format("INÍCIO: {0} HS", schedule), 8, 300);

        private void TextEspecificScheduleEnd(string schedule)
            => pdfElemment.TextPositionX(string.Format("TÉRMINO: {0} HS", schedule), 8, 400);

        public void SetDetailEvent(EventDetail eventDetail)
            => this._eventDetails = eventDetail;

        public void AddService(Lodging services)
            => this._lodging.Add(services);

        public void PrintService()
        {
            PageBreak(110);
            pdfElemment.TextLeft(".HOSPEDAGEM", 10);

            _lodging.ForEach(x => PrintLodging(x));

            PageBreak();
        }

        public void PrintLodging(Lodging lodging)
        {
            lodging.Accommodations.ForEach(x =>
            {
                PageBreak(300);

                pdfElemment.HDivision();
                pdfElemment.TextLeft(x.Service.Name, 8);

                pdfElemment.BigTextLeftColumn(x.Service.Description);
                pdfElemment.TextRightWithBorder(x.Service.TotalValue, x.Service.UnitValue, x.Daily, x.QuantRoom);

                pdfElemment.TextLeft("QUANT. CAMAS", BaseFont.HELVETICA_BOLD, 8, 12, pdfElemment.NextPosition);
                x.Beds.ForEach(b => pdfElemment.TextLeft(b, 8));

                pdfElemment.TextLeft("COMODIDADES", BaseFont.HELVETICA_BOLD, 8, 12, pdfElemment.NextPosition);
                pdfElemment.TextMultipleColumns(x.Service.Conveniences);

                if (!x.Service.Images.Count.Equals(0))
                {
                    PrintImages(x.Service.Images);
                }

            });

        }

        private void PrintImages(List<Image> images)
        {
            PageBreak(200);
            pdfElemment.HDivision();
            pdfElemment.ImagesInLine(images);
            PageBreak();
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
            PageBreak(100);
            var titles = new List<string> { LabelValues.DIAS, LabelValues.VALOR, LabelValues.PORCENTO, LabelValues.DESCONTOS, LabelValues.VALOR_FINAL };

            pdfElemment.Table(LabelNameTableValue.VALOR_DIA, titles, ContentForDays);
            PageBreak();
        }

        private void PrintTableValueForService()
        {
            PageBreak();
            var titles = new List<string> { LabelValues.ITEM, LabelValues.VALOR, LabelValues.PORCENTO, LabelValues.DESCONTOS, LabelValues.VALOR_FINAL };

            pdfElemment.Table(LabelNameTableValue.VALOR_SERVICO, titles, ContentForServices);
            PageBreak();
        }
        private void PrintTableValueForService2()
        {
            PageBreak(200);
            pdfElemment.HDivision();
            var titles = new List<string> { LabelValues.ITEM, LabelValues.VALOR, LabelValues.PORCENTO, LabelValues.DESCONTOS, LabelValues.VALOR_FINAL };

            pdfElemment.Table2(LabelNameTableValue.VALOR_SERVICO, titles, ContentForServices);
            PageBreak();
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
            pdfElemment.TextCenter(NameDocument, BaseFont.HELVETICA_BOLD, 20, pdfElemment.CenterX(), 810);
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
            pdfElemment.HDivision();
            var requester = _client.Requester ?? "Cliente Solicitante";
            var cnpjRequester = _client.CnpjRequester ?? "88.888.888/8888-88";
            var affiliate = _client.Affiliate ?? "Cliente Afiliado";
            var cnpjAffiliate = _client.CnpjAffiliate ?? "88.888.888/8888-88";

            pdfElemment.TextLeft(string.Format("{0} : {1}", LabelClient.CLIENTE_SOLICITANTE, requester), BaseFont.HELVETICA_BOLD, 8, 14, pdfElemment.NextPosition);
            pdfElemment.TextLeft(string.Format("{0} : {1}", LabelClient.CNPJ, cnpjRequester), BaseFont.HELVETICA_BOLD, 8, 14, pdfElemment.NextPosition);
            pdfElemment.TextLeft(string.Format("{0} : {1}", LabelClient.AFILIADO_CONTRATADO, affiliate), BaseFont.HELVETICA_BOLD, 8, 14, pdfElemment.NextPosition);
            pdfElemment.TextLeft(string.Format("{0} : {1}", LabelClient.CNPJ, cnpjAffiliate), BaseFont.HELVETICA_BOLD, 8, 14, pdfElemment.NextPosition);
            pdfElemment.HDivision();
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
            pdfElemment.CheckPosition(titleBody.MinSizeHeight);
            pdfElemment.SetAtualPosition(pdfElemment.Position - 30);
            pdfElemment.TextCenter(titleBody.TitleBody, BaseFont.HELVETICA_BOLD, titleBody.SizeFont, titleBody.ColorFont, pdfElemment.CenterX(), pdfElemment.Position);

            PrintBackgrounBody(titleBody);

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
