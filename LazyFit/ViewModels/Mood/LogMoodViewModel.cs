using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Java.Lang;
using Java.Time.Temporal;
using LazyFit.Classes;
using LazyFit.Models.Moods;
using LazyFit.Services;

namespace LazyFit.ViewModels.MoodViewModels
{
    public partial class LogMoodViewModel : ObservableObject
    {

        [ObservableProperty]
        private List<MoodProperty> _moods;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveMoodCommand))]
        private MoodProperty _selectedMood;

        [ObservableProperty]
        private List<DateString> _dates;

        [ObservableProperty]
        private DateString _selectedDate;

        public LogMoodViewModel() 
        {
            LoadDates();
            LoadMoods();
        }

        private void LoadDates()
        {
            Dates = [
                new DateString("Yesterday", DateTime.Now.AddDays(-1), "left.png"), 
                new DateString("Today", DateTime.Now, "down.png"),
                ];
            SelectedDate = Dates[1];

        }

        private async void LoadMoods()
        {
            //Moods = new List<MoodProperty>();
            Moods = await MoodService.GetAllMoodProperties();
            Moods = Moods.OrderByDescending(m => m.MoodID).ToList();
               
        }

        [RelayCommand(CanExecute = nameof(CanSave))]
        private async Task SaveMood()
        {
            var i = 0;
        }

        private bool CanSave()
        {
            return SelectedMood != null;
        }


    }
}
