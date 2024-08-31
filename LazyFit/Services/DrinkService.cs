using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Messages;
using LazyFit.Models.Drinks;
using LazyFit.Models.Foods;

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

        public static async Task<List<Drink>> GetDrinks(DateTime fromTime, DateTime toTime, bool LoadProperties = false)
        {
            var drinks = await DB.Database.Table<Drink>().Where(d => d.Time >= fromTime && d.Time <= toTime).ToListAsync();

            if (!LoadProperties)
                return drinks;

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

            return GetGoodDrinkRationFromList(drinks);
        }

        public static int GetGoodDrinkRationFromList(List<Drink> drinks)
        {
            double drinksCount = drinks.Count();

            if (drinksCount == 0)
                return 0;

            int goodDrinkCount = drinks.Where(d => d.TypeOfDrink == TypeOfDrink.Water || d.TypeOfDrink == TypeOfDrink.Tea).Count();
            return (int)Math.Round((goodDrinkCount / drinksCount) * 100, 0);
        }

        public static async Task<List<Drink>> GetDrinksFromLastDays(int numberOfDays)
        {
            DateTime now = DateTime.Today;

            DateTime from = new DateTime(now.AddDays(-numberOfDays).Date.Ticks);
            DateTime to = new DateTime(now.Ticks).AddDays(1).AddSeconds(-1);

            return await GetDrinks(from, to, true);

        }

        public static async Task CreateDrink(Drink drink)
        {
            await DB.Database.InsertAsync(drink);
            WeakReferenceMessenger.Default.Send(new DrinkNewMessage(drink));
        }

        public static async Task DeleteDrink(Drink drink)
        {
            await DB.Database.DeleteAsync(drink);
            WeakReferenceMessenger.Default.Send(new DrinkDeleteMessage(drink));
        }

    }
}
