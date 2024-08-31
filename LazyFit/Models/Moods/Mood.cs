using SQLite;

namespace LazyFit.Models.Moods
{
    public class Mood
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public DateTime Time { get; set; }
        public MoodName TypeOfMood { get; set; }

        [Ignore]
        public MoodProperty Property { get; set; }

        public Mood() { }

        public Mood(Guid id, MoodName typeofmood)
        {
            Id = id;
            Time = DateTime.Now;
            TypeOfMood = typeofmood;
        }

        public Mood(MoodName typeOfMood,DateTime time)
        {
            Id = Guid.NewGuid();
            Time = time;
            TypeOfMood = typeOfMood;
        }
    }
}
