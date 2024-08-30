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

        private int _NumberOfDays = 15;

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
            var drinks = await DrinkService.GetDrinksFromLastDays(_NumberOfDays);
            DrinkExists = drinks.Any();
            DrinkRatio = DrinkService.GetGoodDrinkRationFromList(drinks);
        }

        private async Task LoadFasts()
        {
            var fasts = await FastService.GetFastsFromLastDays(_NumberOfDays);
            FastsExists = fasts.Any();
            
            if (FastsExists)
                FastRatio = FastService.GetFastFinishRatioFromList(fasts);
        }

        private async Task LoadWeight()
        {
            Weight = await WeightService.GetWeightMonthAvg(DateTime.Now, _NumberOfDays);
        }
    }
}
