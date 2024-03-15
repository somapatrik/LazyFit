using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Models.Drinks;
using LazyFit.Services;
using Mopups.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LazyFit.ViewModels.DrinkViewModels
{
    class LogDrinkViewModel : PrimeViewModel
    {

        private ObservableCollection<DrinkProperty> _Drinks;
        public ObservableCollection<DrinkProperty> Drinks { get => _Drinks; set => SetProperty(ref _Drinks, value); }


        private TimeSpan _SelectedTime;

        public TimeSpan SelectedTime
        {
            get => _SelectedTime; set
            {
                SetProperty(ref _SelectedTime, value);
                RefreshCans();
            }
        }

        private DrinkProperty _SelectedDrink;
        public DrinkProperty SelectedDrink
        {
            get => _SelectedDrink;
            set
            {
                SetProperty(ref _SelectedDrink, value);
                RefreshCans();
            }
        }

        public ICommand SaveDrink { private set; get; }
        public ICommand SetDrink { private set; get; }
        public ICommand SetTimeNow { private set; get; }

        public LogDrinkViewModel()
        {
            SaveDrink = new Command(SaveDrinkHandler, CanSave);
            SetDrink = new Command(SetDrinkHandler);
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
            Drinks = new ObservableCollection<DrinkProperty>();

            var allDrinks = await DrinkService.GetDrinkProperties();
            allDrinks.ForEach(Drinks.Add);
        }

        private async void SaveDrinkHandler()
        {
            DateTime time = DateTime.Now.Date.Add(SelectedTime);
            
            await DrinkService.InsertDrink(new Drink(Guid.NewGuid(), time, SelectedDrink.DrinkID));
            WeakReferenceMessenger.Default.Send(new Messages.ReloadActionsMessage(0));

            await MopupService.Instance.PopAsync();
        }

        private bool CanSave()
        {
            return SelectedTime <= DateTime.Now.TimeOfDay && SelectedDrink != null;
        }

        private async void SetDrinkHandler(object selectedDrink)
        {
            SelectedDrink = (DrinkProperty)selectedDrink;
        }

        private void RefreshCans()
        {
            ((Command)SaveDrink).ChangeCanExecute();
        }
    }
}
