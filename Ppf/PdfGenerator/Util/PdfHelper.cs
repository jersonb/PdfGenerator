using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.IO;

namespace PdfGenerator.Util
{
    public class PdfHelper
    {
        #region Properties
        internal float NextPosition
            => GetNextPosition();

        public float Position { get; private set; } = 790;

        private readonly Document _doc;
        private readonly PdfContentByte _contentByte;
        private PdfWriter pdf;
        public bool PageBreak { get; private set; }
        #endregion

        internal PdfHelper(Document doc, MemoryStream ms)
        {
            this._doc = doc;
            pdf = PdfWriter.GetInstance(doc, ms);
            pdf.CloseStream = false;
            this._doc.Open();
            this._contentByte = pdf.DirectContent;
        }

        #region Table
        internal void Table(string nameTable, List<string> titles, List<string> content)
        {              
            var countColumn = titles.Count;

            var table = new PdfPTable(countColumn)
            {
                TotalWidth = _doc.PageSize.Width - 50,
                HorizontalAlignment = Element.ALIGN_CENTER,
                KeepTogether = true,
                SplitLate = false,

            };

            var tableheader = new PdfPCell(CellTableHeader(nameTable))
            {
                Colspan = countColumn,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Border = 0
                
            };

            table.AddCell(tableheader);

            titles.ForEach(x => table.AddCell(CellTableTitle(x)));

            content.ForEach(x => table.AddCell(CellTableBody(x)));

            SetAtualPosition(this.Position - 10);
            CheckPosition(table.TotalHeight + 100);
            this._contentByte.SetColorStroke(BaseColor.BLACK);

            var factorCorrectionHeight = 10;
            table.WriteSelectedRows(0, -1, 15, Position, _contentByte);
            Rectangle(25, Position - (factorCorrectionHeight + table.TotalHeight), table.TotalWidth, table.TotalHeight + factorCorrectionHeight, 0.1f, 1, BaseColor.GRAY,BaseColor.WHITE,0.001f);


            HDivision();
           
            CheckPosition(100);
        }
       internal void Table2(string nameTable, List<string> titles, List<string> content)
        {              
            var countColumn = titles.Count;

            var table = new PdfPTable(countColumn)
            {
                TotalWidth = _doc.PageSize.Width - 50,
                HorizontalAlignment = Element.ALIGN_CENTER,
                KeepTogether = true,
                SplitLate = false,

            };

            var tableheader = new PdfPCell(CellTableHeader(nameTable))
            {
                Colspan = countColumn,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Border = 0
                
            };

            table.AddCell(tableheader);

            titles.ForEach(x => table.AddCell(CellTableTitle(x)));

            content.ForEach(x => table.AddCell(CellTableBody(x)));

            SetAtualPosition(this.Position - 10);
            CheckPosition(table.TotalHeight + 100);
            this._contentByte.SetColorStroke(BaseColor.BLACK);

            var factorCorrectionHeight = 10;
            table.WriteSelectedRows(0, -1, 15, Position, _contentByte);
            Rectangle(25, Position - (factorCorrectionHeight + table.TotalHeight), table.TotalWidth, table.TotalHeight + factorCorrectionHeight, 0.1f, 1, BaseColor.GRAY,BaseColor.WHITE,0.001f);

        }

        private PdfPCell CellTable(string text, string nameFont, int sizeFont)
            => new PdfPCell(TetxtTable(text, nameFont, sizeFont))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Border = 0,
            };


        private PdfPCell CellTableHeader(string text)
            => CellTable(text, BaseFont.HELVETICA_BOLD, 12);


        private PdfPCell CellTableTitle(string text)
            => CellTable(text, BaseFont.HELVETICA_BOLD, 8);


        private PdfPCell CellTableBody(string text)
            => CellTable(text, BaseFont.HELVETICA, 8);


        private Phrase TetxtTable(string text, string nameFont, int sizeFont)
            => new Phrase(text, new Font(GetFont(nameFont), sizeFont));

        #endregion

