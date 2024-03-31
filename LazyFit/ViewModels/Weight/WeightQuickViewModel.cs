using CommunityToolkit.Mvvm.ComponentModel;
using LazyFit.Services;
using Microcharts;
using SkiaSharp;
using System.Runtime.CompilerServices;

namespace LazyFit.ViewModels.WeightViewModels
{
    internal partial class WeightQuickViewModel : ObservableObject
    {
        [ObservableProperty]
        private Chart _WeightChart;

        [ObservableProperty]
        private int _WeightAvg;
       
        public WeightQuickViewModel() 
        {
            LoadData();
        }

        private async void LoadData()
        {
            await LoadChart();
        }

        private async Task LoadChart()
        {
            var entries = new List<ChartEntry>();
            var weights = await WeightService.GetLastWeights(10);

            if (weights != null)
            {
                weights.ForEach(weight =>
                {
                    entries.Add(new ChartEntry((float)weight.WeightValue)
                    {
                        ValueLabel = weight.WeightValue.ToString(),
                        Color = SKColors.DarkOrange
                    });
                });

            }
            else
                entries.Add(new ChartEntry(0));

            WeightChart = new LineChart()
            {
                Entries = entries,
                ValueLabelTextSize = 22,
                ValueLabelOption = ValueLabelOption.TopOfElement,
                IsAnimated = false
            };
        }

    }
}
