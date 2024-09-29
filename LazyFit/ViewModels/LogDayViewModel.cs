using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LazyFit.Views;
using Mopups.Services;

namespace LazyFit.ViewModels
{
    public partial class LogDayViewModel : ObservableObject
    {
        [ObservableProperty]
        private DateTime _ActionDate;

        public LogDayViewModel(DateTime actionDate)
        {
            ActionDate = actionDate;
        }

        [RelayCommand]
        private async Task ShowDrink()
        {
            await MopupService.Instance.PopAsync();
            await MopupService.Instance.PushAsync(new LogDrinkView(ActionDate));

            // Workaround: To be removed
            await Shell.Current.Navigation.PopToRootAsync();
        }

        [RelayCommand]
        private async Task ShowFood()
        {
            await MopupService.Instance.PopAsync();
            await MopupService.Instance.PushAsync(new LogFoodView(ActionDate));

            // Workaround: To be removed
            await Shell.Current.Navigation.PopToRootAsync();
        }
    }
}
