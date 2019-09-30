using iTextSharp.text;
using System.Collections.Generic;

namespace PdfGenerator.Models.Body.Services
{

    public class Accommodation
    {
        private Accommodation() { }

        public Services Service { get; private set; }
        public string Daily { get; private set; }
        public string QuantRoom { get; private set; }

        public List<string> Beds { get; private set; }

        public static Accommodation Create(string daily, string quantRoom,List<string> beds)
            => new Accommodation
            {
                Daily = daily,
                QuantRoom = quantRoom ,
                Beds=beds
            };


        public void SetService(Services service)
            => this.Service = service;

        
    }
}
