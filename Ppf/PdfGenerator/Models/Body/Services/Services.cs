using iTextSharp.text;
using System.Collections.Generic;

namespace PdfGenerator.Models.Body.Services
{
    public class Services
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string TotalValue { get; private set; }
        public string UnitValue { get; private set; }
      
        public List<string> Conveniences { get; private set; }
        public List<Image> Images { get; private set; }


        private Services()
        {
            Conveniences = new List<string>();
            Images = new List<Image>();
        }

        public static Services Create(string name, string description, string totalValue, string unitValue)
        {
            return new Services
            {   Name = name,
                Description = description,
                TotalValue = totalValue,
                UnitValue = unitValue
            };
        }

        public void SetConveniences(List<string> conveniences) => this.Conveniences.AddRange(conveniences);
        public void AddConvenience(string convenience) => this.Conveniences.Add(convenience);

        public void SetImages(List<Image> images) => this.Images.AddRange(images);
        public void AddImage(Image image) => this.Images.Add(image);
    }
}
