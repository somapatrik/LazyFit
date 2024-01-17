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

        private bool _CanSave = false;

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
            fastSpan = FinishedFast.GetTimeSpanSinceStart();
            planSpan = FinishedFast.GetPlannedEnd() - FinishedFast.StartTime;
        }

        private async void SaveEditsHandler()
        {
            var fast = FinishedFast;
            await DB.UpdateFast(fast);
            LoadFast(fast.Id);
        }

        private bool CanSaveEdit()
        {
            return true;
        }
    }
}
