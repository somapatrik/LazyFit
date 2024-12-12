using LazyFit.Models.Drinks;

namespace LazyFit.LocalData
{
    internal class LocalDrinkPropertyData
    {
        private List<DrinkProperty> _drinkProperties;
        public List<DrinkProperty> DrinkProperties => _drinkProperties;

        public LocalDrinkPropertyData()
        {
            _drinkProperties = new List<DrinkProperty>()
            {
                new DrinkProperty()
                {
                    DrinkID = TypeOfDrink.Coffee,
                    DisplayName = "Coffee",
                    Description = "Dark perfection",
                    ImageName = "coffee.png"
                },
                new DrinkProperty()
                {
                    DrinkID = TypeOfDrink.Beer,
                    DisplayName = "Beer",
                    Description = "The beverage of Kings",
                    ImageName = "beer.png"
                },
                new DrinkProperty()
                {
                    DrinkID = TypeOfDrink.Water,
                    DisplayName = "Water",
                    Description = "Classic",
                    ImageName = "water.png"
                },
                new DrinkProperty()
                {
                    DrinkID = TypeOfDrink.Sweet,
                    DisplayName = "Sweet",
                    Description = "ANYTHING with sugar",
                    ImageName = "cola.png"
                },
                new DrinkProperty()
                {
                    DrinkID = TypeOfDrink.Tea,
                    DisplayName = "Tea",
                    Description = "No sugar!",
                    ImageName = "tea.png"
                },
                new DrinkProperty()
                {
                    DrinkID = TypeOfDrink.Alcoholic,
                    DisplayName = "Alcoholic",
                    Description = "Beer excluded",
                    ImageName = "alcohol.png"
                }
            };
        }
    }
}
