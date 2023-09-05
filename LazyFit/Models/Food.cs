using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyFit.Models
{

    public class Food
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public DateTime Time { get; set; }
        public FoodType.TypeOfFood TypeOfFood { get; set; }

        public Food() { }

        public Food(Guid id, FoodType.TypeOfFood typeOfFood)
        {
            Id = id;
            Time = DateTime.Now;
            TypeOfFood = typeOfFood;
        }
    }
}
