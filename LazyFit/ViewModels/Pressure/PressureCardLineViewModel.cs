using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Messages;
using LazyFit.Models.Pressure;
using LazyFit.Services;
using System.Collections.ObjectModel;

namespace LazyFit.ViewModels.Pressure
{
    internal class PressureCardLineViewModel : PrimeViewModel
    {
        private ObservableCollection<BloodPressure> _BloodPressures;
        public ObservableCollection<BloodPressure> BloodPressures { get => _BloodPressures; private set => SetProperty(ref _BloodPressures, value); }

        PressureService PressureService;

        public PressureCardLineViewModel() 
        {
            PressureService = new PressureService();

            LoadPressure();
            WeakReferenceMessenger.Default.Register<RefreshPressureCards>(this, (a, b) => { LoadPressure(); });
        }
        private async void LoadPressure()
        {
            BloodPressures = new ObservableCollection<BloodPressure>();
            (await PressureService.GetLastPressures(10)).ForEach(BloodPressures.Add);
        }
    }
}
