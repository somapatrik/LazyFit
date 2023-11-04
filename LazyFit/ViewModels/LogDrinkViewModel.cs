using LazyFit.Models.Drinks;
using LazyFit.Services;
using System.Collections.ObjectModel;

namespace LazyFit.ViewModels
{
    class LogDrinkViewModel : PrimeViewModel
    {
        private ObservableCollection<DrinkProperty> _Drinks;
        public ObservableCollection<DrinkProperty> Drinks { get => _Drinks; set => SetProperty(ref _Drinks, value); }

        public LogDrinkViewModel() 
        {
            LoadDrinks();
        }

        private async void LoadDrinks()
        {
            Drinks = new ObservableCollection<DrinkProperty>();

            var allDrinks = await DB.GetDrinkProperties();
            allDrinks.ForEach(Drinks.Add);
        }
    }
}
