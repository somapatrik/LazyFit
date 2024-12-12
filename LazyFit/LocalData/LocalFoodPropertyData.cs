using LazyFit.Models.Foods;

namespace LazyFit.LocalData
{
    internal class LocalFoodPropertyData
    {
        private List<FoodProperty> _foodProperties;
        public List<FoodProperty> FoodProperties => _foodProperties;

        public LocalFoodPropertyData() 
        {
            _foodProperties = new List<FoodProperty>()
            {
                new FoodProperty()
                {
                    FoodId = TypeOfFood.Normal,
                    DisplayName = "Casual",
                    Description = "Ok food",
                    ImageName = "normalfood.png"
                },
                new FoodProperty()
                {
                    FoodId = TypeOfFood.Healthy,
                    DisplayName = "Healthy",
                    Description = "Like really healthy",
                    ImageName = "healthyfood.png"
                },
                new FoodProperty()
                {
                    FoodId = TypeOfFood.Unhealthy,
                    DisplayName = "Unhealthy",
                    Description = "Junk, tasty stuff",
                    ImageName = "hamburger.png"
                },
                new FoodProperty()
                {
                    FoodId = TypeOfFood.Snack,
                    DisplayName = "Snack",
                    Description = "Quick and bad",
                    ImageName = "snack.png"
                },
            };
        }
    }
}
