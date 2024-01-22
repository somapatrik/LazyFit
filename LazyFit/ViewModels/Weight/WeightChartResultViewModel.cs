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
        private bool _DataExists;
        private decimal _MinWeight;
        private decimal _AvgWeight;
        private decimal _MaxWeight;
        private int _Improved;
        private decimal _ImprovedValue;
        private bool _PreviousDataExists;

        public bool DataExists { get => _DataExists; set => SetProperty(ref _DataExists, value); }

        public decimal MinWeight { get => _MinWeight; set => SetProperty(ref _MinWeight, Math.Round(value, 1)); }

        public decimal AvgWeight { get => _AvgWeight; set => SetProperty(ref _AvgWeight, Math.Round(value, 1)); }

        public decimal MaxWeight { get => _MaxWeight; set => SetProperty(ref _MaxWeight, Math.Round(value, 1)); }

        public int Improved { get => _Improved; set=> SetProperty(ref _Improved, value); }
        public decimal ImprovedValue { get => _ImprovedValue; set => SetProperty(ref _ImprovedValue, Math.Round(value,1)); }
        public bool PreviousDataExists { get => _PreviousDataExists; set => SetProperty(ref _PreviousDataExists, value); }

        public WeightChartResultViewModel()
        {
            LoadResults();
            WeakReferenceMessenger.Default.Register<Messages.ShowPageMessage>(this, (r, m) => { ShowPage(m.Value); });
        }

        protected override async void LoadResults()
        {
            List<Weight> weights = await DB.GetWeights(FirstDateTime, LastDateTime);
            DataExists = weights.Any();

            List<Weight> previousWeights = await DB.GetWeights(PreviousFirstDate, PreviousLastDate);
            PreviousDataExists = previousWeights.Any();

            List<ChartEntry> entries = new List<ChartEntry>();

            MinWeight = AvgWeight = MaxWeight = 0;
            Improved = 0;

            if (DataExists)
            {
                MinWeight = weights.Min(w => w.WeightValue);
                AvgWeight = weights.Average(w => w.WeightValue);
                MaxWeight = weights.Max(w => w.WeightValue);

                
                if (PreviousDataExists)
                {
                    var prevAvg = previousWeights.Average(w=>w.WeightValue);
                    var improvedVal = prevAvg - AvgWeight;
                    ImprovedValue = improvedVal * (-1);

                    if (improvedVal > 0)
                        Improved = 1;
                    else if (improvedVal < 0)
                        Improved = -1;
                    else
                        Improved = 0;

                }

            }
            else
            {
                PreviousDataExists = false;
            }

        }

    }
}
