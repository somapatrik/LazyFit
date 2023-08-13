using CommunityToolkit.Maui.Views;
using LazyFit.Pages;
using LazyFit.Views;
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
            var startPopup = new FastOptionsPage();
            await Shell.Current.Navigation.PushModalAsync(startPopup);
            var t = true;
        }

    }
}
