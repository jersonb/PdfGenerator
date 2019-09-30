using PdfGenerator.Models.Body.Evento;
using System.Collections.Generic;

namespace PdfGenerator.Models.Body.Events.Food
{
    public class Meal
    {
        private Meal()
        {
        }

        public string Name { get; private set; }
        public int Quant { get; private set; }
        public string UnitValue { get; private set; }
        public string Total { get; private set; }
        public string Type { get; private set; }
        public string Drinks { get; private set; }
        public Agenda Schedule { get; private set; }
        public List<Food> Foods { get; private set; }

        public void SetSchedule(Agenda schedule)
              => this.Schedule = schedule;

        public void AddFoods(List<Food> foods)
           => this.Foods = foods;

        public static Meal CreateSmallMeal(string type, string name, int quant, string unitValue, string total, string drinks)
       => new Meal
            {
                Type = type,
                Name = name,
                Quant = quant,
                UnitValue = unitValue,
                Total = total,
                Drinks = drinks,
            };
        
        public static Meal CreatePrincialMeal(string type, string name, int quant, string unitValue, string total)
        => new Meal
            {
                Type = type,
                Name = name,
                Quant = quant,
                UnitValue = unitValue,
                Total = total,
            };
        }
}
