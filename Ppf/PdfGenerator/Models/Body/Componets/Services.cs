

namespace PdfGenerator.Models.Body.Componets
{
    internal class Services : BodyElemment
    {
        public string Name { get; set; }
        public string Description { get; set; }
        private Services()
        {
        }

        public static Services CreateAccommodation()
        {
            return new Services
            {
                TitleBody = ".HOSPEDAGEM",
                
            };
        }
    }
}
