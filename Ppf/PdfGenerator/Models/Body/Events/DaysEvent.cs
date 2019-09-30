using System.Collections.Generic;

namespace PdfGenerator.Models.Body.Evento
{
    public class DaysEvent
    {
        private DaysEvent()
        {
            this.Rooms = new List<Room>();
        }

        public static DaysEvent Create(int order, string date, int quantRoom, Agenda schedule)
           => new DaysEvent
           {
               Order = order,
               Date = date,
               Schedule = schedule,
               QuantPeople = quantRoom
           };

        public int Order { get; private set; }
        public string Date { get; private set; }
        public int QuantPeople { get; private set; }
        public Agenda Schedule { get; private set; }
        public List<Room> Rooms { get; private set; }

        public void AddRoom(Room room)
            => this.Rooms.Add(room);
    }
}