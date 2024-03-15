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
            // Load data
            List<Drink> drinks = await DrinkService.GetDrinks(FirstDateTime, LastDateTime, true);
            DataExists = drinks.Any();
            List<ChartEntry> entries = new List<ChartEntry>();

            // Empty chart
            var properties = await DrinkService.GetDrinkProperties();
            properties.ForEach(p => entries.Add(new ChartEntry(0) { Label = p.DisplayName, Color = SKColors.Gray }));

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
                        color = SKColors.Brown;
                    else if (drinkGroup.TypeOfDrink == TypeOfDrink.Sweet)
                        color = SKColors.DeepPink;
                    else if (drinkGroup.TypeOfDrink == TypeOfDrink.Tea)
                        color = SKColors.GreenYellow;
                    else if (drinkGroup.TypeOfDrink == TypeOfDrink.Beer)
                        color = SKColors.Gold;
                    else if (drinkGroup.TypeOfDrink == TypeOfDrink.Alcoholic)
                        color = SKColors.Red;
                    else if (drinkGroup.TypeOfDrink == TypeOfDrink.Water)
                        color = SKColor.Parse("#187ccf");


                    var entryFound = entries.FirstOrDefault(e => e.Label == drinkGroup.DisplayName);

                    if (entryFound != null)
                    {
                        entries.Remove(entryFound);

                        entries.Add(new ChartEntry(drinkGroup.Count)
                        {
                            Color = color,
                            Label = drinkGroup.DisplayName,
                            ValueLabel = drinkGroup.Count.ToString()
                        });
                    }
                }
            }

            DrinkChart = new RadarChart()
            {
                Entries = entries.OrderBy(x=>x.Label),
                LabelTextSize = 40,
                LineSize = 6
            };
        }
    }
}
