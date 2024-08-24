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
        [NotifyPropertyChangedFor(nameof(IsVisible))]
        private bool _FoodExists;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsVisible))]
        private bool _FastsExists;

        [ObservableProperty]
        private decimal _FoodRatio;

        [ObservableProperty]
        private decimal _DrinkRatio;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsVisible))]
        private bool _DrinkExists;

        [ObservableProperty]
        private Chart _QuickChart;

        private int _NumberOfDays = 10;

        public bool IsVisible => FoodExists || DrinkExists || FastsExists;


        public MainQuickViewModel() 
        {
            InitValues();
            SetRefreshValues();
        }

        private async void SetRefreshValues()
        {
            // Food
            WeakReferenceMessenger.Default.Register<FoodNewMessage>(this, async (a, b) =>
            {
                await LoadFood();
                await LoadChart();
            });

            WeakReferenceMessenger.Default.Register<FoodDeleteMessage>(this, async (a, b) =>
            {
                await LoadFood();
                await LoadChart();
            });

            // Drinking
            WeakReferenceMessenger.Default.Register<DrinkNewMessage>(this, async (a, b) =>
            {
                await LoadDrink();
                await LoadChart();
            });

            WeakReferenceMessenger.Default.Register<DrinkDeleteMessage>(this, async (a, b) =>
            {
                await LoadDrink();
                await LoadChart();
            });

            // Fasting
            //WeakReferenceMessenger.Default.Register<FastStartMessage>(this, async (a, b) =>
            //{
            //    await LoadFasts();
            //    await LoadChart();
            //});

            WeakReferenceMessenger.Default.Register<FastEndMessage>(this, async (a, b) =>
            {
                await LoadFasts();
                await LoadChart();
            });

            WeakReferenceMessenger.Default.Register<FastDeleteMessage>(this, async (a, b) =>
            {
                await LoadFasts();
                await LoadChart();
            });
        }

        private async void InitValues()
        {
            List<Task> tasks = new List<Task>
            {
                //LoadWeight(), 
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
                chartEntries.Add(new ChartEntry((float)FastRatio) { Color = SKColor.Parse(LazyColors.Yellow100Accent) });
            
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
            var foods = await FoodService.GetFoodsFromLastDays(_NumberOfDays);

            FoodExists = foods.Any();
            FoodRatio = FoodService.GetFoodRatioFromList(foods);
        }

        private async Task LoadDrink()
        {
            DrinkExists = (await DrinkService.GetLastDrinks(_NumberOfDays)).Any();
            DrinkRatio = await DrinkService.GetGoodDrinkRatio(_NumberOfDays);
        }

        private async Task LoadFasts()
        {
            FastsExists = await FastService.FastsExists(_NumberOfDays);
            
            if (FastsExists)
                FastRatio = await FastService.GetFastFinishRatio(_NumberOfDays);
        }

        private async Task LoadWeight()
        {
            Weight = await WeightService.GetWeightMonthAvg(DateTime.Now, _NumberOfDays);
        }
    }
}
