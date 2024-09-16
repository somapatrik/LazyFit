using CommunityToolkit.Mvvm.ComponentModel;
using LazyFit.Classes;
using LazyFit.Services;
using Microcharts;
using SkiaSharp;


namespace LazyFit.ViewModels.MoodViewModels
{
    internal partial class MoodChartViewModel : ObservableObject
    {
        [ObservableProperty]
        private DateTime _FromDate;
        partial void OnFromDateChanged(DateTime value)
        {
            LoadChart();
        }

        [ObservableProperty]
        private DateTime _ToDate;
        partial void OnToDateChanged(DateTime value)
        {
            LoadChart();
        }

        [ObservableProperty]
        private Chart _ChartObject;

        public MoodChartViewModel() 
        {      
            LoadChart();
        }

        private async void LoadChart()
        {
            var entries = GetEntriesReady();

            // Get moods
            var moods = await MoodService.GetMoods(FromDate, ToDate, true);

            if (moods.Any())
            {


                // Group moods by day and Avg. value
                var groupedMoods = moods.GroupBy(mood => mood.Time.Date)
                    .Select(item => new DateFloat()
                    {
                        Date = item.Key,
                        Value = item.Average(m => (float)m.TypeOfMood)
                    })
                    .ToList();

                // Find same day as in entries
                groupedMoods.ForEach(mood =>
                {
                    var found = entries.FirstOrDefault(ent => ent.Date == mood.Date);

                    if (found != null)
                        found.Value = mood.Value;
                });

            }

            List<ChartEntry> chartEntries = new List<ChartEntry>();
            entries.ForEach(entry =>
            {
                var color = SKColors.Transparent;

                if (entry.Value >= 0 && entry.Value < 1)
                    color = SKColors.Red;
                else if (entry.Value >= 1 && entry.Value < 2)
                    color = SKColors.IndianRed;
                else if (entry.Value >= 2 && entry.Value < 3)
                    color = SKColors.Orange;
                else if (entry.Value >= 3 && entry.Value < 4)
                    color = SKColors.LawnGreen;
                else if (entry.Value >= 4)
                    color = SKColors.LimeGreen;


                float? EntryValue = entry.Value == -1 ? null : entry.Value;
                chartEntries.Add(new ChartEntry(EntryValue)
                {
                    Label = entry.Date.ToString("d."),
                    Color = color
                });
            });

            ChartObject = new PointChart()
            {
                Entries = chartEntries,
                MaxValue = 5,
                MinValue = 0,
                LabelTextSize = 32,
                LabelOrientation = Orientation.Vertical,
                PointSize = 25
            };


        }

        private List<DateFloat> GetEntriesReady()
        {
            List<DateFloat> entries = new List<DateFloat>();

            DateTime startDate = FromDate;
            DateTime endDate = ToDate;

            while (startDate.Date != endDate.Date)
            {
                entries.Add(new DateFloat() { Date = startDate, Value = -1 });
                startDate = startDate.AddDays(1);
            }

            return entries;
        }
    }
}
