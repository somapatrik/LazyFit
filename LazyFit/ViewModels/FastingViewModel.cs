
using LazyFit.Models;
using LazyFit.Services;
using LazyFit.Views;
using Mopups.Services;
using System.Windows.Input;

namespace LazyFit.ViewModels
{
    internal class FastingViewModel : PrimeViewModel
    {

        private bool _isFastActive;
        public bool isFastActive { get => _isFastActive;set => SetProperty(ref _isFastActive,value); }

        private Fast _ActiveFast;
        public Fast ActiveFast { get => _ActiveFast; set => SetProperty(ref _ActiveFast,value); }

        public ICommand OpenFasting { get; private set; }

        public FastingViewModel() 
        {
            RelayCommands();
            RefreshFastData();
        }

        private void RelayCommands()
        {
            OpenFasting = new Command(OpenFastingStart);
        }

        private async void OpenFastingStart()
        {
            var startView = new StartFastingView();
            startView.NewFastStarted += StartView_NewFastStarted;

            await MopupService.Instance.PushAsync(startView);
        }

        private void StartView_NewFastStarted(object sender, EventArgs e)
        {
            RefreshFastData();
        }

        private async void RefreshFastData()
        {
            ActiveFast = await DB.GetRunningFast();
            isFastActive = ActiveFast != null;
        }
    }
}
