using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Classes;
using LazyFit.Messages;
using LazyFit.Models.Moods;
using LazyFit.Services;
using Microcharts;
using SkiaSharp;

namespace LazyFit.ViewModels.MoodViewModels
{
    internal partial class MoodQuickViewModel : ObservableObject
    {
        [ObservableProperty]
        private Chart _chart;

        [ObservableProperty]
        private bool _moodsExists;

        [ObservableProperty]
        private string _imageTitle;

        private int _numberOfDays = 15;

        public MoodQuickViewModel()
        {
            WeakReferenceMessenger.Default.Register<MoodNewMessage>(this,  (a, b) =>  LoadChart());
            WeakReferenceMessenger.Default.Register<MoodDeleteMessage>(this,  (a, b) =>  LoadChart());
            WeakReferenceMessenger.Default.Register<MoodUpdateMessage>(this,  (a, b) =>  LoadChart());

            LoadChart();
        }

        private async void LoadChart()
        {
            // Get moods
            var moods = await MoodService.GetMoodsFromLastDays(_numberOfDays);
            MoodsExists = moods.Any();

            SetTitle(moods);

            // DateFloats
            var entries = GetEntriesReady();

            if (MoodsExists)
            {

                // Group moods by day and Avg. value
                var groupedMoods = moods.GroupBy(mood => mood.Time.Date)
                    .Select(item => new DateFloat() 
                    { 
                        Date = item.Key, Value = item.Average(m => (float)m.TypeOfMood) 
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


                chartEntries.Add(new ChartEntry(entry.Value)
                {
                    Label = entry.Date.ToString("d."),
                    Color = color
                });
            });

            Chart = new LineChart()
            {
                Entries = chartEntries,
                MaxValue = 5,
                MinValue = 0,
                LabelTextSize = 32,
                LabelOrientation = Orientation.Vertical,
                PointMode = PointMode.None
            };


        }

        private List<DateFloat> GetEntriesReady()
        {
            List<DateFloat> entries = new List<DateFloat>();

            DateTime now = DateTime.Now;
            DateTime someDay = now.AddDays(-_numberOfDays).Date;

            for (int i = 0; i <= _numberOfDays; i++)
            {
                entries.Add(new DateFloat() { Date = someDay, Value = 0 });
                someDay = someDay.AddDays(1);
            }

            return entries;
        }

        private async void SetTitle(List<Mood> moods)
        {
            var title = "";

            // Select the lowest mood as a default
            var moodProperties = await MoodService.GetAllMoodProperties();
            MoodProperty worstMood = moodProperties.FirstOrDefault(m=> m.MoodID == moodProperties.Min(mp => mp.MoodID));

            title = worstMood.ImageName;

            if (moods.Any())
            {
                var avgScore = moods.Average(m => (int)m.Property.MoodID);
                var roundedMood = moodProperties.First(mp => (int)mp.MoodID == Math.Round(avgScore));
                title = roundedMood.ImageName;
            }

            ImageTitle = title;
        }

    }
}
