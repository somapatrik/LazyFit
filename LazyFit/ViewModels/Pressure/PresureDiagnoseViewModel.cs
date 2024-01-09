using LazyFit.Models.Pressure;
using Mopups.Services;
using System.Windows.Input;

namespace LazyFit.ViewModels.Pressure
{
    class PresureDiagnoseViewModel : PrimeViewModel
    {
        private BloodPressure _Pressure;
        public BloodPressure Pressure { get => _Pressure; set => SetProperty(ref _Pressure, value); }

        public string PressureResult => Pressure.High + " / " + Pressure.Low;


        public ICommand CloseAll { private set; get; }

        public PresureDiagnoseViewModel(BloodPressure pressure)
        {
            Pressure = pressure;

            CloseAll = new Command(async () => await MopupService.Instance.PopAllAsync());
        }

    }
}
