using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfGenerator.Models;
using System.IO;

namespace PdfGenerator.Util
{
    public class PdfHelper
    {
        private readonly Document _doc;
        private readonly PdfContentByte _contentByte;

        public string NameFont { private get; set; }
        public int SizeFont { private get; set; }

        internal PdfHelper(Document doc, PdfWriter pdf)
        {
            this._doc = doc;
            this._contentByte = pdf.DirectContent;
        }

        internal void NewPage(HeaderFooterConfig header, HeaderFooterConfig footer)
        {
            _doc.NewPage();

            SetHeader(header);
            SetFooter(footer);
        }


        internal void SetHeader(HeaderFooterConfig config)
        {

            if (config.ShowBoarder)
            {
                var spacing = config.Spacing != 0 ? config.Spacing : 1f;
                var boarderWidth = config.BoarderWidth != 0 ? config.BoarderWidth : 1;
                var radius = config.BoardRadius != 0 ? config.BoardRadius : 0;
                var lowerLeftX = config.LowerLeftX != 0 ? config.LowerLeftX : (_doc.RightMargin - spacing);
                var lowerLeftY = config.LowerLeftY != 0 ? config.LowerLeftY : (_doc.PageSize.Height - _doc.TopMargin - spacing);
                var widthRectangle = config.WidthRectangle != 0 ? config.WidthRectangle : (_doc.Right - _doc.RightMargin - spacing);
                var heigthRectangle = config.HeigthRectangle != 0 ? config.HeigthRectangle : (_doc.TopMargin - spacing);
                var backColor = config.BackColor ?? BaseColor.WHITE;
                var boarderColor = config.BackColor ?? BaseColor.BLACK;

                Rectangle(lowerLeftX, lowerLeftY, widthRectangle, heigthRectangle, boarderWidth, radius, boarderColor, backColor);
            }

            if (config.Logo != null)
            {
                AddImageToHeader(config.Logo);
            }

        }

        private void AddImageToHeader(Image image)
        {
            image.ScaleToFit(image.Width, image.Height);
            image.SetAbsolutePosition(0, _doc.PageSize.Height - _doc.TopMargin);
            this._doc.Add(image);
        }

        internal void SetFooter(HeaderFooterConfig config)
        {
            if (config.ShowBoarder)
            {
                var spacing = config.Spacing != 0 ? config.Spacing : 1f;
                var boarderWidth = config.BoarderWidth != 0 ? config.BoarderWidth : 1;
                var radius = config.BoardRadius != 0 ? config.BoardRadius : 0;
                var lowerLeftX = config.LowerLeftX != 0 ? config.LowerLeftX : (_doc.RightMargin - spacing);
                var lowerLeftY = spacing;
                var widthRectangle = config.WidthRectangle != 0 ? config.WidthRectangle : (_doc.Right - _doc.RightMargin - spacing);
                var heigthRectangle = config.HeigthRectangle != 0 ? config.HeigthRectangle : (_doc.BottomMargin - spacing);
                var backColor = config.BackColor ?? BaseColor.WHITE;
                var boarderColor = config.BackColor ?? BaseColor.BLACK;

                Rectangle(lowerLeftX, lowerLeftY, widthRectangle, heigthRectangle, boarderWidth, radius, boarderColor, backColor);

            }

            if (config.Logo != null)
            {
                AddImageToFooter(config.Logo);
            }

        }
      
        private void AddImageToFooter(Image image)
        {
            image.ScaleToFit(image.Width, image.Height);
            image.SetAbsolutePosition(0,0);
            this._doc.Add(image);
        }

