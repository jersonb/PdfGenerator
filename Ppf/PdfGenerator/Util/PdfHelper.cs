using iTextSharp.text;
using iTextSharp.text.pdf;
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
        public int Lines { get;private set; } = 45;
        #endregion

        internal PdfHelper(Document doc, MemoryStream ms)
        {
            this._doc = doc;
            var pdf = PdfWriter.GetInstance(doc, ms);
            pdf.CloseStream = false;
            this._doc.Open();
            this._contentByte = pdf.DirectContent;
        }

        #region Text
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
            this._contentByte.MoveTo(this._doc.LeftMargin, position);
            this._contentByte.LineTo(this._doc.PageSize.Width - this._doc.RightMargin, position);
            this._contentByte.Stroke();
        }
        #endregion

        #region Position
        internal void ResetLines() => Lines = 45;
        internal void AdjustLines(int lines) => Lines += lines;

        private float GetCurrentPosition() => _contentByte.YTLM;

        private float GetNextPosition() => _contentByte.YTLM - 15;

        internal void NextPage()
        {
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
                TextCenter(i.ToString(), BaseFont.COURIER, 8, _doc.PageSize.Width / 2, NextPosition);


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
