using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LazyFit.Models;
using LazyFit.Services;
using LazyFit.Views.Fasting;
using Microcharts;
using SkiaSharp;
using System.Windows.Input;

namespace LazyFit.ViewModels.Fasting
{
    internal partial class FastingViewModel : ObservableObject
    {
        #region Properties

        [ObservableProperty]
        private bool _isFastActive;
        [ObservableProperty]
        private Fast _ActiveFast;
        [ObservableProperty]
        private string _TimerMessage;
        [ObservableProperty]
        private TimeSpan _TimeSinceStart;
        [ObservableProperty]
        private double _PercentDone;
        [ObservableProperty]
        private Chart _ProgressChart;
        [ObservableProperty]
        private string _StopLabel;

        private System.Threading.Timer _RefreshTimer;

        #endregion


        public FastingViewModel()
        {
            _RefreshTimer = new System.Threading.Timer(TimerHandler, null, Timeout.Infinite, 1000);

            RefreshFastData();
            
            if (!IsFastActive)
                CreateEmptyChart();
        }

        private void TimerHandler(object state)
        {
            PercentDone = ActiveFast.GetElapsedTimePercentage(DateTime.Now);
            //TimeSpan untilEnd = ActiveFast.GetTimeSpanUntilEnd();
            TimeSinceStart = ActiveFast.GetTimeSpanSinceStart(DateTime.Now);
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

        [RelayCommand]
        private async void ShowStopDialog()
        {
            if (DateTime.Now < ActiveFast.GetPlannedEnd() &&
                await Shell.Current.DisplayAlert("Fail fast", "Would you like to FAIL this fast?", "STAY UNFIT", "no...sorry") == false)
            {
                return;
            }

            StopFasting();
        }

        [RelayCommand]
        private async void StopFasting()
        {
            _RefreshTimer.Change(Timeout.Infinite, Timeout.Infinite);
            CreateEmptyChart();

            await FastService.EndFast(ActiveFast);

            ActiveFast = null;
            IsFastActive = false;
            TimerMessage = "";
            PercentDone = 0;


            await Shell.Current.Navigation.PushAsync(new FastingReportPage(ActiveFast.Id));            
        }

        private void StartView_NewFastStarted(object sender, EventArgs e)
        {
            RefreshFastData();
        }

        private async void RefreshFastData()
        {
            ActiveFast = await FastService.GetRunningFast();
            IsFastActive = ActiveFast != null;
            if (ActiveFast != null)
            {
                IsFastActive = true;
                _RefreshTimer.Change(0, 1000);
            }
        }
    }
}
