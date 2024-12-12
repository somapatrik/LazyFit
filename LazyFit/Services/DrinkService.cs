using CommunityToolkit.Mvvm.Messaging;
using LazyFit.LocalData;
using LazyFit.Messages;
using LazyFit.Models.Drinks;

namespace LazyFit.Services
{
    public class DrinkService
    {

        DatabaseService Connection;
        LocalDrinkPropertyData DrinkPropertyRepository;

        public DrinkService() 
        {
            Connection = new DatabaseService();
            DrinkPropertyRepository = new LocalDrinkPropertyData();
        }

        public async Task UpdateDrinkProperty(DrinkProperty drinkProperty)
        {
            await Connection.Database.InsertOrReplaceAsync(drinkProperty);
        }

        public List<DrinkProperty> GetDrinkProperties()
        {
            return DrinkPropertyRepository.DrinkProperties;
        }

        public async Task<List<Drink>> GetDrinks(DateTime fromTime, DateTime toTime, bool LoadProperties = false)
        {
            var drinks = await Connection.Database.Table<Drink>().Where(d => d.Time >= fromTime && d.Time <= toTime).ToListAsync();

            if (!LoadProperties)
                return drinks;

            var drinkProperties = GetDrinkProperties();

            drinks.ForEach(f => f.Property = drinkProperties.FirstOrDefault(fp => fp.DrinkID == f.TypeOfDrink));
            return drinks;
        }

        public async Task<List<Drink>> GetLastDrinks(int numberOfDrinks)
        {
            var drinks = await Connection.Database.Table<Drink>().OrderByDescending(d=>d.Time).Take(numberOfDrinks).ToListAsync();

            var drinkProperties = GetDrinkProperties();
            
            drinks.ForEach(f => f.Property = drinkProperties.FirstOrDefault(fp => fp.DrinkID == f.TypeOfDrink));
            return drinks;
        }

        public async Task<int> GetGoodDrinkRatio(int numberOfDrinks)
        {
            var drinks = await GetLastDrinks(numberOfDrinks);

            return GetGoodDrinkRationFromList(drinks);
        }

        public int GetGoodDrinkRationFromList(List<Drink> drinks)
        {
            double drinksCount = drinks.Count();

            if (drinksCount == 0)
                return 0;

            int goodDrinkCount = drinks.Where(d => d.TypeOfDrink == TypeOfDrink.Water || d.TypeOfDrink == TypeOfDrink.Tea).Count();
            return (int)Math.Round((goodDrinkCount / drinksCount) * 100, 0);
        }

        public async Task<List<Drink>> GetDrinksFromLastDays(int numberOfDays)
        {
            DateTime now = DateTime.Today;

            DateTime from = new DateTime(now.AddDays(-numberOfDays).Date.Ticks);
            DateTime to = new DateTime(now.Ticks).AddDays(1).AddSeconds(-1);

            return await GetDrinks(from, to, true);

        }

        public async Task CreateDrink(Drink drink)
        {
            await Connection.Database.InsertAsync(drink);
            WeakReferenceMessenger.Default.Send(new DrinkNewMessage(drink));
        }

        public async Task DeleteDrink(Drink drink)
        {
            await Connection.Database.DeleteAsync(drink);
            WeakReferenceMessenger.Default.Send(new DrinkDeleteMessage(drink));
        }

    }
}
