using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Messages;
using LazyFit.Services;

namespace LazyFit.ViewModels.Reports
{
    partial class NoWaterWarningViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _IsVisible;

        public NoWaterWarningViewModel()
        {
            WeakReferenceMessenger.Default.Register<DrinkNewMessage>(this, async (a, b) =>
            {
                await CheckWaterToday();
            });

            WeakReferenceMessenger.Default.Register<DrinkDeleteMessage>(this, async (a, b) =>
            {
                await CheckWaterToday();
            });

            CheckWaterToday();
        }

        public async Task CheckWaterToday()
        {
            var now = DateTime.Now;
            var fromDate = now.Hour < 12 ? now.Date : now.Date.AddHours(12);
            
            var drinks = await DrinkService.GetDrinks(fromDate, DateTime.Now, true);

            if (drinks.FirstOrDefault(d => d.TypeOfDrink == Models.Drinks.TypeOfDrink.Water) != null)
                IsVisible = false;
            else
                IsVisible = true;
        }

        //[RelayCommand]
        //public async Task CheckWater()
        //{
        //    await CheckWaterToday();
        //}
    }
}
