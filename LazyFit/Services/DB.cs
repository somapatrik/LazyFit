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
            // Drink properties

            List<DrinkProperty> drinks = new List<DrinkProperty>()
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

            drinks.ForEach(async d => await Database.InsertOrReplaceAsync(d));

            // Food
            List<FoodProperty> foods = new List<FoodProperty>()
            {
                new FoodProperty()
                {
                    FoodId = TypeOfFood.Normal,
                    DisplayName = "Casual",
                    Description = "Ok food",
                    ImageName = "normalfood.png"
                },
                new FoodProperty()
                {
                    FoodId = TypeOfFood.Healthy,
                    DisplayName = "Healthy",
                    Description = "Like really healthy",
                    ImageName = "healthyfood.png"
                },
                new FoodProperty()
                {
                    FoodId = TypeOfFood.Unhealthy,
                    DisplayName = "Unhealthy",
                    Description = "Junk, tasty stuff",
                    ImageName = "hamburger.png"
                },
                new FoodProperty()
                {
                    FoodId = TypeOfFood.Snack,
                    DisplayName = "Snack",
                    Description = "Quick and bad",
                    ImageName = "snack.png"
                },
            };

            foods.ForEach(async f => await Database.InsertOrReplaceAsync(f));

            // Moods
            List<MoodProperty> moods = new List<MoodProperty>()
            {
                new MoodProperty()
                {
                    MoodID = MoodName.VeryGood,
                    DisplayName = "Very good",
                    Description = "Everything is great!",
                    ImageName = "very_happy.png"
                },
                new MoodProperty()
                {
                    MoodID = MoodName.Good,
                    DisplayName = "Good",
                    Description = "This is fine!",
                    ImageName = "happy.png"
                },
                new MoodProperty()
                {
                    MoodID = MoodName.Normal,
                    DisplayName = "Normal",
                    Description = "Not great, not terrible...",
                    ImageName = "neutral.png"
                },
                new MoodProperty()
                {
                    MoodID = MoodName.Bad,
                    DisplayName = "Bad",
                    Description = "Could be better",
                    ImageName = "angry.png"
                },
                new MoodProperty()
                {
                    MoodID = MoodName.VeryBad,
                    DisplayName = "Very bad",
                    Description = "F*ck this!",
                    ImageName = "cursing.png"
                },
            };

            moods.ForEach(async m=> await Database.InsertOrReplaceAsync(m));

        }

        #endregion

        public static async Task DeleteItem(object item)
        {
            await Database.DeleteAsync(item); 
        }

        #region actions
        public static async Task<List<ActionSquare>> GetActionSquares(DateTime fromTime, DateTime toTime)
        {
            var foods = await FoodService.GetFoods(fromTime, toTime, true);
            var drinks = await DrinkService.GetDrinks(fromTime, toTime, true);
            var moods = await MoodService.GetMoods(fromTime, toTime, true);
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
                    ItemName = Enum.GetName(typeof(TypeOfFood), food.TypeOfFood),
                    IconName = food.Property.ImageName
                    
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
                    ItemName = Enum.GetName(typeof(TypeOfDrink), drink.TypeOfDrink),
                    IconName = drink.Property.ImageName
                });
            });

            moods.ForEach(mood =>
            {

                string moodName = Enum.GetName(typeof(MoodName), mood.TypeOfMood);

                if (mood.TypeOfMood == MoodName.VeryBad)
                {
                    moodName = "Very bad";
                }
                
                if (mood.TypeOfMood == MoodName.VeryGood)
                {
                    moodName = "Very good";
                }
                       
                actionSquares.Add(new ActionSquare()
                {
                    ActionObject = mood,
                    ActionName = nameof(Mood),
                    Color = Colors.DarkBlue.ToHex(),
                    Time = mood.Time,
                    IsBad = mood.TypeOfMood == MoodName.Bad,
                    ItemName = $"{moodName} mood",
                    IconName = mood.Property.ImageName
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
                    ItemName = fast.Completed ? "Completed fast" : "Failed fast",
                    IconName = "fasting.png"
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
                    ItemName = weight.WeightValue.ToString(),
                    IconName = "weight.png"
                }) ;
            });


            return actionSquares.OrderBy(a => a.Time).ToList();

        }

        #endregion


    }
}
