using LazyFit.Classes;
using LazyFit.Models.Foods;
using LazyFit.Services;
using Microcharts;
using SkiaSharp;

namespace LazyFit.ViewModels.FoodViewModels
{
    internal class FoodChartResultViewModel : ResultComponent
    {
        private Chart _FoodChart;

        public Chart FoodChart { get => _FoodChart; set => SetProperty(ref _FoodChart, value); }

        public FoodChartResultViewModel()
        {
            
        }
        protected override async void LoadResults()
        {
            List<Food> foods = await DB.GetFoods(FirstDateTime, LastDateTime,true);
            DataExists = foods.Any();
            List<ChartEntry> entries = new List<ChartEntry>();

            if (DataExists) 
            {
                var foodTypeCounts = foods
                   .GroupBy(f => f.TypeOfFood)
                   .Select(group => new
                   {
                       TypeOfFood = group.Key,
                       DisplayName = group.First().Property.DisplayName,
                       Count = group.Count()
                   });

                foreach (var foodGroup in foodTypeCounts)
                {
                    SKColor color = SKColors.Gray;

                    if (foodGroup.TypeOfFood == TypeOfFood.Normal)
                        color = SKColor.Parse("#187ccf");
                    else if (foodGroup.TypeOfFood == TypeOfFood.Healthy)
                        color = SKColors.LimeGreen;
                    else if (foodGroup.TypeOfFood == TypeOfFood.Snack)
                        color = SKColors.DeepPink;
                    else if (foodGroup.TypeOfFood == TypeOfFood.Unhealthy)
                        color = SKColors.Brown;


                    entries.Add(new ChartEntry(foodGroup.Count) 
                    { 
                        Color = color, 
                        Label = foodGroup.DisplayName, 
                        ValueLabel = foodGroup.Count.ToString() 
                    });
                }
            }
            else
            {
                var properties = await DB.GetFoodProperties();
                properties.ForEach(p => entries.Add(new ChartEntry(0) {  Label = p.DisplayName}));
            }


            FoodChart = new RadarChart()
            {
                Entries = entries,
                LabelTextSize = 40,
                LineSize = 6
            };
        }
    }
}
