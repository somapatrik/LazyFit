using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Models.Foods;
using LazyFit.Models.WeightModels;

namespace LazyFit.Services
{
    public static class FoodService
    {
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
            WeakReferenceMessenger.Default.Send(new Messages.NewFoodMessage(food));
        }

        public static async Task<List<Food>> GetFoods(DateTime fromTime, DateTime toTime, bool LoadProperties = false)
        {
            if (!LoadProperties)
                return await DB.Database.Table<Food>().Where(f => f.Time >= fromTime && f.Time <= toTime).ToListAsync();

            var foods = await DB.Database.Table<Food>().Where(f => f.Time >= fromTime && f.Time <= toTime).ToListAsync();
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
            double foodCount = foods.Count();

            if (foodCount == 0)
                return 0;

            int goodCount = foods.Where(f=>f.TypeOfFood == TypeOfFood.Normal || f.TypeOfFood == TypeOfFood.Healthy).Count();
            int goodRatio = (int)Math.Round((goodCount / foodCount) * 100,0);
            return goodRatio;
        }
        
    }
}
