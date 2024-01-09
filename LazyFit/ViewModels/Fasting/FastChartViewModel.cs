using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Classes;
using LazyFit.Models;
using LazyFit.Services;
using LazyFit.ViewModels.Classes;
using Microcharts;
using SkiaSharp;

namespace LazyFit.ViewModels.Fasting
{

    class FastChartViewModel : ResultComponent
    {
        private Chart _FastChart;
        public Chart FastChart { get => _FastChart; set => SetProperty(ref _FastChart, value); }

        public FastChartViewModel()
        {
            LoadResults();
            WeakReferenceMessenger.Default.Register<Messages.ShowPageMessage>(this, (r, m) => { ShowPage(m.Value); });
        }

        protected override async void LoadResults()
        {
            List<Fast> fasts = await DB.GetFasts(FirstDateTime, LastDateTime);


            List<DateInt> hoursFasted = fasts.GroupBy(obj => obj.StartTime.Date)
                                             .Select(group => new DateInt()
                                             {
                                                 Date = group.Key,
                                                 Value = (int)group.Sum(obj => ((TimeSpan)(obj.EndTime - obj.StartTime)).TotalHours)
                                             }).ToList();


            List<DateInt> shouldFasted = fasts.GroupBy(obj => obj.StartTime.Date)
                                          .Select(group => new DateInt()
                                          {
                                              Date = group.Key,
                                              Value = (int)group.Sum(obj => (obj.GetPlannedEnd() - obj.StartTime).TotalHours)
                                          }).ToList();


            ChartSerie shoudlFastSerie = new ChartSerie()
            {
                Entries = CreateEntriesPerWeek(shouldFasted),
                Color = SKColor.Parse("#6c757d"),
                Name = "Fasting plan"
            };

            ChartSerie hoursFastedSerie = new ChartSerie()
            {
                Entries = CreateEntriesPerWeek(hoursFasted),
                Color = SKColor.Parse("#0b5ed7"),
                Name = "Hours starved"
            };


            FastChart = new BarChart()
            {
                Series = new List<ChartSerie> { hoursFastedSerie, shoudlFastSerie },
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                LabelTextSize = 36,
                ValueLabelTextSize = 36,
                SerieLabelTextSize = 36,
                LegendOption = SeriesLegendOption.Bottom,

            };

        }

        private List<ChartEntry> CreateEntriesPerWeek(List<DateInt> dateInts)
        {
            DateTime actDate = FirstDateTime;

            //int remainHours = 0;
            int enterValue = 0;

            List<ChartEntry> entries = new List<ChartEntry>();

            while (actDate <= LastDateTime)
            {
                DateInt found = dateInts.FirstOrDefault(x => x.Date.Date == actDate.Date);
                int i = actDate.Day;



                //if (found != null)
                //    enterValue = remainHours + found.Value;
                //else 
                //    enterValue = remainHours;

                //remainHours = 0;

                //if (enterValue > 24)
                //{
                //    remainHours = enterValue - 24;
                //    enterValue = 24;
                //}

                // Bar color
                //SKColor barColor = SKColor.Parse("#0b5ed7");
                //SKColor labelColor = SKColors.Gray;

                //if (enterValue == 0) 
                //{ 
                //    barColor = SKColors.Transparent;
                //    labelColor = SKColors.Transparent;
                //}


                //entries.Add(new ChartEntry(enterValue) { Label = i.ToString(), ValueLabel = enterValue.ToString(), Color = barColor, ValueLabelColor = labelColor });


                if (found == null)
                {
                    entries.Add(new ChartEntry(0) { Label = i.ToString() });
                }
                else
                {
                    entries.Add(new ChartEntry(found.Value) { Label = i.ToString(), ValueLabel = found.Value.ToString() });
                }



                actDate = actDate.AddDays(1);
            };

            return entries;
        }

    }
}
