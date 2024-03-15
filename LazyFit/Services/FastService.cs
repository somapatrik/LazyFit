using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Messages;
using LazyFit.Models;

namespace LazyFit.Services
{
    public static class FastService
    {

        public static async Task<List<Fast>> GetLastFasts(int numberOfFasts)
        {
            return await DB.Database.Table<Fast>().Where(f=>f.EndTime != null).ThenByDescending(f=>f.StartTime).ToListAsync();
        } 
        public static async Task<bool> FastsExists(int numberOfFasts)
        {
            var fasts = await GetLastFasts(numberOfFasts);
            return fasts.Any();
        }
        public static async Task<int> GetFastFinishRatio(int numberOfFasts)
        {
            var fasts = await DB.Database.Table<Fast>()
                .Where(f => f.EndTime != null)
                .OrderByDescending(w => w.EndTime)
                .Take(numberOfFasts).ToListAsync();

            decimal fastCount = fasts.Count();
            int finishedCount = fasts.Where(f => f.Completed).Count();

            if (fastCount > 0)
                return (int)Math.Round((finishedCount / fastCount) * 100, 0);

            return 0;
        }
        public static async Task<Fast> GetFast(Guid fastId)
        {
            return await DB.Database.Table<Fast>().FirstOrDefaultAsync(f => f.Id == fastId);
        }
        public static async Task<List<Fast>> GetFastHistory()
        {
            return await DB.Database.Table<Fast>().Where(f => f.EndTime != null).OrderByDescending(x => x.StartTime).ToListAsync();
        }
        public static async Task<List<Fast>> GetFastsByPage(int pageNumber = 0)
        {
            DateTime displayTime = DateTime.Today.AddMonths(pageNumber);
            DateTime from = new DateTime(displayTime.Year, displayTime.Month, 1);
            DateTime to = from.AddMonths(1).AddDays(-1);

            var f = await DB.Database.Table<Fast>().Where(f => f.EndTime != null && f.StartTime >= from && f.StartTime <= to).ToListAsync();
            return f;

        }
        public static async Task<List<Fast>> GetFastsByPagePerWeek(int pageNumber = 0)
        {
            DateTime today = DateTime.Today.AddDays(7 * pageNumber);
            int dayofWeek = today.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)today.DayOfWeek;

            DateTime monday = today.AddDays(-(dayofWeek - 1));
            DateTime sunday = monday.AddDays(7).AddMinutes(-1);

            var f = await DB.Database.Table<Fast>().Where(f => f.EndTime != null && f.StartTime >= monday && f.StartTime <= sunday).ToListAsync();
            return f;
        }
        public static async Task<List<Fast>> GetFasts(DateTime fromDate, DateTime toDate)
        {
            return await DB.Database.Table<Fast>().Where(f => f.EndTime != null && f.StartTime >= fromDate && f.StartTime <= toDate).ToListAsync();
        }
        public static async Task<Fast> GetRunningFast()
        {
            return await DB.Database.Table<Fast>().FirstOrDefaultAsync(f => f.EndTime == null);
        }
        public static async Task InsertFast(Fast fast)
        {
            await DB.Database.InsertAsync(fast);
            WeakReferenceMessenger.Default.Send(new FastStartMessage(fast));
        }
        public static async Task UpdateFast(Fast fast)
        {
            await DB.Database.UpdateAsync(fast);
        }
        public static async Task EndFast(Fast fast)
        {
            fast.End();
            await DB.Database.UpdateAsync(fast);
            WeakReferenceMessenger.Default.Send(new FastEndMessage(fast));
        }
        public static async Task StartFast(int hours)
        {
            Fast fast = new Fast(Guid.NewGuid());
            fast.SetHours(hours);
            await DB.Database.InsertAsync(fast);
            WeakReferenceMessenger.Default.Send(new FastStartMessage(fast));
        }
    }
}
