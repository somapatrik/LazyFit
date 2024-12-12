
using LazyFit.Models.Moods;

namespace LazyFit.LocalData
{
    internal class LocalMoodPropertyData
    {

        private List<MoodProperty> _moodProperties;
        public List<MoodProperty> MoodProperties => _moodProperties;

        public LocalMoodPropertyData() 
        {
            _moodProperties = new List<MoodProperty>()
            {
                new MoodProperty()
                {
                    MoodID = MoodName.VeryGood,
                    DisplayName = "Very good",
                    Description = "Everything is great!",
                    ImageName = "very_happy.png"
                },
                new MoodProperty()
                {
                    MoodID = MoodName.Good,
                    DisplayName = "Good",
                    Description = "This is fine!",
                    ImageName = "happy.png"
                },
                new MoodProperty()
                {
                    MoodID = MoodName.Normal,
                    DisplayName = "Normal",
                    Description = "Not great, not terrible...",
                    ImageName = "neutral.png"
                },
                new MoodProperty()
                {
                    MoodID = MoodName.Bad,
                    DisplayName = "Bad",
                    Description = "Could be better",
                    ImageName = "angry.png"
                },
                new MoodProperty()
                {
                    MoodID = MoodName.VeryBad,
                    DisplayName = "Very bad",
                    Description = "F*ck this!",
                    ImageName = "cursing.png"
                },
            };
        }
    }
}
