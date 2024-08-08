using LazyFit.Models;
using LazyFit.Models.Drinks;
using LazyFit.Models.Foods;
using LazyFit.Models.Moods;
using LazyFit.Models.Pressure;
using LazyFit.Models.WeightModels;
using SQLite;

namespace LazyFit.Services
{
    public static class DB
    {
        #region DB settings

        public static SQLiteAsyncConnection Database;

        //public static string DatabaseFilename = "lazyfit.db3";
        public static string DatabaseFilename = "lazyfit2.db3";


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
                Database.CreateTableAsync<Weight>(),

                Database.CreateTableAsync<Drink>(),
                Database.CreateTableAsync<DrinkProperty>(),

                Database.CreateTableAsync<Food>(),
                Database.CreateTableAsync<FoodProperty>(),

                Database.CreateTableAsync<Mood>(),
                Database.CreateTableAsync<MoodProperty>(),

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

            foreach (MoodName moodName in Enum.GetValues(typeof(MoodName)))
            {
                string name = "";

                if (moodName == MoodName.VeryBad)
                    name = "Very bad";
                else if (moodName == MoodName.Bad)
                    name = "Bad";
                else if (moodName == MoodName.Normal)
                    name = "Normal";
                else if (moodName == MoodName.Good)
                    name = "Good";
                else if (moodName == MoodName.VeryGood)
                    name = "Very good";

                MoodProperty moodProperty = new MoodProperty()
                {
                    MoodID = moodName,
                    DisplayName = name,
                    Description = "",
                    ImageName = moodName.ToString() + ".png",
                };
                await Database.InsertOrReplaceAsync(moodProperty);
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
            var moods = await MoodService.GetMoods(fromTime, toTime);
            var weights = await WeightService.GetWeights(fromTime, toTime);
            var fasts = await FastService.GetFasts(fromTime, toTime);

            List<ActionSquare> actionSquares = new List<ActionSquare>();

            foods.ForEach(food =>
            {
                actionSquares.Add(new ActionSquare() 
                { 
                    ActionObject = food,
                    ActionName = nameof(Food),
                    Color = LazyColors.FreshGreen, 
                    Time = food.Time, 
                    IsBad = (food.TypeOfFood == TypeOfFood.Unhealthy || food.TypeOfFood == TypeOfFood.Snack),
                    ItemName = Enum.GetName(typeof(TypeOfFood), food.TypeOfFood)
                });
            });

            drinks.ForEach(drink =>
            {
                actionSquares.Add(new ActionSquare() 
                {
                    ActionObject = drink,
                    ActionName = nameof(Drink),
                    Color = LazyColors.WaterBlue, 
                    Time = drink.Time, 
                    IsBad = (drink.TypeOfDrink != TypeOfDrink.Water && drink.TypeOfDrink != TypeOfDrink.Tea),
                    ItemName = Enum.GetName(typeof(TypeOfDrink), drink.TypeOfDrink)
                });
            });

            moods.ForEach(mood =>
            {
                actionSquares.Add(new ActionSquare()
                {
                    ActionObject = mood,
                    ActionName = nameof(Mood),
                    Color = Colors.DarkBlue.ToHex(),
                    Time = mood.Time,
                    IsBad = (mood.TypeOfMood == MoodName.Bad),
                    ItemName = Enum.GetName(typeof(MoodName), mood.TypeOfMood)
                });
            });

            fasts.ForEach(fast =>
            {
                actionSquares.Add(new ActionSquare()
                {
                    ActionObject = fast,
                    ActionName = nameof(Fast),
                    Color = LazyColors.LazyColor,
                    Time = (DateTime)fast.EndTime,
                    IsBad = (!fast.Completed),
                    ItemName = fast.Completed ? "Completed fast" : "Failed fast"
                });
            });


            weights.ForEach(weight =>
            {

                actionSquares.Add(new ActionSquare()
                {
                    ActionObject = weight,
                    ActionName = nameof(Weight),
                    Color = Colors.DarkOrange.ToHex(),
                    Time = weight.Time,
                    IsBad = false,
                    ItemName = weight.WeightValue.ToString()
                }) ;
            });


            return actionSquares.OrderBy(a => a.Time).ToList();

        }

        #endregion


    }
}
