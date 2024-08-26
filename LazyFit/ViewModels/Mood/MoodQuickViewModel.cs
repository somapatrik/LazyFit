using CommunityToolkit.Mvvm.ComponentModel;
using LazyFit.Classes;
using LazyFit.Services;
using Microcharts;

namespace LazyFit.ViewModels.MoodViewModels
{
    internal partial class MoodQuickViewModel : ObservableObject
    {
        [ObservableProperty]
        private Chart _chart;

        [ObservableProperty]
        private bool _moodsExists;

        private int _numberOfDays = 10;

        public MoodQuickViewModel()
        {
            LoadChart();
        }

        private async void LoadChart()
        {
            var moods = await MoodService.GetMoodsFromLastDays(10);
            MoodsExists = moods.Any();

            var entries = GetEntriesReady();

            var groupedMoods = moods.GroupBy(mood => mood.Time.Date)
                .Select(item => new DateFloat() { Date = item.Key, Value = item.Average(m=> (float) m.TypeOfMood)}).ToList();

            groupedMoods.ForEach(g=> );

        }

        private List<ChartEntry> GetEntriesReady()
        {
            List<ChartEntry> entries = new List<ChartEntry>();

            DateTime now = DateTime.Now;
            DateTime someDay = now.AddDays(-_numberOfDays);

            for (int i = 0; i < _numberOfDays; i++)
            {
                entries.Add(new ChartEntry(0) { Label = someDay.ToString("d.") });
                someDay.AddDays(1);
            }

            return entries;
        }

    }
}
