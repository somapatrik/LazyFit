using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LazyFit.Views;
using Mopups.Services;
using System.Windows.Input;

namespace LazyFit.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private DateTime _FromDate;

        [ObservableProperty]
        private DateTime _ToDate;

        public string AppVersion  => AppInfo.VersionString;

        public MainViewModel() 
        {
            FromDate = DateTime.Now.AddDays(-1);
            ToDate = DateTime.Now.AddDays(-2);
        }

        [RelayCommand]
        private async Task OpenLogButtons()
        {
            await MopupService.Instance.PushAsync(new LogButtonsView());
        }

        [RelayCommand]
        private void SetDates()
        {
            FromDate = FromDate.AddDays(-1);
            ToDate = ToDate.AddDays(-1);
        }



    }
}
