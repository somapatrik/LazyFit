using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyFit.Models
{
    public class Drink
    {
        public Guid Id { get; set; }
        public DateTime Time { get; set; }
        public DrinkType.TypeOfDrink TypeOfDrink { get; set; }

        public Drink() { }

        public Drink(Guid id, DrinkType.TypeOfDrink typeOfDrink)
        {
            Id = id;
            Time = DateTime.Now;
            TypeOfDrink = typeOfDrink;
        }
    }
}
