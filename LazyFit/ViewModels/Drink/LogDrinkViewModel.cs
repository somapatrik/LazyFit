using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LazyFit.Models.Drinks;
using LazyFit.Services;
using Mopups.Services;
using System.Collections.ObjectModel;

namespace LazyFit.ViewModels.DrinkViewModels
{
    internal partial class LogDrinkViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<DrinkProperty> _Drinks;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveDrinkCommand))]
        private DateTime _SelectedDate;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveDrinkCommand))]
        private TimeSpan _SelectedTime;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveDrinkCommand))]
        private DrinkProperty _SelectedDrink;

        [ObservableProperty]
        private DateTime _MaxDate;

        public LogDrinkViewModel()
        {
            MaxDate = DateTime.Today;

            SetNow();
            LoadDrinks();
        }

        [RelayCommand]
        private void SetNow()
        {
            SelectedDate = DateTime.Now.Date;
            SelectedTime = DateTime.Now.TimeOfDay;
        }

        private async void LoadDrinks()
        {
            Drinks = new ObservableCollection<DrinkProperty>();

            var allDrinks = await DrinkService.GetDrinkProperties();
            allDrinks.ForEach(Drinks.Add);
        }

        [RelayCommand(CanExecute = nameof(CanSave))]
        private async Task SaveDrink()
        {
            DateTime time = SelectedDate.Date.Add(SelectedTime);
            Drink drink = new Drink(Guid.NewGuid(), time, SelectedDrink.DrinkID);

            await DrinkService.CreateDrink(drink);
  
            await MopupService.Instance.PopAsync();
        }
        [RelayCommand]
        private void SetDrink(object selectedDrink)
        {
            SelectedDrink = (DrinkProperty)selectedDrink;
        }

        private bool CanSave()
        {
            DateTime checkTime = new DateTime(SelectedDate.Date.Ticks + SelectedTime.Ticks);
            return checkTime <= DateTime.Now && SelectedDrink != null;
        }


    }
}
