using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Classes;
using LazyFit.Models;
using LazyFit.Services;
using Microcharts;
using SkiaSharp;

namespace LazyFit.ViewModels
{
    internal class WeightChartResultViewModel : ResultComponent
    {

        private Chart _WeightChart;
        public Chart WeightChart { get => _WeightChart; set => SetProperty(ref _WeightChart,value); }

        public WeightChartResultViewModel()
        {
            LoadResults();
            WeakReferenceMessenger.Default.Register<Messages.ShowPageMessage>(this, (r, m) => { ShowPage(m.Value); });
        }

        protected override async void LoadResults()
        {
            List<Weight> weights = (await DB.GetWeightByPagePerWeek(PageNumber)).OrderBy(w=>w.Time).ToList();


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
            // Find week
            DateTime today = DateTime.Today.AddDays(7 * pageNum);
            int dayofWeek = today.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)today.DayOfWeek;

            DateTime monday = today.AddDays(-(dayofWeek - 1));
            DateTime sunday = monday.AddDays(6);
            DateTime actDate = monday;

            // Get last weight before this
            Weight latestWeight = await DB.GetLastWeightOlderThan(monday);
            float lastWeight = latestWeight == null ? 0 : (float)latestWeight.WeightValue;

            List<ChartEntry> entries = new List<ChartEntry>();

            while (actDate <= sunday)
            {
                DateFloat found = dateFloats.FirstOrDefault(x => x.Date.Date == actDate.Date);
                int i = actDate.Day;

                if (found != null)
                {
                    entries.Add(new ChartEntry(found.Value) { Label = i.ToString(), ValueLabel = found.Value.ToString(), Color = SKColors.OrangeRed});
                    lastWeight = found.Value;
                }
                else
                {
                    // Future will show 0
                    // Past should show last know weight
                    float displayValue = DateTime.Today < actDate ? 0 : lastWeight;
                    entries.Add(new ChartEntry(displayValue) { Label = i.ToString(), TextColor = SKColors.Transparent, Color = SKColors.OrangeRed });
                }

                actDate = actDate.AddDays(1);
            };

            return entries;
        }
    }
}
