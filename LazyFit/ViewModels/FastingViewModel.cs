using CommunityToolkit.Maui.Views;
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
            var startPopup = new StartFastingView();
            await Shell.Current.ShowPopupAsync(startPopup);

        }

    }
}