        internal BaseFont GetFont(string font)
        {
            return BaseFont.CreateFont(font, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        }

        internal BaseFont GetFont()
        {
            return BaseFont.CreateFont(this.NameFont ?? BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        }




        internal void AddImage(string path, float fitWidth, float fitHeight, float absoluteX, float absoluteY)
        {
            var st = File.Open(path, FileMode.Open);

            var image = Image.GetInstance(st);
            image.ScaleToFit(fitWidth, fitHeight);
            image.SetAbsolutePosition(absoluteX, absoluteY);
            this._doc.Add(image);
        }

        internal void AddImage(Image image, float fitWidth, float fitHeight, float absoluteX, float absoluteY)
        {
            image.ScaleToFit(fitWidth, fitHeight);
            image.SetAbsolutePosition(absoluteX, absoluteY);
            this._doc.Add(image);
        }

        internal void TextColumn(string text, string nameFont, int sizeFont, float xInfEsq, float yInfEsq, float xSupDir, float ySupDir)
        {
            var ct = new ColumnText(this._contentByte);

            ct.SetSimpleColumn(xInfEsq, yInfEsq, xSupDir, ySupDir);
            ct.AddElement(TextJustified(text, nameFont, sizeFont));
            ct.Go();
        }

        internal void Rectangle(float xInit, float yInit, float width, float height, float widthLine, float radius)
        {
            this._contentByte.SetColorFill(BaseColor.BLACK);
            this._contentByte.RoundRectangle(xInit, yInit, width, height, radius);
            this._contentByte.SetLineWidth(widthLine);
            this._contentByte.Stroke();
        }

        internal void Rectangle(float xInit, float yInit, float width, float height, float widthLine, float radius, BaseColor boaderColor, BaseColor internColor)
        {
            this._contentByte.SetColorStroke(boaderColor);
            this._contentByte.SetColorFill(internColor);
            this._contentByte.RoundRectangle(xInit, yInit, width, height, radius);
            this._contentByte.SetLineWidth(widthLine);
            this._contentByte.FillStroke();
        }


        internal void HLine(float position)
        {
            this._contentByte.MoveTo(this._doc.LeftMargin, position);
            this._contentByte.LineTo(this._doc.PageSize.Width - this._doc.RightMargin, position);
            this._contentByte.Stroke();
        }

        internal void VLine(float x, float yInit, float yFinal)
        {
            this._contentByte.SetColorFill(BaseColor.BLACK);

            this._contentByte.MoveTo(x, yInit);
            this._contentByte.LineTo(x, yFinal);
            this._contentByte.Stroke();
        }

        internal void VLine(float x, float yInit, float yFinal, BaseColor color)
        {
            this._contentByte.SetColorFill(color);

            this._contentByte.MoveTo(x, yInit);
            this._contentByte.LineTo(x, yFinal);
            this._contentByte.Stroke();
        }

        internal void TextCenter(string text, string nameFont, int sizeFont, float positionX, float positionY)
        {
            var bf = GetFont(nameFont);
            this._contentByte.SetColorFill(BaseColor.BLACK);
            this._contentByte.SetFontAndSize(bf, sizeFont);
            this._contentByte.BeginText();
            this._contentByte.ShowTextAligned(Element.ALIGN_CENTER, text, positionX, positionY, 0);
            this._contentByte.EndText();
        }

        internal void TextRigth(string text, string nameFont, int sizeFont, float positionX, float positionY)
        {
            var bf = GetFont(nameFont);
            this._contentByte.SetColorFill(BaseColor.BLACK);
            this._contentByte.SetFontAndSize(bf, sizeFont);
            this._contentByte.BeginText();
            this._contentByte.ShowTextAligned(Element.ALIGN_LEFT, text, positionX, positionY, 0);
            this._contentByte.EndText();
        }

        internal void TextLeft(string text, string nameFont, int sizeFont, float positionX, float positionY)
        {
            var bf = GetFont(nameFont);
            this._contentByte.SetColorFill(BaseColor.BLACK);
            this._contentByte.SetFontAndSize(bf, sizeFont);
            this._contentByte.BeginText();
            this._contentByte.ShowTextAligned(Element.ALIGN_RIGHT, text, positionX, positionY, 0);
            this._contentByte.EndText();
        }

        internal Paragraph Text(string text, string nameFont, int sizeFont, int alignText)
        {
            var font = GetFont(nameFont);
            var paragraph = new Paragraph(text, new Font(font, sizeFont, Font.NORMAL, BaseColor.BLACK));
            paragraph.Alignment = alignText;

            return paragraph;
        }

        internal Paragraph TextCenter(string text, string nameFont, int sizeFont)
        {
            var font = GetFont(nameFont);
            var paragraph = new Paragraph(text, new Font(font, sizeFont, Font.NORMAL, BaseColor.BLACK));
            paragraph.Alignment = Element.ALIGN_CENTER;

            return paragraph;

        }

        internal Paragraph TextCenter(string text, int sizeFont)
        {
            var font = GetFont();
            var paragraph = new Paragraph(text, new Font(font, sizeFont, Font.NORMAL, BaseColor.BLACK));
            paragraph.Alignment = Element.ALIGN_CENTER;

            return paragraph;

        }

        internal Paragraph TextCenter(string text)
        {
            var font = GetFont();
            var paragraph = new Paragraph(text, new Font(font, this.SizeFont != 0 ? this.SizeFont : 12, Font.NORMAL, BaseColor.BLACK));
            paragraph.Alignment = Element.ALIGN_CENTER;

            return paragraph;

        }

        internal Paragraph TextJustified(string text, string nameFont, int sizeFont)
        {
            var font = GetFont(nameFont);
            var paragraph = new Paragraph(text, new Font(font, sizeFont, Font.NORMAL, BaseColor.BLACK));
            paragraph.Alignment = Element.ALIGN_JUSTIFIED;

            return paragraph;
        }

        internal Paragraph TextJustified(string text, int sizeFont)
        {
            var font = GetFont();
            var paragraph = new Paragraph(text, new Font(font, sizeFont, Font.NORMAL, BaseColor.BLACK));
            paragraph.Alignment = Element.ALIGN_JUSTIFIED;

            return paragraph;
        }

        internal Paragraph TextJustified(string text)
        {
            var font = GetFont();
            var paragraph = new Paragraph(text, new Font(font, this.SizeFont != 0 ? this.SizeFont : 12, Font.NORMAL, BaseColor.BLACK));
            paragraph.Alignment = Element.ALIGN_JUSTIFIED;

            return paragraph;
        }

        internal Paragraph TextLeft(string text, string nameFont, int sizeFont)
        {
            var font = GetFont(nameFont);
            var paragraph = new Paragraph(text, new Font(font, sizeFont, Font.NORMAL, BaseColor.BLACK));
            paragraph.Alignment = Element.ALIGN_LEFT;

            return paragraph;
        }

        internal Paragraph TextLeft(string text, int sizeFont)
        {
            var font = GetFont();
            var paragraph = new Paragraph(text, new Font(font, sizeFont, Font.NORMAL, BaseColor.BLACK));
            paragraph.Alignment = Element.ALIGN_LEFT;

            return paragraph;
        }

        internal Paragraph TextLeft(string text)
        {
            var font = GetFont();
            var paragraph = new Paragraph(text, new Font(font, this.SizeFont != 0 ? this.SizeFont : 12, Font.NORMAL, BaseColor.BLACK));
            paragraph.Alignment = Element.ALIGN_LEFT;

            return paragraph;
        }


        internal Paragraph TextRigth(string text, int sizeFont, string nameFont)
        {
            var font = GetFont(nameFont);
            var paragraph = new Paragraph(text, new Font(font, sizeFont, Font.NORMAL, BaseColor.BLACK));
            paragraph.Alignment = Element.ALIGN_RIGHT;

            return paragraph;
        }

        internal Paragraph TextRigth(string text, int sizeFont)
        {
            var font = GetFont();
            var paragraph = new Paragraph(text, new Font(font, sizeFont, Font.NORMAL, BaseColor.BLACK));
            paragraph.Alignment = Element.ALIGN_RIGHT;

            return paragraph;
        }
        internal Paragraph TextRigth(string text)
        {
            var font = GetFont();
            var paragraph = new Paragraph(text, new Font(font, this.SizeFont != 0 ? this.SizeFont : 12, Font.NORMAL, BaseColor.BLACK));
            paragraph.Alignment = Element.ALIGN_RIGHT;

            return paragraph;
        }

        internal void AddGrid()
        {
            AddGrid(true);
        }

        internal void AddGrid(bool activate)
        {
            if (activate)
            {
                var bf = GetFont(BaseFont.COURIER);
                this._contentByte.SetColorFill(BaseColor.DARK_GRAY);
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
    }
}
