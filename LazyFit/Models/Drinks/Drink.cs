using SQLite;

namespace LazyFit.Models.Drinks
{
    public class Drink
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public DateTime Time { get; set; }
        public TypeOfDrink TypeOfDrink { get; set; }
        [Ignore]
        public DrinkProperty Property { get; set; }

        public Drink() { }

        public Drink(Guid id, TypeOfDrink typeOfDrink)
        {
            Id = id;
            Time = DateTime.Now;
            TypeOfDrink = typeOfDrink;
        }

        public Drink(Guid id, DateTime time, TypeOfDrink typeOfDrink) 
        {
            Id = id;
            Time = time;
            TypeOfDrink = typeOfDrink;
        }
    }
}
