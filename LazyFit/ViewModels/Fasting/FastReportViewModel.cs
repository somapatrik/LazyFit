using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LazyFit.Models;
using LazyFit.Services;
using Mopups.Droid.Implementation;
using Mopups.PreBaked.Services;
using Mopups.Services;

namespace LazyFit.ViewModels.Fasting
{
    internal class FastReportViewModel : PrimeViewModel
    {

        private Fast _FinishedFast;

        public Fast FinishedFast { get => _FinishedFast; set => SetProperty(ref _FinishedFast, value); }

        private TimeSpan _fastSpan;
        private TimeSpan _planSpan;
        public TimeSpan fastSpan { get => _fastSpan; set => SetProperty(ref _fastSpan,value); }

        public TimeSpan planSpan { get => _planSpan; set => SetProperty(ref _planSpan, value); }

        public string GoodTitle
        {
            get
            {
                List<string> list = new List<string>()
                {
                    "Master Faster!",
                    "Fasting Champion!",
                    "Fastastic Job!",
                    "You've Fasted Triumphantly!",
                    "Fast and Fabulous!",
                    "Speed of Light Fasting!",
                    "Fast-tastic Victory!",
                    "Hunger Conqueror!",
                    "Fasting Maestro!",
                    "Fastastic Achievement!",
                };
                Random rnd = new Random();
                return list[rnd.Next(list.Count - 1)];
            }
        }

        public string BadTitle
        {
            get
            {
                List<string> list = new List<string>()
                {
                    "Total fail!",
                    "Busted!",
                    "Game over",
                    "Oh sh*t",
                    "Was it that hard?",
                    "Hunger won today!",
                    "Fail!",
                    "Disappointment!",
                    "Total letdown!",
                    "Defeated!",
                    "Heartbreaking fail!"
                };
                Random rnd = new Random();
                return list[rnd.Next(list.Count - 1)].ToUpper();
            }
        }

        public ICommand ChangeStart;

        public FastReportViewModel(Guid fastId) 
        {
            LoadFast(fastId);
            LoadData();

        }

        private async void LoadFast(Guid fastId)
        {
            FinishedFast = await DB.GetFast(fastId);
            fastSpan = FinishedFast.GetTimeSpanSinceStart();
            planSpan = FinishedFast.GetPlannedEnd() - FinishedFast.StartTime;


        }

        private void LoadData()
        {
            
        }
    }
}
