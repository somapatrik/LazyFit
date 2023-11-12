using LazyFit.Models;
using LazyFit.Models.Drinks;
using LazyFit.Models.Foods;
using SQLite;
using System.Text;

namespace LazyFit.Services
{
    public static class DB
    {
        #region DB settings

        static SQLiteAsyncConnection Database;

        public static string DatabaseFilename = "lazyfit.db3";

        public static SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

        public static async Task InitDB()
        {
            if (Database != null)
                return;

            Database = new SQLiteAsyncConnection(DatabasePath, Flags);

            List<Task> tables = new List<Task>()
            {
                Database.CreateTableAsync<Fast>(),
                Database.CreateTableAsync<Mood>(),
                Database.CreateTableAsync<Weight>(),

                Database.CreateTableAsync<Drink>(),
                Database.CreateTableAsync<DrinkProperty>(),

                Database.CreateTableAsync<Food>(),
                Database.CreateTableAsync<FoodProperty>(),
            };

            Task CreateTables = Task.WhenAll(tables);
            await CreateTables;


            // Default data
            foreach (TypeOfDrink drink in Enum.GetValues(typeof(TypeOfDrink)))
            {
                string description = "";

                if (drink == TypeOfDrink.Coffee)
                    description = "Dark perfection";
                else if (drink == TypeOfDrink.Beer)
                    description = "The beverage of Kings";
                else if (drink == TypeOfDrink.Water)
                    description = "Basic stuff";
                else if (drink == TypeOfDrink.Sweet)
                    description = "Anything with sugar";
                else if (drink == TypeOfDrink.Tea)
                    description = "No sugar!";
                else if (drink == TypeOfDrink.Alcoholic)
                    description = "Beer excluded";

                    DrinkProperty drinkProperty = new DrinkProperty()
                    {
                        DrinkID = drink,
                        DisplayName = drink.ToString(),
                        Description = description,
                        ImageName = drink.ToString() + ".png",
                    };
                await UpdateDrinkProperty(drinkProperty);
            }

            foreach (TypeOfFood food in Enum.GetValues(typeof(TypeOfFood)))
            {
                string description = "";

                if (food == TypeOfFood.Normal)
                    description = "Casual food";
                else if (food == TypeOfFood.Healthy)
                    description = "Like really healthy";
                else if (food == TypeOfFood.Unhealthy)
                    description = "The tasty stuff";
                else if (food == TypeOfFood.Snack)
                    description = "Usually the bad kind";

                FoodProperty foodProperty = new FoodProperty()
                {
                    FoodId = food,
                    DisplayName = food.ToString(),
                    Description = description,
                    ImageName = food.ToString() + ".png"
                };
                await UpdateDrinkProperty(foodProperty);
            }

        }



        #endregion


        public static async Task DeleteItem(object item)
        {
            await Database.DeleteAsync(item); 
        }


