using CommunityToolkit.Mvvm.Messaging;
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
            WeakReferenceMessenger.Default.Register<Messages.ShowPageMessage>(this, (r, m) =>{ ShowPage(m.Value);  });
        }

        protected override async void LoadResults()
        {
            List<Fast> fasts = await DB.GetFastsByPagePerWeek(PageNumber);

            List<DateInt> dateInts = fasts.GroupBy(obj => obj.StartTime.Date)
                                            .Select(group => 
                                            new DateInt()
                                            {
                                                Date = group.Key,
                                                Value = (int)group.Sum(obj => ((TimeSpan)(obj.EndTime-obj.StartTime)).TotalHours)
                                            })
                                            .ToList();


            FastChart = new BarChart() 
            { 
                Entries = CreateEntriesPerWeek(PageNumber, dateInts), 
                LabelTextSize = 36,
                LabelOrientation = Orientation.Horizontal, 
                ValueLabelOrientation=Orientation.Horizontal               
            };

        }

        private List<ChartEntry> CreateEntriesPerWeek(int pageNum, List<DateInt> dateInts)
        {
            DateTime today = DateTime.Today.AddDays(7 * pageNum);
            int dayofWeek = today.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)today.DayOfWeek;

            DateTime monday = today.AddDays(-(dayofWeek - 1));
            DateTime sunday = monday.AddDays(6);
            DateTime actDate = monday;



            List<ChartEntry> entries = new List<ChartEntry>();

            while (actDate <= sunday)
            {
                DateInt found = dateInts.FirstOrDefault(x => x.Date.Date == actDate.Date);
                int i = actDate.Day;
                if (found != null)
                    entries.Add(new ChartEntry(found.Value) { Label = i.ToString(), ValueLabel = found.Value.ToString(), Color = SKColor.Parse("#0b5ed7") });
                else
                    entries.Add(new ChartEntry(0) { Label = i.ToString(), TextColor = SKColors.LightGray, Color = SKColors.Transparent });

                actDate = actDate.AddDays(1);
            };

            return entries;
        }

        private List<ChartEntry> CreateEntries(int pageNum, List<DateInt> dateInts)
        {
            DateTime now = DateTime.Today.AddMonths(pageNum);
            DateTime firstDate = new DateTime(now.Year, now.Month, 1);
            DateTime lastDate = firstDate.AddMonths(1).AddDays(-1);
            DateTime actDate = firstDate;

            List<ChartEntry> entries = new List<ChartEntry>();

            for (int i = firstDate.Day; i <= lastDate.Day; i++)
            {
                DateInt found = dateInts.FirstOrDefault(x => x.Date.Date == actDate.Date);
                
                if (found != null)
                    entries.Add(new ChartEntry(found.Value) { Label = i.ToString(),  ValueLabel = found.Value.ToString(),Color = SKColors.LimeGreen });
                else
                    entries.Add(new ChartEntry(0) { Label = i.ToString(), TextColor = SKColors.SlateGray });
    
                actDate = actDate.AddDays(1);
            }
            return entries;
        }
    }
}
