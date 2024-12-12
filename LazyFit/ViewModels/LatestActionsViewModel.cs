using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Messages;
using LazyFit.Models;
using LazyFit.Models.Drinks;
using LazyFit.Models.Foods;
using LazyFit.Models.Moods;
using LazyFit.Models.WeightModels;
using LazyFit.Services;
using LazyFit.Views.Reports;
using System.Collections.ObjectModel;

namespace LazyFit.ViewModels
{
    partial class LatestActionsViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<ActionSquareDate> _ActionSquares;

        private int numberOfDays = 15;

        FoodService FoodService;
        DrinkService DrinkService;
        MoodService MoodService;
        WeightService WeightService;
        FastService FastService;

        public LatestActionsViewModel() 
        {
            FoodService = new FoodService();
            DrinkService = new DrinkService();
            MoodService = new MoodService();
            WeightService = new WeightService();
            FastService = new FastService();

            LoadData();
            WireMessages();
        }

        private void WireMessages()
        {
            // Drinking
            WeakReferenceMessenger.Default.Register<DrinkNewMessage>(this, async (a,b) => await LoadActions());
            WeakReferenceMessenger.Default.Register<DrinkDeleteMessage>(this, async (a, b) => await LoadActions());

            // Food
            WeakReferenceMessenger.Default.Register<FoodNewMessage>(this, async (a, b) => await LoadActions());
            WeakReferenceMessenger.Default.Register<FoodDeleteMessage>(this, async (a, b) => await LoadActions());

            // Fast
            WeakReferenceMessenger.Default.Register<FastEndMessage>(this, async (a, b) => await LoadActions());
            WeakReferenceMessenger.Default.Register<FastDeleteMessage>(this, async (a, b) => await LoadActions());

            // Weight
            WeakReferenceMessenger.Default.Register<WeightRefreshMessage>(this, async (a, b) => await LoadActions());

            // Mood
            WeakReferenceMessenger.Default.Register<MoodNewMessage>(this, async (a,b) => await LoadActions());
            WeakReferenceMessenger.Default.Register<MoodDeleteMessage>(this, async (a, b) => await LoadActions());
            WeakReferenceMessenger.Default.Register<MoodUpdateMessage>(this, async (a, b) => await LoadActions());

            // Refresh actions
            WeakReferenceMessenger.Default.Register<ActionsReloadMessages>(this, async (a,b) => await LoadActions());
        }

        private async void LoadData()
        {
            await LoadActions();
        }

        private async Task LoadActions()
        {
            ActionSquares = new ObservableCollection<ActionSquareDate>();

            DateTime now = DateTime.Now;
            
            for (int days = 0; days < numberOfDays; days++)
            {
                DateTime from = now.AddDays(-days).Date;
                DateTime to = from.AddDays(1).AddSeconds(-1);
                var actions = await GetActionSquares(from, to);

                if (actions.Any())
                    ActionSquares.Add(new ActionSquareDate() { Time = from, Actions = actions.OrderByDescending(a=>a.Time).ToList() });

            }            
        }

        [RelayCommand]
        private async Task OpenDay(object sender)
        {
            ActionSquareDate day = (ActionSquareDate)sender;
            //await MopupService.Instance.PushAsync(new ActionDayView(day));
            await Shell.Current.Navigation.PushAsync(new ActionDayView(day));
        }

        public async Task<List<ActionSquare>> GetActionSquares(DateTime fromTime, DateTime toTime)
        {
            var foods = await FoodService.GetFoods(fromTime, toTime, true);
            var drinks = await DrinkService.GetDrinks(fromTime, toTime, true);
            var moods = await MoodService.GetMoods(fromTime, toTime, true);
            var weights = await WeightService.GetWeights(fromTime, toTime);
            var fasts = await FastService.GetFasts(fromTime, toTime);

            List<ActionSquare> actionSquares = new List<ActionSquare>();

            foods.ForEach(food =>
            {
                actionSquares.Add(new ActionSquare()
                {
                    ActionObject = food,
                    ActionName = nameof(Food),
                    Color = LazyColors.FreshGreen,
                    Time = food.Time,
                    IsBad = (food.TypeOfFood == TypeOfFood.Unhealthy || food.TypeOfFood == TypeOfFood.Snack),
                    ItemName = Enum.GetName(typeof(TypeOfFood), food.TypeOfFood),
                    IconName = food.Property.ImageName

                });
            });

            drinks.ForEach(drink =>
            {
                actionSquares.Add(new ActionSquare()
                {
                    ActionObject = drink,
                    ActionName = nameof(Drink),
                    Color = LazyColors.WaterBlue,
                    Time = drink.Time,
                    IsBad = (drink.TypeOfDrink != TypeOfDrink.Water && drink.TypeOfDrink != TypeOfDrink.Tea),
                    ItemName = Enum.GetName(typeof(TypeOfDrink), drink.TypeOfDrink),
                    IconName = drink.Property.ImageName
                });
            });

            moods.ForEach(mood =>
            {

                string moodName = Enum.GetName(typeof(MoodName), mood.TypeOfMood);

                if (mood.TypeOfMood == MoodName.VeryBad)
                {
                    moodName = "Very bad";
                }

                if (mood.TypeOfMood == MoodName.VeryGood)
                {
                    moodName = "Very good";
                }

                actionSquares.Add(new ActionSquare()
                {
                    ActionObject = mood,
                    ActionName = nameof(Mood),
                    Color = Colors.DarkBlue.ToHex(),
                    Time = mood.Time,
                    IsBad = mood.TypeOfMood == MoodName.Bad,
                    ItemName = $"{moodName} mood",
                    IconName = mood.Property.ImageName
                });
            });

            fasts.ForEach(fast =>
            {
                actionSquares.Add(new ActionSquare()
                {
                    ActionObject = fast,
                    ActionName = nameof(Fast),
                    Color = LazyColors.LazyColor,
                    Time = (DateTime)fast.EndTime,
                    IsBad = (!fast.Completed),
                    ItemName = fast.Completed ? "Completed fast" : "Failed fast",
                    IconName = "fasting.png"
                });
            });


            weights.ForEach(weight =>
            {

                actionSquares.Add(new ActionSquare()
                {
                    ActionObject = weight,
                    ActionName = nameof(Weight),
                    Color = Colors.DarkOrange.ToHex(),
                    Time = weight.Time,
                    IsBad = false,
                    ItemName = weight.WeightValue.ToString(),
                    IconName = "weight.png"
                });
            });


            return actionSquares.OrderBy(a => a.Time).ToList();
        }


    }
}
