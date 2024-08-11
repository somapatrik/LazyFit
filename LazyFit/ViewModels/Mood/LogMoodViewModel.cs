using CommunityToolkit.Mvvm.ComponentModel;
using LazyFit.Models.Moods;
using LazyFit.Services;

namespace LazyFit.ViewModels.MoodViewModels
{
    public partial class LogMoodViewModel : ObservableObject
    {

        [ObservableProperty]
        private List<MoodProperty> _moods;

        [ObservableProperty]
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
    }
}
