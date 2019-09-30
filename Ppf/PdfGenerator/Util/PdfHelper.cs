using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.IO;

namespace PdfGenerator.Util
{
    public class PdfHelper
    {
        #region Properties
        internal float CurrentPosition
        {
            get
            {
                return GetCurrentPosition();
            }
        }

        internal float NextPosition
        {
            get
            {
                return GetNextPosition();
            }
        }

        public float InitialPosition { get; set; }

        private readonly Document _doc;
        private readonly PdfContentByte _contentByte;
        private PdfWriter pdf;
        public int Lines { get; private set; } = 45;
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
            if ((NextPosition + table.TotalHeight) > 41)
            {

                table.WriteSelectedRows(0, -1, 15, NextPosition - 15, _contentByte);
            }

            Lines -= (table.Rows.Count + 3);

            var factoCorrectionPosition = 20;
            var factoCorrectionHeight = 10;

            Rectangle(25, NextPosition - (factoCorrectionPosition + table.TotalHeight), table.TotalWidth, table.TotalHeight + factoCorrectionHeight, 0.1f, 1, BaseColor.GRAY);


            TextCenter(" ", BaseFont.HELVETICA, 8, 25, NextPosition - (factoCorrectionPosition + table.TotalHeight));

            HDivision(12);
            HLine();
        }

