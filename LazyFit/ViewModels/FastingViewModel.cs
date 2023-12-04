using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Models;
using LazyFit.Services;
using LazyFit.Views;
using LazyFit.Views.Fasting;
using Microcharts;
using Mopups.Services;
using SkiaSharp;
using System;
using System.Windows.Input;

namespace LazyFit.ViewModels
{
    internal class FastingViewModel : PrimeViewModel
    {
        #region Properties

        private bool _isFastActive;
        private Fast _ActiveFast;
        private string _TimerMessage;
        private TimeSpan _TimeSinceStart;
        private double _PercentDone;
        private Chart _ProgressChart;
        private System.Threading.Timer refreshTimer;
        private string _StopLabel;

        public bool isFastActive { get => _isFastActive;set => SetProperty(ref _isFastActive,value); }
        public Fast ActiveFast { get => _ActiveFast; set => SetProperty(ref _ActiveFast,value); }       
        public string TimerMessage { get => _TimerMessage;set => SetProperty(ref _TimerMessage,value); }
        public string StopLabel { get => _StopLabel; set => SetProperty(ref _StopLabel, value); }
        public TimeSpan TimeSinceStart { get => _TimeSinceStart; set => SetProperty(ref _TimeSinceStart, value); }
        public double PercentDone { get => _PercentDone; set => SetProperty(ref _PercentDone, value); }
        public Chart ProgressChart { get => _ProgressChart; set => SetProperty(ref _ProgressChart, value); }

        #endregion

        #region Commands
        
        public ICommand OpenFasting { get; private set; }
        public ICommand StopFasting { get; private set; }
        public ICommand ShowStopDialog { get; private set; }
        
        #endregion

        public FastingViewModel() 
        {
            RelayCommands();

            refreshTimer = new System.Threading.Timer(TimerHandler,null,Timeout.Infinite, 1000);

            RefreshFastData();

            if (!isFastActive)
                CreateEmptyChart();

            SelectStopLabel();
        }

        private void SelectStopLabel()
        {
            string[] endLabels = { 
                     "Stop fast"
                    ,"End the suffering"
                    ,"Screw it, let´s eat!"
                    ,"Game over"
                    ,"I´m done"
                    ,"The grand finale"
                    ,"End the journey" 
            };

            Random random = new Random();
            int index = random.Next(endLabels.Length);
            StopLabel = endLabels[index];
        }

        private void TimerHandler(object state)
        {
            PercentDone = ActiveFast.GetElapsedTimePercentage();
            //TimeSpan untilEnd = ActiveFast.GetTimeSpanUntilEnd();
            TimeSinceStart =  ActiveFast.GetTimeSpanSinceStart();
            TimerMessage = PercentDone >= 100 ? "Done!" + Environment.NewLine + "+" + TimeSinceStart.ToString(@"hh\:mm\:ss") : TimeSinceStart.ToString(@"hh\:mm\:ss");
            RefreshChart();
        }

        private void RefreshChart()
        {
            float done = (float)PercentDone;
            var entries = new List<ChartEntry>()
            {
                new ChartEntry(done) {  Color = SKColors.LimeGreen },
                new ChartEntry(100f - done) {  Color = SKColor.Parse("#f6f8fa") }
                
            };

            ProgressChart = new DonutChart() 
            { 
                Entries = entries, 
                IsAnimated = false,
                MaxValue = 100f,
                MinValue = 0f,
                Margin = 0,
                HoleRadius = 0.6f
            };

        }

        private void CreateEmptyChart()
        {
            ProgressChart = new DonutChart()
            {
                Entries = new List<ChartEntry>() { new ChartEntry(100) { Color = SKColor.Parse("#f6f8fa") } },
                IsAnimated = true,
                MaxValue = 100f,
                MinValue = 0f,
                Margin = 0,
                HoleRadius = 0.6f
            };
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
                await Shell.Current.DisplayAlert("Fail fast", "Would you like to FAIL this fast?", "STAY UNFIT", "no...sorry") == false)
            {
                return;
            }

            StopFastingHandler();
        }

        private async void StopFastingHandler()
        {
            refreshTimer.Change(Timeout.Infinite, Timeout.Infinite);
            CreateEmptyChart();

            ActiveFast.End();
            await DB.UpdateFast(ActiveFast);
            await Shell.Current.Navigation.PushAsync(new FastingReportPage(ActiveFast.Id));

            ActiveFast = null;
            isFastActive = false;
            TimerMessage = "";
            PercentDone = 0;
           
            WeakReferenceMessenger.Default.Send(new Messages.ReloadActionsMessage(0));
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
