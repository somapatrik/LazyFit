
using LazyFit.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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

            await Database.CreateTableAsync<Fast>();
        }


        #endregion

        public static async Task<List<Fast>> GetFastHistory()
        {
            return await Database.Table<Fast>().Where(f=> f.EndTime != null).OrderByDescending(x=>x.StartTime).ToListAsync();
        }

        public static async Task<Fast> GetRunningFast()
        {
            return await Database.Table<Fast>().Where(f => f.EndTime == null).FirstOrDefaultAsync();
        }

        public static async Task InsertFast(Fast fast)
        {
            await Database.InsertAsync(fast);
        }

        public static async Task UpdateFast(Fast fast)
        {
            await Database.UpdateAsync(fast);
        }



    }
}
