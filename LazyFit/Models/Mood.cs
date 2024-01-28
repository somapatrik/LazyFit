using SQLite;

namespace LazyFit.Models
{
    public class Mood
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public DateTime Time { get; set; }
        public MoodName TypeOfMood { get; set; }

        public Mood() { }

        public Mood(Guid id, MoodName typeofmood)
        {
            Id = id;
            Time = DateTime.Now;
            TypeOfMood = typeofmood;
        }   
    }
}
