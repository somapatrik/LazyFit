
using SQLite;

namespace LazyFit.Models.Moods
{
    public class MoodProperty
    {
        [PrimaryKey]
        public MoodName ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
    }
}
