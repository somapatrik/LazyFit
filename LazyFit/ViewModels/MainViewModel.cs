using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LazyFit.Models.Pressure;
using LazyFit.Views;
using LazyFit.Views.Administration;
using Mopups.Services;
using System.Collections.ObjectModel;

namespace LazyFit.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<BloodPressure> _BloodPressures;

        public string AppVersion  => AppInfo.VersionString;

        public MainViewModel() 
        {

        }

        [RelayCommand]
        private async Task OpenResults()
        {
            await Shell.Current.Navigation.PushModalAsync(new ResultsPage());
        }

        [RelayCommand]
        private async Task OpenAbout()
        {
            await Shell.Current.Navigation.PushAsync(new AboutPage());
        }

        [RelayCommand]
        private async Task OpenLogButtons()
        {
            await MopupService.Instance.PushAsync(new LogButtonsView());
        }



    }
}
