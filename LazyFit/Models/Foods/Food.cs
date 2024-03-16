using SQLite;

namespace LazyFit.Models.Foods
{

    public class Food : LogAction
    {
        //[PrimaryKey]
        //public Guid Id { get; set; }
        //public DateTime Time { get; set; }
        public TypeOfFood TypeOfFood { get; set; }

        [Ignore]
        public FoodProperty Property { get; set; }
        public Food() { }

        public Food(Guid id, TypeOfFood typeOfFood)
        {
            Id = id;
            Time = DateTime.Now;
            TypeOfFood = typeOfFood;
        }

        public Food(Guid id, DateTime time, TypeOfFood typeOfFood)
        {
            Id = id;
            Time = time;
            TypeOfFood = typeOfFood;
        }
    }
}
