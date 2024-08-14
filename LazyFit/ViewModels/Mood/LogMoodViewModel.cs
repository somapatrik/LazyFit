using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Java.Lang;
using Java.Time.Temporal;
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

        public LogMoodViewModel() 
        {
            LoadMoods();
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
