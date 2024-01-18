using System.Windows.Input;
using LazyFit.Models;
using LazyFit.Services;

namespace LazyFit.ViewModels.Fasting
{
    internal class FastReportViewModel : PrimeViewModel
    {
        private Fast _FinishedFast;
        private TimeSpan _fastSpan;
        private TimeSpan _planSpan;
        private DateTime _StartDate;
        private DateTime _EndDate;
        private TimeSpan _StartTime;
        private TimeSpan _EndTime;

        public Fast FinishedFast 
        { 
            get => _FinishedFast; 
            set => SetProperty(ref _FinishedFast, value); 
        }
        public TimeSpan fastSpan 
        { 
            get => _fastSpan; 
            set => SetProperty(ref _fastSpan,value); 
        }
        public TimeSpan planSpan 
        { 
            get => _planSpan; 
            set => SetProperty(ref _planSpan, value); 
        }

        public DateTime StartDate 
        { 
            get => _StartDate;
            set
            {
                SetProperty(ref _StartDate, value);
                RefreshCan();
            }
        }
        public DateTime EndDate 
        { 
            get => _EndDate; 
            set 
            {
                SetProperty(ref _EndDate, value);
                RefreshCan();
            }
}

        public TimeSpan StartTime
        {
            get => _StartTime;
            set
            {
                SetProperty(ref _StartTime, value);
                RefreshCan();
            }
        }

        public TimeSpan EndTime
        {
            get => _EndTime;
            set
            {
                SetProperty(ref _EndTime, value);
                RefreshCan();
            }
        }

        public string ResultTitle { get => _ResultTitle; set => SetProperty(ref _ResultTitle, value); }

        private bool _EnableEdit;
        private string _ResultTitle;

        private string _GoodTitle
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
        private string _BadTitle
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

        public ICommand DeleteFast { private set; get; }
        public ICommand SaveEdits { private set; get; }

        public FastReportViewModel(Guid fastId) 
        {
            LoadFast(fastId);

            DeleteFast = new Command(async () => {
                if (await Shell.Current.DisplayAlert("Delete this fast", "Are you sure?", "Delete", "Cancel"))
                {
                    await DB.DeleteItem(FinishedFast);
                    await Shell.Current.Navigation.PopAsync();
                }
            
            });

            SaveEdits = new Command(SaveEditsHandler, CanSaveEdit);
        }

        private async void LoadFast(Guid fastId)
        {
            FinishedFast = await DB.GetFast(fastId);

            fastSpan = (DateTime)FinishedFast.EndTime - FinishedFast.StartTime;
            planSpan = FinishedFast.GetPlannedEnd() - FinishedFast.StartTime;

            StartDate = FinishedFast.StartTime;
            StartTime = FinishedFast.StartTime.TimeOfDay;

            var et = (DateTime)FinishedFast.EndTime;
            EndDate = et;
            EndTime = et.TimeOfDay;

            SetTitle();

            _EnableEdit = true;
            
        }

        private void SetTitle()
        {
            if (FinishedFast.Completed)
                ResultTitle = _GoodTitle;
            else
                ResultTitle = _BadTitle;
        }

        private async void SaveEditsHandler()
        {
            var fast = FinishedFast;

            var st = StartDate.Date.AddTicks(StartTime.Ticks);
            var end = EndDate.Date.AddTicks(EndTime.Ticks);
            fast.ChangeDates(st,end);

            await DB.UpdateFast(fast);
            LoadFast(fast.Id);
        }

        private bool CanSaveEdit()
        {
            var st = StartDate.Date.AddTicks(StartTime.Ticks);
            var end = EndDate.Date.AddTicks(EndTime.Ticks);

            bool checkDates = end >= st;
            return _EnableEdit && checkDates;
        }

        private void RefreshCan()
        {
            ((Command)SaveEdits).ChangeCanExecute();
        }
    }
}
