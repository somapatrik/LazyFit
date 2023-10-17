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
            List<Fast> fasts = await DB.GetFasts(FirstDateTime, LastDateTime);

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
            DateTime actDate = FirstDateTime;

            int remainHours = 0;
            int enterValue = 0;

            List<ChartEntry> entries = new List<ChartEntry>();

            while (actDate <= LastDateTime)
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

                // Bar color
                SKColor barColor = SKColor.Parse("#0b5ed7");
                SKColor labelColor = SKColors.Gray;
                
                if (enterValue == 0) 
                { 
                    barColor = SKColors.Transparent;
                    labelColor = SKColors.Transparent;
                }


                entries.Add(new ChartEntry(enterValue) { Label = i.ToString(), ValueLabel = enterValue.ToString(), Color = barColor, ValueLabelColor = labelColor });

                actDate = actDate.AddDays(1);
            };

            return entries;
        }

    }
}
