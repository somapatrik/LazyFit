using LazyFit.Models;

namespace LazyFit.Services
{
    public static class MoodService
    {
        public static async Task InsertMood(Mood mood)
        {
            await DB.Database.InsertAsync(mood);
        }

        public static async Task UpdateMood(Mood mood)
        {
            await DB.Database.UpdateAsync(mood);
        }

        public static async Task DeleteMood(Mood mood)
        {
            await DB.Database.DeleteAsync(mood);
        }

        public static async Task<List<Mood>> GetAllMoods()
        {
            return await DB.Database.Table<Mood>().ToListAsync();
        }

        public static async Task<List<Mood>> GetMoods(DateTime fromTime, DateTime toTime)
        {
            return await DB.Database.Table<Mood>().Where(m => m.Time >= fromTime && m.Time <= toTime).ToListAsync();
        }
    }
}
