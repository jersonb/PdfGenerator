namespace PdfGenerator.Models.Body.Componets
{
    public class GenericItem
    {
        private GenericItem() { }

        public static GenericItem Create(string name, string value, string quant, string total)
            => new GenericItem
            {
                Name = name,
                Value = value,
                Quant = quant,
               
                Total = total
            };

        public static GenericItem Create(string name, string value)
            => new GenericItem
            {
                Name = name,
                Value = value
            };

        public string Name { get; private set; }
        public string Value { get; private set; }
        public string Quant { get; private set; }
     
        public string Total { get; private set; }

    }
}
