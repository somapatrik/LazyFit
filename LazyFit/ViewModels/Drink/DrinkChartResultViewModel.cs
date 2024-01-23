using LazyFit.Classes;
using LazyFit.Models.Drinks;
using LazyFit.Services;
using Microcharts;
using SkiaSharp;

namespace LazyFit.ViewModels.DrinkViewModels
{
    internal class DrinkChartResultViewModel : ResultComponent
    {
        private Chart _DrinkChart;

        public Chart DrinkChart { get => _DrinkChart; set => SetProperty(ref _DrinkChart, value); }

        public DrinkChartResultViewModel() { }
        protected override async void LoadResults()
        {
            List<Drink> drinks = await DB.GetDrinks(FirstDateTime, LastDateTime, true);
            DataExists = drinks.Any();
            List<ChartEntry> entries = new List<ChartEntry>();

            if (DataExists)
            {
                var drinkTypeCounts = drinks
                   .GroupBy(f => f.TypeOfDrink)
                   .Select(group => new
                   {
                       TypeOfDrink = group.Key,
                       DisplayName = group.First().Property.DisplayName,
                       Count = group.Count()
                   });

                foreach (var drinkGroup in drinkTypeCounts)
                {
                    SKColor color = SKColors.Gray;

                    if (drinkGroup.TypeOfDrink == TypeOfDrink.Coffee)
                        color = SKColor.Parse("#187ccf");
                    else if (drinkGroup.TypeOfDrink == TypeOfDrink.Sweet)
                        color = SKColors.LimeGreen;
                    else if (drinkGroup.TypeOfDrink == TypeOfDrink.Tea)
                        color = SKColors.DeepPink;
                    else if (drinkGroup.TypeOfDrink == TypeOfDrink.Beer)
                        color = SKColors.Brown;
                    else if (drinkGroup.TypeOfDrink == TypeOfDrink.Alcoholic)
                        color = SKColors.Brown;
                    else if (drinkGroup.TypeOfDrink == TypeOfDrink.Water)
                        color = SKColors.Brown;


                    entries.Add(new ChartEntry(drinkGroup.Count)
                    {
                        Color = color,
                        Label = drinkGroup.DisplayName,
                        ValueLabel = drinkGroup.Count.ToString()
                    });
                }
            }
            else
            {
                var properties = await DB.GetDrinkProperties();
                properties.ForEach(p => entries.Add(new ChartEntry(0) { Label = p.DisplayName }));
            }


            DrinkChart = new RadarChart()
            {
                Entries = entries,
                LabelTextSize = 40,
                LineSize = 6
            };
        }
    }
}
