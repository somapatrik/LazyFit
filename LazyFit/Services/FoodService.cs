using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Messages;
using LazyFit.Models.Foods;

namespace LazyFit.Services
{
    public static class FoodService
    {
        public static async Task CreateFood(Food food)
        {
            await DB.Database.InsertAsync(food);
            WeakReferenceMessenger.Default.Send(new FoodNewMessage(food));
        }

        public static async Task DeleteFood(Food food)
        {
            await DB.Database.DeleteAsync(food);
            WeakReferenceMessenger.Default.Send(new FoodDeleteMessage(food));
        }

        public static async Task<List<FoodProperty>> GetFoodProperties()
        {
            return await DB.Database.Table<FoodProperty>().ToListAsync();
        }

        public static async Task UpdateDrinkProperty(FoodProperty foodProperty)
        {
            await DB.Database.InsertOrReplaceAsync(foodProperty);
        }

        public static async Task InsertFood(Food food)
        {
            await DB.Database.InsertAsync(food);
        }

        public static async Task<List<Food>> GetFoods(DateTime fromTime, DateTime toTime, bool LoadProperties = false)
        {
            var foods = await DB.Database.Table<Food>().Where(f => f.Time >= fromTime && f.Time <= toTime).ToListAsync();

            if (!LoadProperties)
                return foods;

            var foodProperties = await GetFoodProperties();

            foods.ForEach(f => f.Property = foodProperties.FirstOrDefault(fp => fp.FoodId == f.TypeOfFood));
            return foods;
        }

        public static async Task<List<Food>> GetLastFoods(int numberOfFoods)
        {
            var foodProperties = await GetFoodProperties();
            var foods = await DB.Database.Table<Food>().OrderByDescending(f => f.Time).Take(numberOfFoods).ToListAsync();
            foods.ForEach(f => f.Property = foodProperties.FirstOrDefault(fp => fp.FoodId == f.TypeOfFood));
            return foods;
        }

        public static async Task<int> GetGoodFoodRatio(int numberOfFoods)
        {
            List<Food> foods = await GetLastFoods(numberOfFoods);
            return GetFoodRatioFromList(foods);
        }

        public static int GetFoodRatioFromList(List<Food> foodList)
        {
            double foodCount = foodList.Count();

            if (foodCount == 0)
                return 0;

            int goodCount = foodList.Where(f => f.TypeOfFood == TypeOfFood.Normal || f.TypeOfFood == TypeOfFood.Healthy).Count();
            int goodRatio = (int)Math.Round((goodCount / foodCount) * 100, 0);
            return goodRatio;
        }

        public static async Task<List<Food>> GetFoodsFromLastDays(int numberOfDays)
        {
            DateTime now = DateTime.Today;
             
            DateTime from = new DateTime(now.AddDays(-numberOfDays).Date.Ticks);
            DateTime to = new DateTime(now.Ticks).AddDays(1).AddSeconds(-1);
            
           return await GetFoods(from, to, true);

        }
        
    }
}
