using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Models.Pressure;
using LazyFit.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LazyFit.ViewModels
{
    public class MainViewModel : PrimeViewModel
    {
        private ObservableCollection<BloodPressure> _BloodPressures;

        public ICommand OpenX => new Command(async () => await Browser.Default.OpenAsync("https://www.x.com/lazyfitapp", BrowserLaunchMode.SystemPreferred));
        public ICommand OpenResults => new Command(async () => await Shell.Current.Navigation.PushModalAsync(new ResultsPage()));

        public ObservableCollection<BloodPressure> BloodPressures 
        {
            get => _BloodPressures; 
            set => SetProperty(ref _BloodPressures, value); 
        }

        public string AppVersion  => AppInfo.VersionString;

        public MainViewModel() 
        {

        }

        



    }
}
