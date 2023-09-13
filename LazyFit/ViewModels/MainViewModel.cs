using Microcharts;
using System.Windows.Input;

namespace LazyFit.ViewModels
{
    public class MainViewModel : PrimeViewModel
    {
        private Chart _mychart;
        public Chart mychart { get => _mychart; set => SetProperty(ref _mychart, value); }
        public ICommand OpenResults => new Command(async () => await Shell.Current.Navigation.PushModalAsync(new ResultsPage()));


        public MainViewModel() 
        {
            
        }



    }
}
