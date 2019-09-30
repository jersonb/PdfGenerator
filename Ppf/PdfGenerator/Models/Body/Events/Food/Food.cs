
namespace PdfGenerator.Models.Body.Events.Food
{
    public class Food
    {
        private Food()
        {
        }

        public static Food CreateCustomFood(string name, string description, string quant, string unitValue, string total)
           => new Food
           {
               Name = name,
               Description = description,
               Quant = quant,
               UnitValue = unitValue,
               Total = total
           };


        public static Food CreatePrincipalFood(string name, string description)
            => new Food
            {
                Name = name,
                Description = description
            };

        public static Food CreateSmallFood(string description)
            => new Food
            {
                Name = "",
                Description = description
            };

        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Quant { get; private set; }
        public string UnitValue { get; private set; }
        public string Total { get; private set; }
    }
}
