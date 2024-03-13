using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Messages;
using LazyFit.Models.Drinks;

namespace LazyFit.Services
{
    public static class DrinkService
    {
        public static async Task UpdateDrinkProperty(DrinkProperty drinkProperty)
        {
            await DB.Database.InsertOrReplaceAsync(drinkProperty);
        }

        public static async Task<List<DrinkProperty>> GetDrinkProperties()
        {
            return await DB.Database.Table<DrinkProperty>().ToListAsync();
        }

        public static async Task InsertDrink(Drink drink)
        {
            await DB.Database.InsertAsync(drink);
            WeakReferenceMessenger.Default.Send(new NewDrinkMessage(drink));
        }

        public static async Task<List<Drink>> GetDrinks(DateTime fromTime, DateTime toTime, bool LoadProperties = false)
        {
            if (!LoadProperties)
                return await DB.Database.Table<Drink>().Where(d => d.Time >= fromTime && d.Time <= toTime).ToListAsync();

            var drinks = await DB.Database.Table<Drink>().Where(f => f.Time >= fromTime && f.Time <= toTime).ToListAsync();
            var drinkProperties = await GetDrinkProperties();

            drinks.ForEach(f => f.Property = drinkProperties.FirstOrDefault(fp => fp.DrinkID == f.TypeOfDrink));
            return drinks;
        }

        public static async Task<List<Drink>> GetLastDrinks(int numberOfDrinks)
        {
            var drinks = await DB.Database.Table<Drink>().OrderByDescending(d=>d.Time).Take(numberOfDrinks).ToListAsync();
            var drinkProperties = await GetDrinkProperties();
            drinks.ForEach(f => f.Property = drinkProperties.FirstOrDefault(fp => fp.DrinkID == f.TypeOfDrink));
            return drinks;
        }

        public static async Task<int> GetGoodDrinkRatio(int numberOfDrinks)
        {
            var drinks = await GetLastDrinks(numberOfDrinks);
            double drinksCount = drinks.Count();
            int goodDrinkCount= drinks.Where(d=>d.TypeOfDrink == TypeOfDrink.Water || d.TypeOfDrink == TypeOfDrink.Tea).Count();

            return (int)Math.Round((goodDrinkCount / drinksCount)*100, 0);

        }
    }
}