        public static async Task<List<TakenAction>> GetLatestActions(DateTime fromTime, DateTime toTime)
        {
            List<TakenAction> actions = new List<TakenAction>();


            var foods = await GetFoods(fromTime, toTime);
            var drinks = await GetDrinks(fromTime, toTime);
            var moods = await GetMoods(fromTime, toTime);
            var weights = await GetWeights(fromTime, toTime);

            List<FoodProperty> foodProperties = await GetFoodProperties();
            List<DrinkProperty> drinkProperties = await GetDrinkProperties();

            foods.ForEach(food =>
            {
                FoodProperty property = foodProperties.FirstOrDefault(f => f.FoodId == food.TypeOfFood);

                TakenAction action = new TakenAction()
                {
                    Id = food.Id,
                    Date = food.Time,
                    SubjectText = property.DisplayName,
                    AdditionalText = "",
                    Type = food.GetType().Name,
                    ClassObject = food,
                };
                actions.Add(action);

            });

            drinks.ForEach(drink =>
            {
                DrinkProperty property = drinkProperties.FirstOrDefault(d => d.DrinkID == drink.TypeOfDrink);

                TakenAction action = new TakenAction()
                {
                    Id = drink.Id,
                    Date = drink.Time,
                    SubjectText = property.DisplayName,
                    AdditionalText = "",
                    Type = drink.GetType().Name,
                    ClassObject = drink,
                };
                actions.Add(action);

            });

            moods.ForEach(mood =>
            {
                TakenAction action = new TakenAction()
                {
                    Id = mood.Id,
                    Date = mood.Time,
                    SubjectText = Enum.GetName(mood.TypeOfMood),
                    AdditionalText = "",
                    Type = mood.GetType().Name,
                    ClassObject = mood,
                };
                actions.Add(action);
            });

            weights.ForEach(weight =>
            {
                TakenAction action = new TakenAction()
                {
                    Id = weight.Id,
                    Date = weight.Time,
                    SubjectText = weight.WeightValue.ToString(),
                    AdditionalText = "",
                    Type = weight.GetType().Name,
                    ClassObject = weight,
                };
                actions.Add(action);
            });

            return actions.OrderByDescending(a=>a.Date).ToList();

        }

        #region Weight

        public static async Task<Weight> GetLastWeightOlderThan(DateTime beforeDate)
        {
            return await Database.Table<Weight>().Where(x => x.Time < beforeDate).OrderByDescending(d => d.Time).FirstOrDefaultAsync();
        }

        public static async Task<List<Weight>> GetWeightByPagePerWeek(int pageNumber = 0)
        {
            DateTime today = DateTime.Today.AddDays(7 * pageNumber);
            int dayofWeek = today.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)today.DayOfWeek;

            DateTime monday = today.AddDays(-(dayofWeek - 1));
            DateTime sunday = monday.AddDays(7).AddMinutes(-1);

            return await Database.Table<Weight>().Where(w => w.Time >= monday && w.Time <= sunday).ToListAsync();
        }

        public static async Task<List<Weight>> GetWeights(DateTime fromDate, DateTime toDate)
        {
            return await Database.Table<Weight>().Where(w => w.Time >= fromDate && w.Time <= toDate).ToListAsync();
        }

        public static async Task InsertWeight(Weight weight)
        {
            await Database.InsertAsync(weight);
        }

        public static async Task UpdateWeight(Weight weight)
        {
            await Database.UpdateAsync(weight);
        }

        public static async Task DeleteWeight(Weight weight)
        {
            await Database.DeleteAsync(weight);
        }

        public static async Task<Weight> GetWeight(Guid id)
        {
            return await Database.Table<Weight>().FirstOrDefaultAsync(w => w.Id == id);
        }
        public static async Task<List<Weight>> GetWeightFromTime(DateTime from, DateTime to)
        {
            return await Database.Table<Weight>().Where(w => w.Time >= from && w.Time <= to).ToListAsync();
        }

        public static async Task<Weight> GetLastWeight()
        {
            return await Database.Table<Weight>().OrderByDescending(w=>w.Time).FirstOrDefaultAsync();
        }

        public static async Task<List<Weight>> GetLastWeights(int numberOfWeights)
        {
            var weights = await Database.Table<Weight>().OrderByDescending(w => w.Time).Take(numberOfWeights).ToListAsync();

            return weights.OrderBy(x => x.Time).ToList();
        }

        public static async Task<List<Weight>> GetWeightPage(int pageNumber, int numberOfWeight)
        {
            int offset = pageNumber * numberOfWeight;

            StringBuilder query = new StringBuilder();
            query.AppendLine("SELECT * FROM Weight ORDER BY Time DESC");
            query.AppendLine($"LIMIT {numberOfWeight} OFFSET {offset}");
            var weights = await Database.QueryAsync<Weight>(query.ToString());

            return weights;
        }

        #endregion

        #region Food

        public static async Task<List<FoodProperty>> GetFoodProperties()
        {
            return await Database.Table<FoodProperty>().ToListAsync();
        }

