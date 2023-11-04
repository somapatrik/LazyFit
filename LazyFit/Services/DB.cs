using LazyFit.Models;
using LazyFit.Models.Drinks;
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
                
                Database.CreateTableAsync<Food>(),
                Database.CreateTableAsync<Weight>(),

                Database.CreateTableAsync<Drink>(),
                Database.CreateTableAsync<DrinkProperty>()
            };

            Task CreateTables = Task.WhenAll(tables);
            await CreateTables;


            // Default data
            foreach (TypeOfDrink drink in Enum.GetValues(typeof(TypeOfDrink)))
            {
                DrinkProperty drinkProperty = new DrinkProperty()
                {
                    DrinkID = drink,
                    DisplayName = drink.ToString(),
                    ImageName = drink.ToString() + ".png",
                };
                await UpdateDrinkProperty(drinkProperty);
            }
            
        }



        #endregion

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
