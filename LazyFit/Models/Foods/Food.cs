using SQLite;

namespace LazyFit.Models.Foods
{

    public class Food : LogAction
    {
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