        public static async Task UpdateDrinkProperty(FoodProperty foodProperty)
        {
            await Database.InsertOrReplaceAsync(foodProperty);
        }

        public static async Task InsertFood(Food food)
        {
            await Database.InsertAsync(food);
        }

        public static async Task<List<Food>> GetFoods(DateTime fromTime, DateTime toTime)
        {
            return await Database.Table<Food>().Where(f => f.Time >= fromTime && f.Time <= toTime).ToListAsync();
        }

        #endregion  

        #region Drink

        public static async Task UpdateDrinkProperty(DrinkProperty drinkProperty)
        {
            await Database.InsertOrReplaceAsync(drinkProperty);
        }

        public static async Task<List<DrinkProperty>> GetDrinkProperties()
        {
            return await Database.Table<DrinkProperty>().ToListAsync();
        }

        public static async Task InsertDrink(Drink drink)
        {
            await Database.InsertAsync(drink);

        }

        public static async Task<List<Drink>> GetDrinks(DateTime fromTime, DateTime toTime)
        {
            return await Database.Table<Drink>().Where(d=>d.Time >= fromTime && d.Time <= toTime).ToListAsync();
        }
        #endregion

        #region Mood

        public static async Task InsertMood(Mood mood)
        {
            await Database.InsertAsync(mood);
        }

        public static async Task UpdateMood(Mood mood)
        {
            await Database.UpdateAsync(mood);
        }

        public static async Task DeleteMood(Mood mood)
        {
            await Database.DeleteAsync(mood);
        }

        public static async Task<List<Mood>> GetAllMoods()
        {
           return await Database.Table<Mood>().ToListAsync();
        }

        public static async Task<List<Mood>> GetMoods(DateTime fromTime, DateTime toTime)
        {
            return await Database.Table<Mood>().Where(m=> m.Time >= fromTime && m.Time <= toTime).ToListAsync();
        }

        #endregion

        #region Fast

        public static async Task<List<Fast>> GetFastHistory()
        {
            return await Database.Table<Fast>().Where(f=> f.EndTime != null).OrderByDescending(x=>x.StartTime).ToListAsync();
        }

        public static async Task<List<Fast>> GetFastsByPage(int pageNumber = 0)
        {
            DateTime displayTime = DateTime.Today.AddMonths(pageNumber);
            DateTime from = new DateTime(displayTime.Year, displayTime.Month, 1);
            DateTime to = from.AddMonths(1).AddDays(-1);

            var f = await Database.Table<Fast>().Where(f=>f.EndTime != null && f.StartTime >= from && f.StartTime<=to).ToListAsync();
            return f;

        }

        public static async Task<List<Fast>> GetFastsByPagePerWeek(int pageNumber = 0)
        {
            DateTime today = DateTime.Today.AddDays(7 * pageNumber);
            int dayofWeek = today.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)today.DayOfWeek ;

            DateTime monday = today.AddDays(-(dayofWeek - 1));
            DateTime sunday = monday.AddDays(7).AddMinutes(-1);

            var f = await Database.Table<Fast>().Where(f => f.EndTime != null && f.StartTime >= monday && f.StartTime <= sunday).ToListAsync();
            return f;
        }

        public static async Task<List<Fast>> GetFasts(DateTime fromDate, DateTime toDate)
        {
            return await Database.Table<Fast>().Where(f => f.EndTime != null && f.StartTime >= fromDate && f.StartTime <= toDate).ToListAsync();
        }

        public static async Task<Fast> GetRunningFast()
        {
            return await Database.Table<Fast>().FirstOrDefaultAsync(f => f.EndTime == null);
        }

        public static async Task InsertFast(Fast fast)
        {
            await Database.InsertAsync(fast);
        }

        public static async Task UpdateFast(Fast fast)
        {
            await Database.UpdateAsync(fast);
        }

        #endregion

    }
}
