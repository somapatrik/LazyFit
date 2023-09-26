using LazyFit.Models;
using LazyFit.Services;
using Microcharts;
using SkiaSharp;
using System.Windows.Input;

namespace LazyFit.ViewModels
{
    internal class WeightChartViewModel : PrimeViewModel
    {
        public ICommand ShowPrevious { get; private set; }
        public ICommand ShowNext { get; private set; }


        private Chart _chartWeight;
        public Chart ChartWeight { get => _chartWeight; set => SetProperty(ref _chartWeight, value); }


        private int _showResultsCount = 10;

        private int _currentPage;
        public int CurrentPage { get => _currentPage; set { SetProperty(ref _currentPage, value); RefreshCans(); } }


        private bool _noData;
        public bool NoData { get => _noData; set { SetProperty(ref _noData, value); RefreshCans(); } }



        public WeightChartViewModel() 
        {
            LoadChartData(CurrentPage, _showResultsCount);

            ShowPrevious = new Command(ShowPreviousHandler, CanShowPrevious);
            ShowNext = new Command(ShowNextHandler, CanShowNext);

        }

        private void ShowPreviousHandler(object obj)
        {
            CurrentPage++;
            LoadChartData(CurrentPage, _showResultsCount);
        }

        private void ShowNextHandler(object obj)
        {
            CurrentPage--;
            LoadChartData(CurrentPage, _showResultsCount);
        }

        private bool CanShowNext(object obj)
        {
            return CurrentPage > 0;
        }

        private bool CanShowPrevious(object obj)
        {
            return !NoData;
        }

        public async void LoadChartData(int pageNumber, int numberOfWeight)
        {
            List<Weight> weights = await DB.GetWeightPage(pageNumber, numberOfWeight);

            var entries = weights
                .OrderBy(w => w.Time)
                .Select(weight => new ChartEntry((float)weight.WeightValue)
                {
                    ValueLabel = weight.WeightValue.ToString(),
                    ValueLabelColor = SKColors.OrangeRed,
                    Label = weight.Time.ToString("d"),
                    OtherColor = SKColors.OrangeRed
                })
                .ToList();


            if (!entries.Any())
            {    
               entries.Add(new ChartEntry(0));
               NoData = true;
            }
            else
            {
                NoData = false;
            }
                

            List<ChartSerie> series = new List<ChartSerie>()
            {
                new ChartSerie() 
                { 
                    Entries = entries, 
                    Color = SKColors.OrangeRed,
                    Name="Weight"
                }
            };

            ChartWeight = new LineChart()
            {
                Series = series,
                IsAnimated = true,
                EnableYFadeOutGradient = false,
                LineSize = 7,
                PointSize = 20,
                ShowYAxisLines = false,
                LineMode = LineMode.Spline,
                ValueLabelTextSize = 36,
                LabelTextSize = 36,
                ValueLabelOrientation = Orientation.Horizontal,
                ValueLabelOption = ValueLabelOption.TopOfElement,
                LabelOrientation = Orientation.Vertical,
                
            };

        }

        private void RefreshCans()
        {
            ((Command)ShowNext).ChangeCanExecute();
            ((Command)ShowPrevious).ChangeCanExecute();
        }
    }
}
