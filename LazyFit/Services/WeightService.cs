using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Models.WeightModels;
using System.Text;

namespace LazyFit.Services
{
    public static class WeightService
    {
        public static async Task InsertWeight(Weight weight)
        {
            await DB.Database.InsertAsync(weight);
            WeakReferenceMessenger.Default.Send(new Messages.WeightRefreshMessage(weight));
        }
        public static async Task<Weight> GetLastWeight()
        {
            return await DB.Database.Table<Weight>().OrderByDescending(w => w.Time).FirstOrDefaultAsync();
        }

        public static async Task<List<Weight>> GetLastWeights(int numberOfWeights)
        {
            return await DB.Database.Table<Weight>().OrderByDescending(w => w.Time).Take(numberOfWeights).ToListAsync();
        }

        public static async Task<int> GetWeightMonthAvg(DateTime dateTime, int numberOfWeights)
        {
            int AvgWeight = 0;

            DateTime from = new DateTime(dateTime.Year, dateTime.Month, 1);
            DateTime to = from.AddMonths(1).AddSeconds(-1);
            var weights = await GetLastWeights(numberOfWeights);

            if (weights.Any())
                AvgWeight = (int)Math.Round(weights.Select(w => w.WeightValue).Average(), 0);

            return AvgWeight;
        }

        public static async Task<Weight> GetLastWeightOlderThan(DateTime beforeDate)
        {
            return await DB.Database.Table<Weight>().Where(x => x.Time < beforeDate).OrderByDescending(d => d.Time).FirstOrDefaultAsync();
        }

        public static async Task<List<Weight>> GetWeightByPagePerWeek(int pageNumber = 0)
        {
            DateTime today = DateTime.Today.AddDays(7 * pageNumber);
            int dayofWeek = today.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)today.DayOfWeek;

            DateTime monday = today.AddDays(-(dayofWeek - 1));
            DateTime sunday = monday.AddDays(7).AddMinutes(-1);

            return await DB.Database.Table<Weight>().Where(w => w.Time >= monday && w.Time <= sunday).ToListAsync();
        }

        public static async Task<List<Weight>> GetWeights(DateTime fromDate, DateTime toDate)
        {
            return await DB.Database.Table<Weight>().Where(w => w.Time >= fromDate && w.Time <= toDate).ToListAsync();
        }

        public static async Task UpdateWeight(Weight weight)
        {
            await DB.Database.UpdateAsync(weight);
        }

        public static async Task DeleteWeight(Weight weight)
        {
            await DB.Database.DeleteAsync(weight);
        }

        public static async Task<Weight> GetWeight(Guid id)
        {
            return await DB.Database.Table<Weight>().FirstOrDefaultAsync(w => w.Id == id);
        }
        public static async Task<List<Weight>> GetWeightFromTime(DateTime from, DateTime to)
        {
            return await DB.Database.Table<Weight>().Where(w => w.Time >= from && w.Time <= to).ToListAsync();
        }

        public static async Task<List<Weight>> GetWeightPage(int pageNumber, int numberOfWeight)
        {
            int offset = pageNumber * numberOfWeight;

            StringBuilder query = new StringBuilder();
            query.AppendLine("SELECT * FROM Weight ORDER BY Time DESC");
            query.AppendLine($"LIMIT {numberOfWeight} OFFSET {offset}");
            var weights = await DB.Database.QueryAsync<Weight>(query.ToString());

            return weights;
        }
    }
}
