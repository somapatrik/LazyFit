using LazyFit.Models;
using LazyFit.Services;
using Microcharts;
using Microcharts.Maui;
using SkiaSharp;
using System.Linq;

namespace LazyFit.ViewModels
{
    internal class WeightChartViewModel : PrimeViewModel
    {
        private Chart _chartWeight;
        public Chart ChartWeight { get => _chartWeight; set => SetProperty(ref _chartWeight, value); }
        
        private bool _noData;
        public bool NoData { get => _noData; set => SetProperty(ref _noData, value); }
        public DateTime DateFrom { get; }
        public DateTime DateTo { get; }

        public WeightChartViewModel(DateTime dateFrom, DateTime dateTo) 
        {
            LoadChartData();
            DateFrom = dateFrom;
            DateTo = dateTo;
        }

        public async void LoadChartData()
        {

            List<Weight> weights = await DB.GetLastWeights(10);

            var entries = weights
                .Select(weight => new ChartEntry((float)weight.WeightValue)
                {
                    ValueLabel = weight.WeightValue.ToString(),
                    ValueLabelColor = SKColors.OrangeRed,
                    Label = weight.Time.ToString("F"),
                    OtherColor = SKColors.OrangeRed
                }).ToList();


            if (!entries.Any())
            {    
               entries.Add(new ChartEntry(0));
               NoData = true;
            }
                

            List<ChartSerie> series = new List<ChartSerie>()
            {
                new ChartSerie() 
                { 
                    Entries = entries, 
                    Color = SKColors.OrangeRed,
                    Name="Weight"
                }
            };

            ChartWeight = new LineChart()
            {
                Series = series,
                IsAnimated = false,
                EnableYFadeOutGradient = false,
                LineSize = 7,
                PointSize = 20,
                ShowYAxisLines = false,
                LineMode = LineMode.Spline,
                ValueLabelTextSize = 36,
                LabelTextSize = 36,
                ValueLabelOrientation = Orientation.Horizontal,
                ValueLabelOption = ValueLabelOption.TopOfElement,
                LabelOrientation = Orientation.Vertical,
                
            };

        }
    }
}
