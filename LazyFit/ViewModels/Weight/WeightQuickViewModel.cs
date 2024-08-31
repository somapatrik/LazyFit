using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Messages;
using LazyFit.Services;
using Microcharts;
using SkiaSharp;

namespace LazyFit.ViewModels.WeightViewModels
{
    internal partial class WeightQuickViewModel : ObservableObject
    {
        [ObservableProperty]
        private Chart _WeightChart;

        [ObservableProperty]
        private string _WeightAvg;

        [ObservableProperty]
        private bool _IsVisible;


        public WeightQuickViewModel() 
        {
            LoadData();

            
        }

        private async void LoadData()
        {
            await LoadChart();

            WeakReferenceMessenger.Default.Register<WeightRefreshMessage>(this, async (a, b) =>
            {
                await LoadChart();
            });

            WeakReferenceMessenger.Default.Register<ActionsReloadMessages>(this, async (a, b) =>
            {
                await LoadChart();
            });
        }

        private async Task LoadChart()
        {
            var entries = new List<ChartEntry>();
            var weights = (await WeightService.GetLastWeights(10)).OrderBy(w => w.Time).ToList();

            if (weights.Any())
            {
                IsVisible = true;
                weights.ForEach(weight =>
                {
                    entries.Add(new ChartEntry((float)weight.WeightValue)
                    {
                        ValueLabel = weight.WeightValue.ToString(),
                        Color = SKColors.DarkOrange,
                        ValueLabelColor = SKColors.DarkOrange
                    });
                });
                WeightAvg = Math.Round(weights.Average(w => w.WeightValue), 1).ToString();
            }
            else
            {
                IsVisible=false;
                entries.Add(new ChartEntry(0));
                WeightAvg = "0";
            }

            WeightChart = new LineChart()
            {
                Entries = entries,
                ValueLabelTextSize = 36,
                LineSize = 7,
                PointSize = 20,
                ValueLabelOption = ValueLabelOption.TopOfElement,
                ValueLabelOrientation = Orientation.Horizontal
            };
        }

    }
}