        #region Column
        internal void TextMultipleColumns(List<string> list)
        {
            SetAtualPosition(NextPosition);
            if (list.Count <= 4)
            {
                Column1(list.GetRange(0, list.Count));
            }

            if (list.Count > 4 && list.Count <= 8)
            {
                Column1(list.GetRange(0, 4));
                Column2(list.GetRange(4, list.Count - 4));
            }

            if (list.Count > 8 && list.Count <= 12)
            {
                Column1(list.GetRange(0, 4));
                Column2(list.GetRange(4, 4));
                Column3(list.GetRange(8, list.Count - 8));
            }

            if (list.Count > 12 && list.Count <= 16)
            {
                Column1(list.GetRange(0, 4));
                Column2(list.GetRange(4, 4));
                Column3(list.GetRange(8, 4));
                Column4(list.GetRange(12, list.Count - 12));
            }

            if (list.Count > 16)
            {
                Column1(list.GetRange(0, 4));
                Column2(list.GetRange(4, 4));
                Column3(list.GetRange(8, 4));
                Column4(list.GetRange(12, 4));
            }

            CheckPosition(200);
            SetAtualPosition(Position - 30);
        }

        private void Column(List<string> list, float llx)
        {
            var columnText = new ColumnText(_contentByte)
            {
                Alignment = Element.ALIGN_LEFT,
                AdjustFirstLine = false,
                SpaceCharRatio = 1,
                Leading = 9,
                ExtraParagraphSpace = 0
            };

            var lly = Position - (list.Count * 8);
            var urx = llx + (_doc.PageSize.Width / 4);
            var ury = Position;

            columnText.SetIndent(0, true);

            list.ForEach(x =>
            {
                columnText.AddText(new Phrase(x + "\n", new Font(GetFont(BaseFont.HELVETICA), 8)));
            });

            columnText.SetSimpleColumn(llx, lly, urx, ury + 10);

            columnText.Go();
        }

        private void Column1(List<string> list)
        {
            Column(list, _doc.LeftMargin + 10);
        }

        private void Column2(List<string> list)
        {
            Column(list, (_doc.PageSize.Width / 4) + 15);
        }

        private void Column3(List<string> list)
        {
            Column(list, ((_doc.PageSize.Width / 4) * 2) + 15);
        }

        private void Column4(List<string> list)
        {
            Column(list, (_doc.PageSize.Width / 4) * 3);
        }

        internal void TextRightWithBorder(string total, string forRoom, string daily, string rooms)
        {
            var font = BaseFont.HELVETICA_BOLD;

            var columnText = new ColumnText(_contentByte)
            {
                Alignment = Element.ALIGN_RIGHT,
                AdjustFirstLine = false,
                SpaceCharRatio = 1,
                Leading = 9,
                ExtraParagraphSpace = 0
            };

            var llx = _doc.PageSize.Width * 0.75f;
            var lly = Position;
            var urx = _doc.PageSize.Width - (_doc.Left * 2);
            var ury = Position + 60;

            columnText.SetIndent(0, true);

            columnText.AddText(new Phrase(total + "\n", new Font(GetFont(font), 10)));
            columnText.AddText(new Phrase(forRoom + "\n", new Font(GetFont(font), 8)));
            columnText.AddText(new Phrase(daily + "\n", new Font(GetFont(font), 8)));
            columnText.AddText(new Phrase(rooms + "\n", new Font(GetFont(font), 8)));

            var adjustX = 20f;
            var adjustY = 10f;

            columnText.SetSimpleColumn(llx - adjustX, lly - adjustY, urx - adjustX, ury - adjustY);
            columnText.Go();
            Rectangle(llx, lly, urx - llx, ury - lly, 0.3f, 0, BaseColor.GRAY);
            SetAtualPosition(lly);

        }

        internal void BigTextLeftColumn(string text)
            => BigTextLeftColumn(text, 0.6f);

