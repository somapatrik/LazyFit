using CommunityToolkit.Maui.Views;
using LazyFit.Pages;
using LazyFit.Views;
using Mopups.Services;
using System.Windows.Input;

namespace LazyFit.ViewModels
{
    internal class FastingViewModel : PrimeViewModel
    {

        public ICommand OpenFasting { get; private set; }

        public FastingViewModel() 
        {
            RelayCommands();
        }

        private void RelayCommands()
        {
            OpenFasting = new Command(OpenFastingStart);
        }

        private async void OpenFastingStart()
        {
            await MopupService.Instance.PushAsync(new StartFastingView());
        }

    }
}
