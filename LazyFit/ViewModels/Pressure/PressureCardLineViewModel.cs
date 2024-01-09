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

        public PressureCardLineViewModel() 
        {
            LoadPressure();
            WeakReferenceMessenger.Default.Register<RefreshPressureCards>(this, (a, b) => { LoadPressure(); });
        }
        private async void LoadPressure()
        {
            BloodPressures = new ObservableCollection<BloodPressure>();
            (await DB.GetPressures(DateTime.Now.Date.AddYears(-1), DateTime.Now)).ForEach(BloodPressures.Add);
        }
    }
}
