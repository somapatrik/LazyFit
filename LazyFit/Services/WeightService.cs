using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Models.WeightModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace LazyFit.Services
{
    public static class WeightService
    {
        public static async Task InsertWeight(Weight weight)
        {
            await DB.Database.InsertAsync(weight);
            WeakReferenceMessenger.Default.Send(new Messages.NewWeightMessage(weight));
        }
        public static async Task<Weight> GetLastWeight()
        {
            return await DB.Database.Table<Weight>().OrderByDescending(w => w.Time).FirstOrDefaultAsync();
        }

        public static async Task<List<Weight>> GetLastWeights(int numberOfWeights)
        {
            return await DB.Database.Table<Weight>().OrderByDescending(w=>w.Time).Take(numberOfWeights).ToListAsync();
        }

        public static async Task<int> GetWeightMonthAvg(DateTime dateTime, int numberOfWeights)
        {
            int AvgWeight = 0;

            DateTime from = new DateTime(dateTime.Year, dateTime.Month, 1);
            DateTime to = from.AddMonths(1).AddSeconds(-1);
            var weights = await GetLastWeights(numberOfWeights);

            if (weights.Any())
                AvgWeight = (int)Math.Round(weights.Select(w=>w.WeightValue).Average(),0);

            return AvgWeight;
        }
    }
}
