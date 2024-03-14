using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Messages;
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
        private bool _FastsExists;

        [ObservableProperty]
        private decimal _FoodRatio;

        [ObservableProperty]
        private decimal _DrinkRatio;

        [ObservableProperty]
        private Chart _QuickChart;

        private int _NumberOfRecords = 10;


        public MainQuickViewModel() 
        {
            InitValues();
            SetRefreshValues();
        }

        private async void SetRefreshValues()
        {
            WeakReferenceMessenger.Default.Register<WeightRefreshMessage>(this, async (a, b) => 
            { 
                await LoadWeight(); 
            });

            WeakReferenceMessenger.Default.Register<FoodRefreshMessage>(this, async (a, b) => 
            { 
                await LoadFood(); 
                await LoadChart();
            });

            // Drinking
            WeakReferenceMessenger.Default.Register<DrinkRefreshMessage>(this, async (a, b) => 
            { 
                await LoadDrink(); 
                await LoadChart();
            });

            // Fasting
            WeakReferenceMessenger.Default.Register<StartFastMessage>(this, async (a, b) =>
            {
                await LoadFasts();
                await LoadChart();
            });

            WeakReferenceMessenger.Default.Register<EndFastMessage>(this, async (a, b) =>
            {
                await LoadFasts();
                await LoadChart();
            });
        }

        private async void InitValues()
        {
            List<Task> tasks = new List<Task>
            {
                LoadWeight(), 
                LoadFood(), 
                LoadFasts(), 
                LoadDrink()
            };

            await Task.WhenAll(tasks);
            await LoadChart();
        }

        private async Task LoadChart()
        {
            List<ChartEntry> chartEntries = new List<ChartEntry>();
            
            if (FastsExists)
                chartEntries.Add(new ChartEntry((float)FastRatio) { Color = SKColor.Parse(LazyColors.BootstrapWarningBg) });
            
            chartEntries.Add(new ChartEntry((float)DrinkRatio) { Color = SKColor.Parse(LazyColors.WaterBlue) });
            chartEntries.Add(new ChartEntry((float)FoodRatio) { Color = SKColor.Parse(LazyColors.FreshGreen) });


            QuickChart = new RadialGaugeChart()
            {
                Entries = chartEntries,
                MinValue = 0,
                MaxValue = 100,  
                IsAnimated = false,
            };
        }

        private async Task LoadFood()
        {
            FoodRatio = await FoodService.GetGoodFoodRatio(_NumberOfRecords);
        }

        private async Task LoadDrink()
        {
            DrinkRatio = await DrinkService.GetGoodDrinkRatio(_NumberOfRecords);
        }

        private async Task LoadFasts()
        {
            FastsExists = await FastService.FastsExists(_NumberOfRecords);
            
            if (FastsExists)
                FastRatio = await FastService.GetFastFinishRatio(_NumberOfRecords);
        }

        private async Task LoadWeight()
        {
            Weight = await WeightService.GetWeightMonthAvg(DateTime.Now, _NumberOfRecords);
        }
    }
}