        internal void BigTextLeftColumn(string text, float percentPage)
        {
            var phrase = new Phrase(text, new Font(GetFont(BaseFont.HELVETICA), 7));
            var columnText = new ColumnText(_contentByte)
            {
                Alignment = Element.ALIGN_LEFT,
                AdjustFirstLine = false,
              
                Leading = 10,
              
            };

            var llx = 25f;
            var lly = (Position - (text.Length / 5.5f));
            var urx = _doc.PageSize.Width * percentPage;
            var ury = Position;

            columnText.SetIndent(0, true);
            columnText.SetText(phrase);

            columnText.SetSimpleColumn(llx, lly, urx, ury);

            CheckPosition(ury - lly);
            columnText.Go();
            SetAtualPosition(lly+20);
        }
        #endregion

        #region Text
        private void Text(string text, string nameFont, int sizeFont, float positionX, float positionY, BaseColor color, int align)
        {
            CheckPosition(30);
            var font = GetFont(nameFont);
            this._contentByte.SetColorFill(color);
            this._contentByte.SetFontAndSize(font, sizeFont);
            this._contentByte.BeginText();
            this._contentByte.ShowTextAligned(align, text, positionX, positionY, 0);
            this._contentByte.EndText();
        }

        internal void TextPositionX(string text, int sizeFont, float positionX)
            => Text(text, BaseFont.HELVETICA, sizeFont, positionX, Position, BaseColor.BLACK, Element.ALIGN_LEFT);

        internal void TextCenter(string text, string nameFont, int sizeFont, float positionX, float positionY)
            => Text(text, nameFont, sizeFont, positionX, positionY, BaseColor.BLACK, Element.ALIGN_CENTER);

        internal void TextCenter(string text, string nameFont, int sizeFont, BaseColor colorFont, float positionX, float positionY)
            => Text(text, nameFont, sizeFont, positionX, positionY, colorFont, Element.ALIGN_CENTER);

        internal void TextLeft(string text, string nameFont, int sizeFont, float positionX, float positionY)
            => Text(text, nameFont, sizeFont, positionX, positionY, BaseColor.BLACK, Element.ALIGN_LEFT);

        internal void TextLeft(string text, int sizeFont)
           => TextLeft(text, BaseFont.HELVETICA, sizeFont, 12, NextPosition);
       
        internal void TextLeft(string text, int sizeFont, string nameFont)
           => TextLeft(text, nameFont, sizeFont, 12, NextPosition);

