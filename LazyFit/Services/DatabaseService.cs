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

        public async Task DeleteItem(object item)
        {
            await Database.DeleteAsync(item); 
        }
    }
}
