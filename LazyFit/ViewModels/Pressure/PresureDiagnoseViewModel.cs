using LazyFit.Models.Pressure;
using LazyFit.Services;
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

        PressureService PressureService;

        public PresureDiagnoseViewModel(BloodPressure pressure)
        {
            PressureService = new PressureService();

            Pressure = pressure;

            CloseAll = new Command(async () => await MopupService.Instance.PopAllAsync());
        }

    }
}
