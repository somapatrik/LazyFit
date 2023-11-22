using LazyFit.Views.Fasting;
using Microcharts;
using System.Windows.Input;

namespace LazyFit.ViewModels
{
    public class MainViewModel : PrimeViewModel
    {
        public ICommand OpenX => new Command(async () => await Browser.Default.OpenAsync("https://www.x.com/lazyfitapp", BrowserLaunchMode.SystemPreferred));
        public ICommand OpenResults => new Command(async () => await Shell.Current.Navigation.PushModalAsync(new ResultsPage()));

        public string AppVersion  => AppInfo.VersionString;

        public MainViewModel() 
        {

        }



    }
}
