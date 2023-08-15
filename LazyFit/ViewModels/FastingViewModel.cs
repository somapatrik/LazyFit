
using LazyFit.Models;
using LazyFit.Services;
using LazyFit.Views;
using Mopups.Services;
using System;
using System.Windows.Input;

namespace LazyFit.ViewModels
{
    internal class FastingViewModel : PrimeViewModel
    {

        private bool _isFastActive;
        public bool isFastActive { get => _isFastActive;set => SetProperty(ref _isFastActive,value); }

        private Fast _ActiveFast;
        public Fast ActiveFast { get => _ActiveFast; set => SetProperty(ref _ActiveFast,value); }

        public ICommand OpenFasting { get; private set; }
        public ICommand StopFasting { get; private set; }
        public ICommand ShowStopDialog { get; private set; }

        private string _timerMessage;
        public string TimerMessage { get => _timerMessage;set => SetProperty(ref _timerMessage,value); }

        private double _percentDone;
        public double PercentDone { get => _percentDone; set => SetProperty(ref _percentDone, value); }

        private Timer refreshTimer;

        public FastingViewModel() 
        {
            RelayCommands();
            refreshTimer = new Timer(TimerHandler,null,Timeout.Infinite, 1000);

            RefreshFastData();
        }

        private void TimerHandler(object state)
        {
            PercentDone = ActiveFast.GetElapsedTimePercentage() / 100;
            TimeSpan untilEnd = ActiveFast.GetTimeSpanUntilEnd();
            TimerMessage = untilEnd.ToString(@"hh\:mm\:ss");
        }

        private void RelayCommands()
        {
            OpenFasting = new Command(OpenFastingStart);
            StopFasting = new Command(StopFastingHandler);
            ShowStopDialog = new Command(StopDialogHandler);
        }

        private async void StopDialogHandler()
        {
            if (DateTime.Now < ActiveFast.GetPlannedEnd() && 
                await Shell.Current.DisplayAlert("Fail fast", "Would you like to FAIL this fast?", "STAY FAT", "no...sorry") == false)
            {
                return;
            }

            StopFastingHandler();
        }

        private async void StopFastingHandler()
        {
            refreshTimer.Change(Timeout.Infinite, Timeout.Infinite);

            ActiveFast.End();
            await DB.UpdateFast(ActiveFast);
            
            ActiveFast = null;
            isFastActive = false;
            TimerMessage = "";
            PercentDone = 0;
        }

        private async void OpenFastingStart()
        {
            var startView = new StartFastingView();
            startView.NewFastStarted += StartView_NewFastStarted;

            await MopupService.Instance.PushAsync(startView);
        }

        private void StartView_NewFastStarted(object sender, EventArgs e)
        {
            RefreshFastData();
        }

        private async void RefreshFastData()
        {
            ActiveFast = await DB.GetRunningFast();
            isFastActive = ActiveFast != null;
            if(ActiveFast != null)
            {
                isFastActive = true;
                refreshTimer.Change(0, 1000);
            }
        }
    }
}
