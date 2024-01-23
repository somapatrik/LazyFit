using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Classes;
using LazyFit.Models;
using LazyFit.Services;
using Microcharts;
using SkiaSharp;
using SkiaSharp.Views.Android;

namespace LazyFit.ViewModels.Fasting
{

    internal class FastChartViewModel : ResultComponent
    {
        private Chart _FastChart;
        private double _HoursFasted;
        private double _HoursShouldFasted;
        private bool _Completed;
        private double _PercentFinished;

        public Chart FastChart { get => _FastChart; set => SetProperty(ref _FastChart, value); }
        public double HoursFasted { get => _HoursFasted; set => SetProperty(ref _HoursFasted, value); }
        public double HoursShouldFasted { get => _HoursShouldFasted; set => SetProperty(ref _HoursShouldFasted, value); }
        public double PercentFinished { get=>_PercentFinished; set => SetProperty(ref _PercentFinished,value); }
        public bool Completed { get => _Completed; set => SetProperty(ref _Completed, value); }
        public FastChartViewModel()
        {

        }

        protected override async void LoadResults()
        {
            List<Fast> fasts = await DB.GetFasts(FirstDateTime, LastDateTime);
            List<ChartEntry> entries = new List<ChartEntry>();
            DataExists = fasts.Any();

            HoursFasted = HoursShouldFasted = 0;
            PercentFinished = 0;
            Completed = false;

            if (DataExists)
            {
                HoursFasted = Math.Floor(fasts.Sum(f => ((TimeSpan)(f.EndTime - f.StartTime)).TotalHours));
                HoursShouldFasted = Math.Floor(fasts.Sum(f => (f.GetPlannedEnd() - f.StartTime).TotalHours));
                
                var percent = Math.Round((HoursFasted / HoursShouldFasted) * 100, 0);
                if (percent > 100)
                    percent = 100;
                else if (percent < 0)
                    percent = 0;


                PercentFinished = percent;

                Completed = PercentFinished == 100;

                SKColor color = SKColors.IndianRed;

                if (PercentFinished >= 50 && PercentFinished < 75)
                    color = SKColor.Parse("#ffc107");
                else if (PercentFinished >= 75 && PercentFinished < 100)
                    color = SKColor.Parse("#187ccf");
                else if (PercentFinished == 100)
                    color = SKColors.LimeGreen;

                entries.AddRange(new List<ChartEntry>()
                {
                    new ChartEntry((float)PercentFinished){ Color = color},
                    new ChartEntry((float)(100 - PercentFinished)){ Color = SKColor.Parse("#f6f8fa")}
                 });
            }
            else
            {
                entries.Add(new ChartEntry(0) { Color = SKColor.Parse("#f6f8fa") });
                entries.Add(new ChartEntry(100) { Color = SKColor.Parse("#f6f8fa") });
            }

            FastChart = new DonutChart()
            {
                Entries = entries,
                IsAnimated = false,
                HoleRadius = 0.6f
            };

        }

        

    }
}
