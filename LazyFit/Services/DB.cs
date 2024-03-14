using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Messages;
using LazyFit.Models;
using LazyFit.Models.Drinks;
using LazyFit.Models.Foods;
using LazyFit.Models.Pressure;
using LazyFit.Models.WeightModels;
using SQLite;
using System.Text;

namespace LazyFit.Services
{
    public static class DB
    {
        #region DB settings

        public static SQLiteAsyncConnection Database;

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

                Database.CreateTableAsync<BloodPressure>()
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
                await Database.InsertOrReplaceAsync(drinkProperty);
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
                await Database.InsertOrReplaceAsync(foodProperty);
            }

        }

        #endregion

        public static async Task DeleteItem(object item)
        {
            await Database.DeleteAsync(item); 
        }

        #region actions
        public static async Task<List<ActionSquare>> GetActionSquares(DateTime fromTime, DateTime toTime)
        {
            var foods = await FoodService.GetFoods(fromTime, toTime);
            var drinks = await DrinkService.GetDrinks(fromTime, toTime);
            var moods = await GetMoods(fromTime, toTime);
            var weights = await WeightService.GetWeights(fromTime, toTime);
            var fasts = await GetFasts(fromTime, toTime);

            List<ActionSquare> actionSquares = new List<ActionSquare>();

            foods.ForEach(food =>
            {
                actionSquares.Add(new ActionSquare() 
                { 
                    ActionName = nameof(Food),
                    Color = LazyColors.FreshGreen, 
                    Time = food.Time, 
                    IsBad = (food.TypeOfFood == TypeOfFood.Unhealthy || food.TypeOfFood == TypeOfFood.Snack) });
            });

            drinks.ForEach(drink =>
            {
                actionSquares.Add(new ActionSquare() 
                {
                    ActionName = nameof(Drink),
                    Color = LazyColors.WaterBlue, 
                    Time = drink.Time, 
                    IsBad = (drink.TypeOfDrink != TypeOfDrink.Water && drink.TypeOfDrink != TypeOfDrink.Tea) });
            });

            moods.ForEach(mood =>
            {
                actionSquares.Add(new ActionSquare()
                {
                    ActionName = nameof(Mood),
                    Color = Colors.DarkBlue.ToHex(),
                    Time = mood.Time,
                    IsBad = (mood.TypeOfMood == MoodName.Bad)
                });
            });

            fasts.ForEach(fast =>
            {
                actionSquares.Add(new ActionSquare()
                {
                    ActionName = nameof(Fast),
                    Color = LazyColors.LazyColor,
                    Time = (DateTime)fast.EndTime,
                    IsBad = (!fast.Completed)
                });
            });


            weights.ForEach(weight =>
            {
                actionSquares.Add(new ActionSquare()
                {
                    ActionName = nameof(Weight),
                    Color = Colors.DarkOrange.ToHex(),
                    Time = weight.Time,
                    IsBad = false
                });
            });


            return actionSquares.OrderBy(a => a.Time).ToList();

        }

        public static async Task<List<TakenAction>> GetLatestActions(DateTime fromTime, DateTime toTime)
        {
            List<TakenAction> actions = new List<TakenAction>();


            var foods = await FoodService.GetFoods(fromTime, toTime);
            var drinks = await DrinkService.GetDrinks(fromTime, toTime);
            var moods = await GetMoods(fromTime, toTime);
            var weights = await WeightService.GetWeights(fromTime, toTime);
            var fasts = await GetFasts(fromTime, toTime);
            //var pressures = await GetPressures(fromTime, toTime);

            List<FoodProperty> foodProperties = await FoodService.GetFoodProperties();
            List<DrinkProperty> drinkProperties = await DrinkService.GetDrinkProperties();

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

            fasts.ForEach(fast =>
            {
                TakenAction action = new TakenAction()
                {
                    Id = fast.Id,
                    Date = (DateTime)fast.EndTime,
                    SubjectText = fast.Completed ? "Completed fast" : "Failed fast",
                    AdditionalText = "",
                    Type = fast.GetType().Name,
                    ClassObject = fast,
                };
                actions.Add(action);
            });

            return actions.OrderByDescending(a=>a.Date).ToList();

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

        public static async Task<Fast> GetFast(Guid fastId)
        {
            return await Database.Table<Fast>().FirstOrDefaultAsync(f=>f.Id == fastId);
        }

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
            WeakReferenceMessenger.Default.Send(new StartFastMessage(fast));
        }

        public static async Task UpdateFast(Fast fast)
        {
            await Database.UpdateAsync(fast);
        }

        #endregion

        #region Blood pressure

        public static async Task InsertPressure(BloodPressure pressure)
        {
            await Database.InsertAsync(pressure);
        }

        public static async Task<List<BloodPressure>> GetPressures(DateTime fromDate, DateTime toDate)
        {
            return await Database.Table<BloodPressure>().Where(p => p.Time >= fromDate && p.Time <= toDate).OrderByDescending(bp=>bp.Time).ToListAsync();
        }

        public static async Task<BloodPressure> GetPressure(Guid id)
        {
            return await Database.Table<BloodPressure>().FirstOrDefaultAsync(p => p.Id == id);
        }

        public static async Task<List<BloodPressure>> GetLastPressures(int count)
        {
            return await Database.Table<BloodPressure>().OrderByDescending(p=>p.Time).Take(count).ToListAsync();
        }

        #endregion

    }
}
