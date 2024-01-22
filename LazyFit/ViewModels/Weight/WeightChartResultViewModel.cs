using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Models.WeightModels;
using LazyFit.Classes;
using LazyFit.Services;
using Microcharts;
using SkiaSharp;

namespace LazyFit.ViewModels.WeightViewModels
{
    internal class WeightChartResultViewModel : ResultComponent
    {

        private Chart _WeightChart;
        private bool _DataExists;
        private decimal _MinWeight;
        private decimal _AvgWeight;
        private decimal _MaxWeight;

        public Chart WeightChart { get => _WeightChart; set => SetProperty(ref _WeightChart, value); }

        public bool DataExists { get => _DataExists; set => SetProperty(ref _DataExists, value); }

        public decimal MinWeight { get => _MinWeight; set => SetProperty(ref _MinWeight, Math.Round(value, 1)); }

        public decimal AvgWeight { get => _AvgWeight; set => SetProperty(ref _AvgWeight, Math.Round(value, 1)); }

        public decimal MaxWeight { get => _MaxWeight; set => SetProperty(ref _MaxWeight, Math.Round(value, 1)); }

        public WeightChartResultViewModel()
        {
            LoadResults();
            WeakReferenceMessenger.Default.Register<Messages.ShowPageMessage>(this, (r, m) => { ShowPage(m.Value); });
        }

        protected override async void LoadResults()
        {
            List<Weight> weights = (await DB.GetWeights(FirstDateTime, LastDateTime));//.OrderBy(w => w.Time).ToList();

            DataExists = weights.Any();
            List<ChartEntry> entries = new List<ChartEntry>();

            if (DataExists)
            {
                MinWeight = weights.Min(w => w.WeightValue);
                AvgWeight = weights.Average(w => w.WeightValue);
                MaxWeight = weights.Max(w => w.WeightValue);

                entries.Add(new ChartEntry((float)MinWeight) { Label = "Min", ValueLabel = MinWeight.ToString(), Color = SKColors.OrangeRed });
                entries.Add(new ChartEntry((float)MinWeight) { Label = "Avg", ValueLabel = AvgWeight.ToString(), Color = SKColors.OrangeRed });
                entries.Add(new ChartEntry((float)MinWeight) { Label = "Max", ValueLabel = MaxWeight.ToString(), Color = SKColors.OrangeRed });
            }
            else
            {
                entries.Add(new ChartEntry(0) { Label = "Min", ValueLabel = "0", Color = SKColors.OrangeRed });
                entries.Add(new ChartEntry(0) { Label = "Avg", ValueLabel = "0", Color = SKColors.OrangeRed });
                entries.Add(new ChartEntry(0) { Label = "Max", ValueLabel = "0", Color = SKColors.OrangeRed });
            }

            //List<DateFloat> dateFloats = weights.GroupBy(obj => obj.Time.Date)
            //                               .Select(group =>
            //                               new DateFloat()
            //                               {
            //                                   Date = group.Key,
            //                                   Value = (float)group.Max(obj => obj.WeightValue)
            //                               })
            //                               .ToList();



            // List<ChartEntry> entries = await CreateEntriesPerWeek(PageNumber, dateFloats);

            WeightChart = new LineChart()
            {
                Entries = entries,
                LabelTextSize = 36,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                PointMode = PointMode.None,
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
