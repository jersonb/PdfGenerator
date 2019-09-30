using System.Collections.Generic;

namespace PdfGenerator.Models.Body.Services
{
    public class Lodging
    {
        public List<Accommodation> Accommodations { get; private set; }

        private Lodging()
        {
            Accommodations = new List<Accommodation>();
        }

        public static Lodging Create()
        {
            return new Lodging
            {
            };
        }

        public void AddAcommodation(Accommodation accommodation) => this.Accommodations.Add(accommodation);

        public void AddAcommodations(List<Accommodation> accommodations) => this.Accommodations.AddRange(accommodations);

    }
}
