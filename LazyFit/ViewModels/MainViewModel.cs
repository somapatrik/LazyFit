using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LazyFit.Models.Pressure;
using LazyFit.Views;
using Mopups.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LazyFit.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<BloodPressure> _BloodPressures;

        public ICommand OpenX => new Command(async () => await Browser.Default.OpenAsync("https://www.x.com/lazyfitapp", BrowserLaunchMode.SystemPreferred));
        public ICommand OpenResults => new Command(async () => await Shell.Current.Navigation.PushModalAsync(new ResultsPage()));

        public string AppVersion  => AppInfo.VersionString;

        public MainViewModel() 
        {

        }

        [RelayCommand]
        private async Task OpenLogButtons()
        {
            await MopupService.Instance.PushAsync(new LogButtonsView());
        }

        



    }
}
