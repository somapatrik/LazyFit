
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Messages;
using LazyFit.Models;
using LazyFit.Services;

namespace LazyFit.ViewModels.Fasting
{
    public partial class ActiveFastViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _IsRunning;

        [ObservableProperty]
        private Fast _ActiveFast;

        [ObservableProperty]
        private TimeSpan _EndSpan;

        [ObservableProperty]
        private TimeSpan _StartSpan;

        [ObservableProperty]
        private double _Progress;

        private Timer _Timer;

        public ActiveFastViewModel() 
        {
            _Timer = new System.Threading.Timer(TimerRefresh, null, Timeout.Infinite,Timeout.Infinite);

            LoadData();
            WireMessages();
        }

        private void TimerRefresh(object state)
        {
            EndSpan = ActiveFast.GetTimeSpanUntilEnd();
            StartSpan = ActiveFast.GetTimeSpanSinceStart(DateTime.Now); 
            Progress = (ActiveFast.GetElapsedTimePercentage(DateTime.Now)/100);
        }

        private void StartTimer()
        {
            _Timer.Change(0, 1000);
        }

        private void StopTimer()
        {
            _Timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void WireMessages()
        {
            WeakReferenceMessenger.Default.Register<StartFastMessage>(this, (a, b) => { LoadData(); });
            WeakReferenceMessenger.Default.Register<EndFastMessage>(this, (a, b) => { LoadData(); });
        }

        private async void LoadData()
        {
            await LoadActiveFast();
        }

        private async Task LoadActiveFast()
        {
            ActiveFast = await FastService.GetRunningFast();
            IsRunning = ActiveFast != null;
            if (IsRunning)
                StartTimer();
            else
                StopTimer();

        }
    }
}
