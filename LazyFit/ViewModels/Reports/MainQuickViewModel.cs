using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Messages;
using LazyFit.Models.Foods;
using LazyFit.Services;
using Microcharts;
using SkiaSharp;

namespace LazyFit.ViewModels.Reports
{
    internal partial class MainQuickViewModel : ObservableObject
    {
        [ObservableProperty]
        private decimal _Weight;

        [ObservableProperty]
        private decimal _FastRatio;

        [ObservableProperty]
        private decimal _FoodRatio;

        [ObservableProperty]
        private decimal _DrinkRatio;

        [ObservableProperty]
        private Chart _QuickChart;

        public MainQuickViewModel() 
        {
            InitValues();
            SetRefreshValues();
        }

        private async void SetRefreshValues()
        {
            WeakReferenceMessenger.Default.Register<NewWeightMessage>(this, async (a, b) => 
            { 
                await LoadWeight(); 
                //await LoadChart(); 
            });

            WeakReferenceMessenger.Default.Register<NewFoodMessage>(this, async (a, b) => 
            { 
                await LoadFood(); 
                await LoadChart();
            });
        }

        private async void InitValues()
        {
            List<Task> tasks = new List<Task>
            {
                LoadWeight(), LoadFood(), LoadFood(), LoadDrink()
            };

            await Task.WhenAll(tasks);
            await LoadChart();
        }

        private async Task LoadChart()
        {
            List<ChartEntry> chartEntries = new List<ChartEntry>()
            {
                new ChartEntry((float)FastRatio){ Color = SKColor.Parse(LazyColors.BootstrapWarningBg)},
                new ChartEntry((float)DrinkRatio){ Color = SKColor.Parse(LazyColors.WaterBlue)},
                new ChartEntry((float)FoodRatio){ Color = SKColor.Parse(LazyColors.DarkFreshGreen)},
            };


            QuickChart = new RadialGaugeChart()
            {
                Entries = chartEntries,
                MinValue = 0,
                MaxValue = 100,  
            };
        }

        private async Task LoadFood()
        {
            FoodRatio = await FoodService.GetGoodFoodRatio(10);
        }

        private async Task LoadDrink()
        {
            DrinkRatio = await DrinkService.GetGoodDrinkRatio(10);
        }

        private async Task LoadFasts()
        {
            FastRatio = await FastService.GetFastFinishRatio(10);
        }

        private async Task LoadWeight()
        {
            Weight = await WeightService.GetWeightMonthAvg(DateTime.Now,10);
        }
    }
}
