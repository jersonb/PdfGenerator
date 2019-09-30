namespace PdfGenerator.Models.Body.Evento
{
    public class Agenda
    {
        private Agenda()
        {
        }

        public static Agenda Create(string beginning, string end)
        {
            return new Agenda
            {
                Beginning = beginning,
                End = end
            };
        }

        public string Beginning { get; private set; }
        public string End { get; private set; }

    }
}