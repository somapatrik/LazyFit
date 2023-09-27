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
        public TypeOfDrink TypeOfDrink { get; set; }

        public Drink() { }

        public Drink(Guid id, TypeOfDrink typeOfDrink)
        {
            Id = id;
            Time = DateTime.Now;
            TypeOfDrink = typeOfDrink;
        }
    }
}
