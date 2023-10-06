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

            int remainHours = 0;
            int enterValue = 0;

            List<ChartEntry> entries = new List<ChartEntry>();

            while (actDate <= sunday)
            {
                DateInt found = dateInts.FirstOrDefault(x => x.Date.Date == actDate.Date);
                int i = actDate.Day;

                if (found != null)
                    enterValue = remainHours + found.Value;
                else 
                    enterValue = remainHours;

                    remainHours = 0;

                    if (enterValue > 24)
                    {
                        remainHours = enterValue - 24;
                        enterValue = 24;
                    }

                    entries.Add(new ChartEntry(enterValue) { Label = i.ToString(), ValueLabel = enterValue.ToString(), Color = SKColor.Parse("#0b5ed7") });

                    actDate = actDate.AddDays(1);
            };

            return entries;
        }

    }
}
