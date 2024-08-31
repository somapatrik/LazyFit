
using SQLite;

namespace LazyFit.Models.Moods
{
    public class MoodProperty
    {
        [PrimaryKey]
        public MoodName MoodID { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
    }
}
