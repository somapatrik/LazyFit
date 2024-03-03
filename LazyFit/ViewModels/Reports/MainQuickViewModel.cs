using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Messages;
using LazyFit.Services;

namespace LazyFit.ViewModels.Reports
{
    internal partial class MainQuickViewModel : ObservableObject
    {
        [ObservableProperty]
        private decimal _Weight;

        [ObservableProperty]
        private decimal _FastRatio;

        public MainQuickViewModel() 
        {
            LoadWeight();
            LoadFasts();

            WeakReferenceMessenger.Default.Register<NewWeightMessage>(this, (a, b) => LoadWeight());
        }

        private async void LoadFasts()
        {
            FastRatio = await FastService.GetFastFinishRatio(10);
        }

        private async void LoadWeight()
        {
            Weight = await WeightService.GetWeightMonthAvg(DateTime.Now,10);
        }
    }
}
