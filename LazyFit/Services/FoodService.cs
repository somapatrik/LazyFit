using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Messages;
using LazyFit.Models.Foods;

namespace LazyFit.Services
{
    public  class FoodService
    {
        DB Connection;

        public FoodService() 
        {
            Connection = new DB();
        }

        public  async Task CreateFood(Food food)
        {
            await Connection.Database.InsertAsync(food);
            WeakReferenceMessenger.Default.Send(new FoodNewMessage(food));
        }

        public  async Task DeleteFood(Food food)
        {
            await Connection.Database.DeleteAsync(food);
            WeakReferenceMessenger.Default.Send(new FoodDeleteMessage(food));
        }

        public  async Task<List<FoodProperty>> GetFoodProperties()
        {
            return await Connection.Database.Table<FoodProperty>().ToListAsync();
        }

        public  async Task UpdateDrinkProperty(FoodProperty foodProperty)
        {
            await Connection.Database.InsertOrReplaceAsync(foodProperty);
        }

        public  async Task InsertFood(Food food)
        {
            await Connection.Database.InsertAsync(food);
        }

        public  async Task<List<Food>> GetFoods(DateTime fromTime, DateTime toTime, bool LoadProperties = false)
        {
            var foods = await Connection.Database.Table<Food>().Where(f => f.Time >= fromTime && f.Time <= toTime).ToListAsync();

            if (!LoadProperties)
                return foods;

            var foodProperties = await GetFoodProperties();

            foods.ForEach(f => f.Property = foodProperties.FirstOrDefault(fp => fp.FoodId == f.TypeOfFood));
            return foods;
        }

        public  async Task<List<Food>> GetLastFoods(int numberOfFoods)
        {
            var foodProperties = await GetFoodProperties();
            var foods = await Connection.Database.Table<Food>().OrderByDescending(f => f.Time).Take(numberOfFoods).ToListAsync();
            foods.ForEach(f => f.Property = foodProperties.FirstOrDefault(fp => fp.FoodId == f.TypeOfFood));
            return foods;
        }

        public  async Task<int> GetGoodFoodRatio(int numberOfFoods)
        {
            List<Food> foods = await GetLastFoods(numberOfFoods);
            return GetFoodRatioFromList(foods);
        }

        public  int GetFoodRatioFromList(List<Food> foodList)
        {
            double foodCount = foodList.Count();

            if (foodCount == 0)
                return 0;

            int goodCount = foodList.Where(f => f.TypeOfFood == TypeOfFood.Normal || f.TypeOfFood == TypeOfFood.Healthy).Count();
            int goodRatio = (int)Math.Round((goodCount / foodCount) * 100, 0);
            return goodRatio;
        }

        public  async Task<List<Food>> GetFoodsFromLastDays(int numberOfDays)
        {
            DateTime now = DateTime.Today;
             
            DateTime from = new DateTime(now.AddDays(-numberOfDays).Date.Ticks);
            DateTime to = new DateTime(now.Ticks).AddDays(1).AddSeconds(-1);
            
           return await GetFoods(from, to, true);

        }
        
    }
}
