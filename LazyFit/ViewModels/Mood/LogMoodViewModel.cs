using CommunityToolkit.Mvvm.ComponentModel;
using LazyFit.Models.Moods;

namespace LazyFit.ViewModels.MoodViewModels
{
    public partial class LogMoodViewModel : ObservableObject
    {

        [ObservableProperty]
        private List<MoodProperty> _moods;

        public LogMoodViewModel() 
        {
            LoadMoods();
        }

        private void LoadMoods()
        {
            Moods = new List<MoodProperty>();
               
        }
    }
}
