using LazyFit.Models;
using LazyFit.Services;
using Microcharts;
using System.Linq;

namespace LazyFit.ViewModels
{
    internal class WeightChartViewModel : PrimeViewModel
    {
        private Chart _chartWeight;
        public Chart ChartWeight { get => _chartWeight;set=> SetProperty(ref _chartWeight,value); }

        public WeightChartViewModel(DateTime dateFrom, DateTime dateTo) 
        {
            LoadChartData();
        }

        public async void LoadChartData()
        {
            List<Weight> weights = await DB.GetWeightFromTime(DateTime.Now.AddMonths(-1), DateTime.Now);
            List<ChartEntry> entries = weights.Select(weight => new ChartEntry((float)weight.WeightValue)
            {
                 ValueLabel = weight.WeightValue.ToString()
            }).ToList();

            ChartWeight = new LineChart() { Entries = entries };
        }
    }
}
