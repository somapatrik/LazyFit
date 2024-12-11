using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Messages;
using LazyFit.Models;

namespace LazyFit.Services
{
    public  class FastService
    {
        DB Connection;

        public FastService()
        {
            Connection = new DB();
        }

        public async Task<List<Fast>> GetLastFasts(int numberOfFasts)
        {
            return await Connection.Database.Table<Fast>().Where(f=>f.EndTime != null).ThenByDescending(f=>f.StartTime).ToListAsync();
        } 
        public async Task<bool> FastsExists(int numberOfFasts)
        {
            var fasts = await GetLastFasts(numberOfFasts);
            return fasts.Any();
        }
        public async Task<int> GetFastFinishRatio(int numberOfFasts)
        {
            var fasts = await Connection.Database.Table<Fast>()
                .Where(f => f.EndTime != null)
                .OrderByDescending(w => w.EndTime)
                .Take(numberOfFasts).ToListAsync();

            return GetFastFinishRatioFromList(fasts);   
        }

        public int GetFastFinishRatioFromList(List<Fast> fasts)
        {
            decimal fastCount = fasts.Count();
            int finishedCount = fasts.Where(f => f.Completed).Count();

            if (fastCount > 0)
                return (int)Math.Round((finishedCount / fastCount) * 100, 0);

            return 0;
        }

        public async Task<Fast> GetFast(Guid fastId)
        {
            return await Connection.Database.Table<Fast>().FirstOrDefaultAsync(f => f.Id == fastId);
        }
        public async Task<List<Fast>> GetFastHistory()
        {
            return await Connection.Database.Table<Fast>().Where(f => f.EndTime != null).OrderByDescending(x => x.StartTime).ToListAsync();
        }

        public async Task<List<Fast>> GetFastsByPage(int pageNumber = 0)
        {
            DateTime displayTime = DateTime.Today.AddMonths(pageNumber);
            DateTime from = new DateTime(displayTime.Year, displayTime.Month, 1);
            DateTime to = from.AddMonths(1).AddDays(-1);

            var f = await Connection.Database.Table<Fast>().Where(f => f.EndTime != null && f.StartTime >= from && f.StartTime <= to).ToListAsync();
            return f;

        }

        public async Task<List<Fast>> GetFasts(DateTime fromDate, DateTime toDate)
        {
            return await Connection.Database.Table<Fast>().Where(f => f.EndTime != null && f.StartTime >= fromDate && f.StartTime <= toDate).ToListAsync();
        }
        public async Task<Fast> GetRunningFast()
        {
            return await Connection.Database.Table<Fast>().FirstOrDefaultAsync(f => f.EndTime == null);
        }

        public async Task<List<Fast>> GetFastsFromLastDays(int numberOfDays)
        {
            DateTime now = DateTime.Today;

            DateTime from = new DateTime(now.AddDays(-numberOfDays).Date.Ticks);
            DateTime to = new DateTime(now.Ticks).AddDays(1).AddSeconds(-1);

            return await GetFasts(from, to);

        }

        public async Task UpdateFast(Fast fast)
        {
            await Connection.Database.UpdateAsync(fast);
        }
        
        public async Task EndFast(Fast fast)
        {
            fast.End();
            await Connection.Database.UpdateAsync(fast);
            WeakReferenceMessenger.Default.Send(new FastEndMessage(fast));
        }
        public async Task StartFast(int hours)
        {
            Fast fast = new Fast(Guid.NewGuid());
            fast.SetHours(hours);
            await Connection.Database.InsertAsync(fast);
            WeakReferenceMessenger.Default.Send(new FastStartMessage(fast));
        }

        public async Task DeleteFast(Fast activeFast)
        {
            await Connection.Database.DeleteAsync(activeFast);
            WeakReferenceMessenger.Default.Send(new FastDeleteMessage(activeFast));
        }
    }
}
