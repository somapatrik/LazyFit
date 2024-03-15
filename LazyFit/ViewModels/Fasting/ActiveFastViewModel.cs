
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
            WeakReferenceMessenger.Default.Register<FastStartMessage>(this, (a, b) => { LoadData(); });
            WeakReferenceMessenger.Default.Register<FastEndMessage>(this, (a, b) => { LoadData(); });
            WeakReferenceMessenger.Default.Register<FastDeleteMessage>(this, (a, b) => { LoadData(); });
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

        [RelayCommand]
        private async Task DeleteFast()
        {
            if (await Shell.Current.DisplayAlert("Delete active fast", "", "Delete","Cancel"))
            {
                await FastService.DeleteFast(ActiveFast);
            }
        }

        [RelayCommand]
        private async Task FinishFast()
        {
            bool EndIt = false;

            // Display custom message for failed things
            if (DateTime.Now < ActiveFast.GetPlannedEnd())
                EndIt = await Shell.Current.DisplayAlert("Fail fast", "Would you like to FAIL this fast?", "STAY UNFIT", "no...sorry");
            else
                EndIt = await Shell.Current.DisplayAlert("Finish fast", "Good job. Would you like to finish this fast?", "Finish", "Cancel");

            if (EndIt)
            {
                await FastService.EndFast(ActiveFast);
            }
        }
    }
}
