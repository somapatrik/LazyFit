using LazyFit.Models;
using LazyFit.Models.Administration;
using LazyFit.Models.Drinks;
using LazyFit.Models.Foods;
using LazyFit.Models.Moods;
using LazyFit.Models.Pressure;
using LazyFit.Models.WeightModels;
using SQLite;

namespace LazyFit.Services
{
    public class DatabaseService
    {
        #region DB settings

        public string DatabaseFilename = "lazyfit2.db3";

        public SQLiteAsyncConnection Database;

        public SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

        public DatabaseService()
        {
            Database = new SQLiteAsyncConnection(DatabasePath, Flags);
        }

        public async Task FullInicialization()
        {
            var InstanceResult = await Database.CreateTableAsync<InstanceInfo>() == CreateTableResult.Created;
            
            var FastResult = await Database.CreateTableAsync<Fast>() == CreateTableResult.Created;
            
            var WeightResult = await Database.CreateTableAsync<Weight>() == CreateTableResult.Created;

            var DrinkResult = await Database.CreateTableAsync<Drink>() == CreateTableResult.Created;
            var DrinkPropertyResult = await Database.CreateTableAsync<DrinkProperty>() == CreateTableResult.Created;

            var FoodResult = await Database.CreateTableAsync<Food>() == CreateTableResult.Created;
            var FoodPropertyResult = await Database.CreateTableAsync<FoodProperty>() == CreateTableResult.Created;

            var MoodResult = await Database.CreateTableAsync<Mood>() == CreateTableResult.Created;
            var MoodPropertyResult = await Database.CreateTableAsync<MoodProperty>() == CreateTableResult.Created;

            var BloodResult = await Database.CreateTableAsync<BloodPressure>() == CreateTableResult.Created;

            await UpdateInstance();
                        
        }

        #endregion

        #region Default data creation

        private async Task UpdateInstance()
        {
            var instance = await Database.Table<InstanceInfo>().FirstOrDefaultAsync();
            if (instance == null)
                instance = new InstanceInfo(GetDeviceType(), "Android", DeviceInfo.Current.DeviceType == DeviceType.Virtual);

            await Database.InsertOrReplaceAsync(instance);
        }

        private string GetDeviceType()
        {
            if (DeviceInfo.Current.Idiom == DeviceIdiom.Desktop)
                return "Desktop";
            else if (DeviceInfo.Current.Idiom == DeviceIdiom.Phone)
                return "Phone";
            else if (DeviceInfo.Current.Idiom == DeviceIdiom.Tablet)
                return "Tablet";
            else
                return "";
        }

        #endregion

        public  async Task<List<ActionSquare>> GetActionSquares(DateTime fromTime, DateTime toTime)
        {
            //var foods = await FoodService.GetFoods(fromTime, toTime, true);
            //var drinks = await DrinkService.GetDrinks(fromTime, toTime, true);
            //var moods = await MoodService.GetMoods(fromTime, toTime, true);
            //var weights = await WeightService.GetWeights(fromTime, toTime);
            //var fasts = await FastService.GetFasts(fromTime, toTime);

            //List<ActionSquare> actionSquares = new List<ActionSquare>();

            //foods.ForEach(food =>
            //{
            //    actionSquares.Add(new ActionSquare() 
            //    { 
            //        ActionObject = food,
            //        ActionName = nameof(Food),
            //        Color = LazyColors.FreshGreen, 
            //        Time = food.Time, 
            //        IsBad = (food.TypeOfFood == TypeOfFood.Unhealthy || food.TypeOfFood == TypeOfFood.Snack),
            //        ItemName = Enum.GetName(typeof(TypeOfFood), food.TypeOfFood),
            //        IconName = food.Property.ImageName

            //    });
            //});

            //drinks.ForEach(drink =>
            //{
            //    actionSquares.Add(new ActionSquare() 
            //    {
            //        ActionObject = drink,
            //        ActionName = nameof(Drink),
            //        Color = LazyColors.WaterBlue, 
            //        Time = drink.Time, 
            //        IsBad = (drink.TypeOfDrink != TypeOfDrink.Water && drink.TypeOfDrink != TypeOfDrink.Tea),
            //        ItemName = Enum.GetName(typeof(TypeOfDrink), drink.TypeOfDrink),
            //        IconName = drink.Property.ImageName
            //    });
            //});

            //moods.ForEach(mood =>
            //{

            //    string moodName = Enum.GetName(typeof(MoodName), mood.TypeOfMood);

            //    if (mood.TypeOfMood == MoodName.VeryBad)
            //    {
            //        moodName = "Very bad";
            //    }

            //    if (mood.TypeOfMood == MoodName.VeryGood)
            //    {
            //        moodName = "Very good";
            //    }

            //    actionSquares.Add(new ActionSquare()
            //    {
            //        ActionObject = mood,
            //        ActionName = nameof(Mood),
            //        Color = Colors.DarkBlue.ToHex(),
            //        Time = mood.Time,
            //        IsBad = mood.TypeOfMood == MoodName.Bad,
            //        ItemName = $"{moodName} mood",
            //        IconName = mood.Property.ImageName
            //    });
            //});

            //fasts.ForEach(fast =>
            //{
            //    actionSquares.Add(new ActionSquare()
            //    {
            //        ActionObject = fast,
            //        ActionName = nameof(Fast),
            //        Color = LazyColors.LazyColor,
            //        Time = (DateTime)fast.EndTime,
            //        IsBad = (!fast.Completed),
            //        ItemName = fast.Completed ? "Completed fast" : "Failed fast",
            //        IconName = "fasting.png"
            //    });
            //});


            //weights.ForEach(weight =>
            //{

            //    actionSquares.Add(new ActionSquare()
            //    {
            //        ActionObject = weight,
            //        ActionName = nameof(Weight),
            //        Color = Colors.DarkOrange.ToHex(),
            //        Time = weight.Time,
            //        IsBad = false,
            //        ItemName = weight.WeightValue.ToString(),
            //        IconName = "weight.png"
            //    }) ;
            //});


            //return actionSquares.OrderBy(a => a.Time).ToList();
            return new List<ActionSquare>();
        }


        public async Task DeleteItem(object item)
        {
            await Database.DeleteAsync(item); 
        }
    }
}
