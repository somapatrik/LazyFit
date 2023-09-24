using LazyFit.Classes;
using LazyFit.Models;
using LazyFit.Services;
using LazyFit.ViewModels.Classes;
using Microcharts;
using SkiaSharp;

namespace LazyFit.ViewModels
{

    class FastChartViewModel : ResultComponent
    {
        private Chart _FastChart;
        public Chart FastChart { get => _FastChart; set => SetProperty(ref _FastChart,value); }

        public FastChartViewModel() 
        {
            LoadResults();
        }

        protected override async void LoadResults()
        {
            List<Fast> fasts = await DB.GetFastsByPage(PageNumber);

            List<DateInt> dateInts = fasts.GroupBy(obj => obj.StartTime.Date)
                                            .Select(group => 
                                            new DateInt()
                                            {
                                                Date = group.Key,
                                                Value = (int)group.Sum(obj => ((TimeSpan)(obj.EndTime-obj.StartTime)).TotalHours)
                                            })
                                            .ToList();


            FastChart = new BarChart() { Entries = CreateEntries(PageNumber, dateInts) };

        }

        private List<ChartEntry> CreateEntries(int pageNum, List<DateInt> dateInts)
        {
            DateTime now = DateTime.Now.AddMonths(pageNum);
            DateTime firstDate = new DateTime(now.Year, now.Month, 1);
            DateTime lastDate = firstDate.AddMonths(1).AddDays(-1);
            DateTime actDate = firstDate;

            List<ChartEntry> entries = new List<ChartEntry>();

            for (int i = firstDate.Day; i <= lastDate.Day; i++)
            {
                DateInt found = dateInts.FirstOrDefault(x => x.Date.Date == actDate.Date);
                
                if (found != null)
                    entries.Add(new ChartEntry(found.Value) { Label = i.ToString(), Color = SKColors.LimeGreen });
                else
                    entries.Add(new ChartEntry(0) { Label = i.ToString() });
    
                actDate = actDate.AddDays(1);
            }
            return entries;
        }
    }
}
