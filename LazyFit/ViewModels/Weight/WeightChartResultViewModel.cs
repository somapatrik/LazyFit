using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Classes;
using LazyFit.Models;
using LazyFit.Services;
using Microcharts;
using SkiaSharp;

namespace LazyFit.ViewModels.Weight
{
    internal class WeightChartResultViewModel : ResultComponent
    {

        private Chart _WeightChart;
        public Chart WeightChart { get => _WeightChart; set => SetProperty(ref _WeightChart, value); }

        public WeightChartResultViewModel()
        {
            LoadResults();
            WeakReferenceMessenger.Default.Register<Messages.ShowPageMessage>(this, (r, m) => { ShowPage(m.Value); });
        }

        protected override async void LoadResults()
        {
            List<Weight> weights = (await DB.GetWeights(FirstDateTime, LastDateTime)).OrderBy(w => w.Time).ToList();


            List<DateFloat> dateFloats = weights.GroupBy(obj => obj.Time.Date)
                                           .Select(group =>
                                           new DateFloat()
                                           {
                                               Date = group.Key,
                                               Value = (float)group.Max(obj => obj.WeightValue)
                                           })
                                           .ToList();



            List<ChartEntry> entries = await CreateEntriesPerWeek(PageNumber, dateFloats);

            WeightChart = new LineChart()
            {
                Entries = entries,
                LabelTextSize = 36,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
            };
        }

        private async Task<List<ChartEntry>> CreateEntriesPerWeek(int pageNum, List<DateFloat> dateFloats)
        {
            // Get last weight before this
            Weight latestWeight = await DB.GetLastWeightOlderThan(FirstDateTime);
            float lastWeight = latestWeight == null ? 0 : (float)latestWeight.WeightValue;

            List<ChartEntry> entries = new List<ChartEntry>();

            DateTime actDate = FirstDateTime;
            while (actDate <= LastDateTime)
            {
                DateFloat found = dateFloats.FirstOrDefault(x => x.Date.Date == actDate.Date);
                int i = actDate.Day;

                if (found != null)
                {
                    entries.Add(new ChartEntry(found.Value) { Label = i.ToString(), ValueLabel = found.Value.ToString(), Color = SKColors.OrangeRed });
                    lastWeight = found.Value;
                }
                else
                {
                    float displayValue = lastWeight;

                    entries.Add(new ChartEntry(displayValue)
                    {
                        Label = i.ToString(),
                        ValueLabel = displayValue.ToString(),
                        ValueLabelColor = SKColor.Parse("#02444444"),
                        Color = SKColors.OrangeRed
                    });
                }

                actDate = actDate.AddDays(1);
            };

            return entries;
        }
    }
}
