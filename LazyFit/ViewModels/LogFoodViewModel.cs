using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Models.Foods;
using LazyFit.Services;
using Mopups.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LazyFit.ViewModels
{
    internal class LogFoodViewModel : PrimeViewModel
    {


        private ObservableCollection<FoodProperty> _Foods;
        public ObservableCollection<FoodProperty> Foods { get => _Foods; set => SetProperty(ref _Foods, value); }


        private TimeSpan _SelectedTime;

        public TimeSpan SelectedTime
        {
            get => _SelectedTime; set
            {
                SetProperty(ref _SelectedTime, value);
                RefreshCans();
            }
        }

        private FoodProperty _SelectedFood;
        public FoodProperty SelectedFood
        {
            get => _SelectedFood;
            set
            {
                SetProperty(ref _SelectedFood, value);
                RefreshCans();
            }
        }

        public ICommand SaveFood { private set; get; }
        public ICommand SetFood { private set; get; }
        public ICommand SetTimeNow { private set; get; }

        public LogFoodViewModel()
        {
            SaveFood = new Command(SaveDrinkHandler, CanSave);
            SetFood = new Command(SetDrinkHandler);
            SetTimeNow = new Command(SetNow);

            SetNow();
            LoadDrinks();

        }

        private void SetNow()
        {
            SelectedTime = DateTime.Now.TimeOfDay;
        }

        private async void LoadDrinks()
        {
            Foods = new ObservableCollection<FoodProperty>();

            var allDrinks = await DB.GetFoodProperties();
            allDrinks.ForEach(Foods.Add);
        }

        private async void SaveDrinkHandler()
        {
            DateTime time = DateTime.Now.Date.Add(SelectedTime);
            await DB.InsertFood(new Food(Guid.NewGuid(), time, SelectedFood.FoodId));

            WeakReferenceMessenger.Default.Send(new Messages.ReloadActionsMessage(0));

            await MopupService.Instance.PopAsync();
        }

        private bool CanSave()
        {
            return SelectedTime <= DateTime.Now.TimeOfDay && SelectedFood != null;
        }

        private async void SetDrinkHandler(object selectedDrink)
        {
            SelectedFood = (FoodProperty)selectedDrink;
        }

        private void RefreshCans()
        {
            ((Command)SaveFood).ChangeCanExecute();
        }


    }
}
