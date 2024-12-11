using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Models.Pressure;
using LazyFit.Services;
using LazyFit.Views.Pressure;
using Mopups.Services;
using System.Windows.Input;

namespace LazyFit.ViewModels.Pressure
{
    public class EnterPressureViewModel : PrimeViewModel
    {
        private TimeSpan _SelectedTime;

        public TimeSpan SelectedTime
        {
            get => _SelectedTime; set
            {
                SetProperty(ref _SelectedTime, value);
                RefreshCans();
            }
        }

        private string _Low;

        public string Low
        {
            get => _Low; set
            {
                SetProperty(ref _Low, value);
                RefreshCans();
            }
        }

        private string _High;

        public string High
        {
            get => _High; set
            {
                SetProperty(ref _High, value);
                RefreshCans();
            }
        }

        public ICommand SavePressure { private set; get; }
        public ICommand SetTimeNow { private set; get; }

        private void RefreshCans()
        {
            ((Command)SavePressure).ChangeCanExecute();
        }

        PressureService PressureService;
        public EnterPressureViewModel()
        {
            PressureService = new PressureService();

            SavePressure = new Command(SavePressureHandler, CanSave);
            SetTimeNow = new Command(SetNow);
            SetNow();
        }

        private void SetNow()
        {
            SelectedTime = DateTime.Now.TimeOfDay;
        }

        private async void SavePressureHandler()
        {
            var bloodPressure = new BloodPressure(Guid.NewGuid(), int.Parse(High), int.Parse(Low));

            await PressureService.InsertPressure(bloodPressure);
            await MopupService.Instance.PushAsync(new PressureDiagnose(bloodPressure));

            WeakReferenceMessenger.Default.Send(new Messages.RefreshPressureCards(true));
        }

        private bool CanSave()
        {
            int h;
            int l;

            return int.TryParse(High, out h) && h >= 0 && h <= 300 && int.TryParse(Low, out l) && l >= 0 && l <= 300;
        }
    }
}
