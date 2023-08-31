using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyFit.Models
{
    public class Mood
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public DateTime Time { get; set; }
        public MoodType.MoodName MoodName { get; set; }

        public Mood() { }

        public Mood(Guid id, MoodType.MoodName moodName)
        {
            Id = id;
            Time = DateTime.Now;
            MoodName = moodName;
        }   
    }
}
