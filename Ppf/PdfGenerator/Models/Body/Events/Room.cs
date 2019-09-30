using iTextSharp.text;
using System.Collections.Generic;

namespace PdfGenerator.Models.Body.Evento
{
    public class Room
    {
        private Room()
        {
            Schedules = new List<Agenda>();
            Images = new List<Image>();
        }

        public static Room Create(int order, string name, string format, string area, string highCeiling, int quantGuest, string price)
        {
            return new Room
            {
                Order = order,
                Name = name,
                Format = format,
                Area = area,
                HighCeiling = highCeiling,
                QuantGuest = quantGuest,
                Price = price,
            };
        }

        public int Order { get; private set; }
        public string Name { get; private set; }
        public string Format { get; private set; }
        public string Area { get; private set; }
        public string HighCeiling { get; private set; }
        public int QuantGuest { get; private  set; }
        public string Price { get; private set; }
        public List<Agenda> Schedules { get; private set; }
        public List<Image> Images { get; private set; }

        public void AddSchedule(Agenda schedule)
            => this.Schedules.Add(schedule);

        public void AddImage(Image image)
            => this.Images.Add(image);

    }
}