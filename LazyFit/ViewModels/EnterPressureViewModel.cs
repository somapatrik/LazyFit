using LazyFit.Models.Pressure;
using LazyFit.Services;
using Mopups.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LazyFit.ViewModels
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

        private int _Low;

        public int Low
        {
            get => _Low; set
            {
                SetProperty(ref _Low, value);
                RefreshCans();
            }
        }

        private int _High;

        public int High
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
           // ((Command)SavePressure).ChangeCanExecute();
        }


        public EnterPressureViewModel() 
        {
            SavePressure = new Command(SavePressureHandler);
            SetTimeNow = new Command(SetNow);
            SetNow();
        }

        private void SetNow()
        {
            SelectedTime = DateTime.Now.TimeOfDay;
        }

        private async void SavePressureHandler()
        {
            await DB.InsertPressure(new BloodPressure(Guid.NewGuid(), High, Low ));
            await MopupService.Instance.PopAsync();
        }
    }
}
