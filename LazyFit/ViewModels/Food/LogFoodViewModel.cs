using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LazyFit.Models.Foods;
using LazyFit.Services;
using Mopups.Services;
using System.Collections.ObjectModel;

namespace LazyFit.ViewModels.FoodViewModels
{
    internal partial class LogFoodViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<FoodProperty> _Foods;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveFoodCommand))]
        private TimeSpan _SelectedTime;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveFoodCommand))]
        private DateTime _SelectedDate;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveFoodCommand))]
        private FoodProperty _SelectedFood;

        [ObservableProperty]
        private DateTime _MaxDate;

        public LogFoodViewModel()
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
            Foods = new ObservableCollection<FoodProperty>();

            var allDrinks = await FoodService.GetFoodProperties();
            allDrinks.ForEach(Foods.Add);
        }

        [RelayCommand(CanExecute = nameof(CanSave))]
        private async Task SaveFood()
        {
            DateTime time = SelectedDate.Date.Add(SelectedTime);

            Food food = new Food(Guid.NewGuid(), time, SelectedFood.FoodId);
            await FoodService.CreateFood(food);
            await MopupService.Instance.PopAsync();
        }

        [RelayCommand]
        private void SetFood(object selectedDrink)
        {
            SelectedFood = (FoodProperty)selectedDrink;
        }

        private bool CanSave()
        {
            DateTime checkTime = new DateTime(SelectedDate.Date.Ticks + SelectedTime.Ticks);
            return checkTime <= DateTime.Now && SelectedFood != null;
        }



    }
}
