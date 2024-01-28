using LazyFit.Classes;
using LazyFit.Models.Foods;
using LazyFit.Services;
using Microcharts;
using SkiaSharp;
using LazyFit.Models;

namespace LazyFit.ViewModels.MoodViewModels
{
    internal class MoodChartResultViewModel : ResultComponent
    {
        private Chart _MoodChart;

        public Chart MoodChart { get => _MoodChart; set => SetProperty(ref _MoodChart, value); }
        protected async override void LoadResults()
        {
            List<Mood> moods = await DB.GetMoods(FirstDateTime, LastDateTime);
            DataExists = moods.Any();
            List<ChartEntry> entries = new List<ChartEntry>
            {
                new ChartEntry(0) { Label = "Good", Color = SKColors.Gray },
                new ChartEntry(0) { Label = "Bad", Color = SKColors.Gray },
                new ChartEntry(0) { Label = "Normal", Color = SKColors.Gray }
            };

            if (DataExists)
            {
                var moodTypeCounts = moods
                   .GroupBy(f => f.TypeOfMood)
                   .Select(group => new
                   {
                       TypeOfMood = group.Key,
                       Count = group.Count()
                   });

                foreach (var moodGroup in moodTypeCounts)
                {
                    SKColor color = SKColors.Gray;
                    string name = "";

                    if (moodGroup.TypeOfMood == MoodName.Good)
                    {
                        name = "Good";
                        color = SKColors.LimeGreen;
                    }
                    else if (moodGroup.TypeOfMood == MoodName.Bad)
                    {
                        name = "Bad";
                        color = SKColors.IndianRed;
                    }
                    else if (moodGroup.TypeOfMood == MoodName.Normal)
                    {
                        name = "Normal";
                        color = SKColor.Parse("#187ccf");
                    }

                    var found = entries.FirstOrDefault(x => x.Label == name);
                    if (found != null)
                    {
                        entries.Remove(found);
                        entries.Add(new ChartEntry(moodGroup.Count)
                        {
                            Color = color,
                            Label = name,
                            ValueLabel = moodGroup.Count.ToString()
                        });
                    }
                }
            }


            MoodChart = new RadarChart()
            {
                Entries = entries.OrderBy(x => x.Label),
                LabelTextSize = 40,
                LineSize = 6
            };
        }
    }
}