        private BaseFont GetFont(string font)
            => BaseFont.CreateFont(font, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        #endregion

        #region Image and formats
        internal void ImagesInLine(List<Image> images)
        {
            int i = 0;
            CheckPosition(200);

            SetAtualPosition(Position - 90);

            foreach (var image in images)
            {
                AddImage(image, GetPositionByIndex(i++), Position, (_doc.PageSize.Width / 4) - 10, 200);

                if (i == 4)
                {
                    i = 0;
                    Position -= 120;
                }
            }

            CheckPosition(200);

            SetAtualPosition(Position);
        }

        private float GetPositionByIndex(int index)
        {
            switch (index)
            {
                case 0: return _doc.Left;
                case 1: return (_doc.PageSize.Width / 4) + 5;
                case 2: return ((_doc.PageSize.Width / 4) * index) + 3;
                case 3: return (_doc.PageSize.Width / 4) * index;

                default: return 0;
            }
        }

        internal void AddImage(Image image, float absoluteX, float absoluteY, float width, float height)
        {
            image.ScaleToFit(width, height);
            image.SetAbsolutePosition(absoluteX, absoluteY);

            this._doc.Add(image);
        }

        internal void AddImage(Image image, float absoluteX, float absoluteY)
            => AddImage(image, absoluteX, absoluteY, image.Width, image.Height);

       
        internal void Rectangle(float xInit, float yInit, float width, float height, float widthLine, float radius, BaseColor boaderColor, BaseColor internColor, float opacity)
        {
            this._contentByte.SetColorStroke(boaderColor);
            this._contentByte.SetColorFill(internColor);
            this._contentByte.SaveState();
            var gs1 = new PdfGState
            {
                FillOpacity = opacity
            };
            this._contentByte.SetGState(gs1);
            this._contentByte.RoundRectangle(xInit, yInit, width, height, radius);
            this._contentByte.SetLineWidth(widthLine);
            this._contentByte.FillStroke();
            this._contentByte.RestoreState();
            SetAtualPosition(yInit+5);
            
        }

        internal void Rectangle(float xInit, float yInit, float width, float height, float widthLine, float radius, BaseColor boaderColor)
            => Rectangle(xInit, yInit, width, height, widthLine, radius, boaderColor, BaseColor.WHITE,0.0f);

        internal void HLine(float position)
        {
            this._contentByte.SetLineWidth(1f);
            this._contentByte.SetColorStroke(BaseColor.GRAY);
            this._contentByte.MoveTo(this._doc.LeftMargin, position);
            this._contentByte.LineTo(this._doc.PageSize.Width - this._doc.RightMargin, position);
            this._contentByte.Stroke();
        }

        internal void HDivision()
        {
            CheckPosition(30);
            HLine(NextPosition);
        }
        #endregion

        #region Position
        internal float CenterX()
            => _doc.PageSize.Width / 2;

        private float GetNextPosition()
        {
            SetAtualPosition(Position - 15);
            return Position;
        }

        public void CheckPosition(float minPosition)
            => PageBreak = minPosition > (this.Position - _doc.BottomMargin);


        public void SetAtualPosition(float position)
            => TextCenter(" ", BaseFont.HELVETICA, 8, 25, this.Position = position);


        internal void NextPage()
        {
            PageBreak = false;
            _contentByte.NewPath();
            _doc.NewPage();
            SetAtualPosition(770);
        }
        #endregion

        #region Grid
        internal void ShowGrid() => ShowGrid(true);

        internal void ShowGridAndBoarderLimit()
        {
            ShowGrid(true);
            var xInit = this._doc.Left;
            var yInit = this._doc.Bottom;
            var width = this._doc.PageSize.Width - this._doc.RightMargin - this._doc.LeftMargin;
            var heigth = this._doc.PageSize.Height - this._doc.TopMargin - this._doc.BottomMargin;

            Rectangle(xInit, yInit, width, heigth, 1, 1, BaseColor.RED);
        }

        internal void ShowBoarderLimit()
        {
            this._contentByte.SetLineWidth(1f);
            var xInit = this._doc.Left;
            var yInit = this._doc.Bottom;
            var width = this._doc.PageSize.Width - this._doc.RightMargin - this._doc.LeftMargin;
            var heigth = this._doc.PageSize.Height - this._doc.TopMargin - this._doc.BottomMargin;

            Rectangle(xInit, yInit, width, heigth, 1, 1, BaseColor.BLACK);
        }

        internal void ShowGrid(bool activate)
        {
            if (activate)
            {
                var bf = GetFont(BaseFont.COURIER);
                this._contentByte.SetLineWidth(1f);
                this._contentByte.SetColorFill(BaseColor.DARK_GRAY);
                this._contentByte.SetColorStroke(BaseColor.BLACK);
                this._contentByte.SetFontAndSize(bf, 8);
                this._contentByte.SetLineDash(1f, 1f);
                this._contentByte.BeginText();

                for (float i = 0; i < this._doc.PageSize.Height; i++)
                {
                    if (i % 50 == 0)
                    {
                        this._contentByte.ShowTextAligned(1, i.ToString(), 8, i, 0);
                    }
                }

                for (float i = 0; i < this._doc.PageSize.Width; i++)
                {
                    if (i % 50 == 0)
                    {
                        this._contentByte.ShowTextAligned(1, i.ToString(), i, this._doc.PageSize.Height - 10, 0);
                    }
                }

                this._contentByte.EndText();

                for (float i = 0; i < this._doc.PageSize.Height; i++)
                {
                    if (i % 50 == 0)
                    {
                        this._contentByte.MoveTo(0, i);
                        this._contentByte.LineTo(this._doc.PageSize.Width, i);
                    }
                }

                for (float i = 0; i < this._doc.PageSize.Width; i++)
                {
                    if (i % 50 == 0)
                    {
                        this._contentByte.MoveTo(i, 0);
                        this._contentByte.LineTo(i, this._doc.PageSize.Height);
                    }
                }

                this._contentByte.Stroke();

            }

        }
        #endregion
    }
}
