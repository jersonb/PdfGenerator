using PdfGenerator.Models.Body.Componets;
using PdfGenerator.Models.Body.Events.Food;
using System.Collections.Generic;

namespace PdfGenerator.Models.Body.Evento
{
    public class EventDetail
    {
        private EventDetail()
        {
            this.Days = new List<DaysEvent>();
            this.WhaterAndCoffe = new List<GenericItem>();
            this.Equipments = new List<GenericItem>();
        }

        public static EventDetail Create(string value, Meal breakFast, Meal coffeBreak1, Meal lunch, Meal coffeBreak2, Meal dinner, Meal cocktail)
             => new EventDetail
             {
                 Value = value,
                 BreakFast = breakFast,
                 CoffeBreak1 = coffeBreak1,
                 Lunch = lunch,
                 CoffeBreak2 = coffeBreak2,
                 Dinner = dinner,
                 Cocktail = cocktail,
             };

        public void AddCustomMeal(Meal customFoods, Meal customDrinks)
        {
            CustomFoods = customFoods;
            CustomDrinks = customDrinks;
        }


        public string Value { get; private set; }
        public List<DaysEvent> Days { get; private set; }
        public Meal BreakFast { get; private set; }
        public Meal CoffeBreak1 { get; private set; }
        public Meal Lunch { get; private set; }
        public Meal CoffeBreak2 { get; private set; }
        public Meal Dinner { get; private set; }
        public Meal Cocktail { get; private set; }
        public Meal CustomFoods { get; private set; }
        public Meal CustomDrinks { get; private set; }

        public List<GenericItem> WhaterAndCoffe { get; private set; }
        public List<GenericItem> Equipments { get; private set; }

        public void AddDayEvent(DaysEvent daysEvent)
            => this.Days.Add(daysEvent);

        public void AddWhaterAndCoffe(GenericItem whaterAndCoffe)
            => this.WhaterAndCoffe.Add(whaterAndCoffe);

        public void AddEquipments(GenericItem others)
            => this.Equipments.Add(others);


    }
}