        private PdfPCell CellTableHeader(string text)
        {
            return new PdfPCell(TetxtTable(text, BaseFont.HELVETICA_BOLD, 12))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Border = 0,
            };
        }

        private PdfPCell CellTableTitle(string text)
        {
            return new PdfPCell(TetxtTable(text, BaseFont.HELVETICA_BOLD, 8))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Border = 0,
            };
        }

        private PdfPCell CellTableBody(string text)
        {
            return new PdfPCell(TetxtTable(text, BaseFont.HELVETICA, 8))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Border = 0,
            };
        }

        private Phrase TetxtTable(string text, string nameFont, int sizeFont)
        {
            return new Phrase(text, new Font(BaseFont.CreateFont(nameFont, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), sizeFont));
        }
        #endregion


        #region Text

        internal void TextRightBorder(string[] iten)
        {
            var total = iten[0];
            var forRoom = iten[1];
            var daily = iten[2];
            var roons = iten[3];
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
            var lly = NextPosition-60;
            var urx = _doc.PageSize.Width -(_doc.Left*2);
            var ury = NextPosition;

            columnText.SetIndent(0, true);
          
            columnText.AddText(new Phrase(total + "\n", new Font(GetFont(font), 10)));
            columnText.AddText(new Phrase(forRoom + "\n", new Font(GetFont(font), 8)));
            columnText.AddText(new Phrase(daily + "\n", new Font(GetFont(font), 8)));
            columnText.AddText(new Phrase(roons + "\n", new Font(GetFont(font), 8)));

            var adjustX = 20f;
            var adjustY = 10f;

            columnText.SetSimpleColumn(llx-adjustX, lly-adjustY, urx-adjustX, ury-adjustY);
            columnText.Go();
            Rectangle(llx,lly,urx-llx,ury-lly,0.3f,0,BaseColor.GRAY);
            TextCenter("", BaseFont.HELVETICA, 8, 25, lly);

        }

        internal void TextLeftColumn(string text)
        {
            var phrase = new Phrase(text, new Font(GetFont(BaseFont.HELVETICA), 7));
            var columnText = new ColumnText(_contentByte)
            {
                Alignment = Element.ALIGN_LEFT,
                AdjustFirstLine = false,
                SpaceCharRatio = 1,
                Leading = 8,
                ExtraParagraphSpace = 0,
            };

            var llx = 25;
            var lly = (CurrentPosition - (text.Length / 5.5f))<200f ? 200f:(CurrentPosition - (text.Length / 5.5f));
            var urx = _doc.PageSize.Width * 0.60f;
            var ury = NextPosition;

            columnText.SetIndent(0, true);
            columnText.SetText(phrase);

            columnText.SetSimpleColumn(llx, lly, urx, ury);

            columnText.Go();
           
            Lines -= columnText.LinesWritten;
        }

        internal void TextCenter(string text, string nameFont, int sizeFont, float positionX, float positionY)
        {
            Lines--;

            var font = GetFont(nameFont);
            this._contentByte.SetColorFill(BaseColor.BLACK);
            this._contentByte.SetFontAndSize(font, sizeFont);
            this._contentByte.BeginText();
            this._contentByte.ShowTextAligned(Element.ALIGN_CENTER, text, positionX, positionY, 0);
            this._contentByte.EndText();
        }
        internal void TextRigth(string text, string nameFont, int sizeFont, float positionX, float positionY)
        {
            Lines--;

            var font = GetFont(nameFont);
            this._contentByte.SetColorFill(BaseColor.BLACK);
            this._contentByte.SetFontAndSize(font, sizeFont);
            this._contentByte.BeginText();
            this._contentByte.ShowTextAligned(Element.ALIGN_RIGHT, text, positionX, positionY, 0);
            this._contentByte.EndText();
        }

        internal void TextLeft(string text, string nameFont, int sizeFont, float positionX, float positionY)
        {
            Lines--;
            var bf = GetFont(nameFont);
            this._contentByte.SetColorFill(BaseColor.BLACK);
            this._contentByte.SetFontAndSize(bf, sizeFont);
            this._contentByte.BeginText();
            this._contentByte.ShowTextAligned(Element.ALIGN_LEFT, text, positionX, positionY, 0);
            this._contentByte.EndText();
        }

        private BaseFont GetFont(string font) => BaseFont.CreateFont(font, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        #endregion

        #region Image and formats
        internal void AddImage(Image image, float absoluteX, float absoluteY)
        {
            image.ScaleToFit(image.Width, image.Height);
            image.SetAbsolutePosition(absoluteX, absoluteY);
            this._doc.Add(image);
        }

        internal void Rectangle(float xInit, float yInit, float width, float height, float widthLine, float radius, BaseColor boaderColor, BaseColor internColor)
        {
            Lines--;
            Lines--;
            this._contentByte.SetColorStroke(boaderColor);
            this._contentByte.SetColorFill(internColor);
            this._contentByte.RoundRectangle(xInit, yInit, width, height, radius);
            this._contentByte.SetLineWidth(widthLine);
            this._contentByte.FillStroke();
        }

        internal void Rectangle(float xInit, float yInit, float width, float height, float widthLine, float radius, BaseColor boaderColor)
        {
            Lines--;
            Lines--;
            this._contentByte.SetColorStroke(boaderColor);
            this._contentByte.RoundRectangle(xInit, yInit, width, height, radius);
            this._contentByte.SetLineWidth(widthLine);
            this._contentByte.Stroke();
        }

        internal void HLine()
        {
            Lines--;
            var position = NextPosition;
            this._contentByte.SetLineWidth(1f);
            this._contentByte.SetColorStroke(BaseColor.GRAY);
            this._contentByte.MoveTo(this._doc.LeftMargin, position);
            this._contentByte.LineTo(this._doc.PageSize.Width - this._doc.RightMargin, position);
            this._contentByte.Stroke();
        }
        internal void HDivision(float position)
        {
            Lines--;
            position -= 5;
            this._contentByte.SetLineWidth(1f);
            this._contentByte.SetColorStroke(BaseColor.GRAY);
            this._contentByte.MoveTo(this._doc.LeftMargin, position);
            this._contentByte.LineTo(this._doc.PageSize.Width - this._doc.RightMargin, position);
            this._contentByte.Stroke();
        }
        #endregion

        #region Position
        internal void ResetLines() => Lines = 45;
        internal void AdjustLines(int lines) => Lines += lines;

        internal void NextLine() => TextCenter(" ", BaseFont.HELVETICA, 8, 25, NextPosition);

        internal float CenterX() => _doc.PageSize.Width / 2;

        private float GetCurrentPosition() => _contentByte.YTLM;

        private float GetNextPosition() => _contentByte.YTLM - 15;

        internal void NextPage()
        {
            _contentByte.NewPath();
            _doc.NewPage();

            ResetLines();
        }
        #endregion

        #region Grid
        internal void ShowGrid() => ShowGrid(true);

        internal void ShowLines()
        {

            for (int i = 0; i < 100; i++)
            {
                TextCenter(i.ToString(), BaseFont.COURIER, 8, CenterX(), NextPosition);


                if (Lines == 0)
                {
                    NextPage();
                    ShowGrid();
                }
            }

        }

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
