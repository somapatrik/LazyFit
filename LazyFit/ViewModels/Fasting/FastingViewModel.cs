using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LazyFit.Models;
using LazyFit.Services;
using LazyFit.Views.Fasting;
using Microcharts;
using Mopups.Droid.Implementation;
using Mopups.PreBaked.PopupPages.EntryInput;
using Mopups.PreBaked.PopupPages.SingleResponse;
using Mopups.PreBaked.Services;
using Mopups.Services;
using SkiaSharp;

namespace LazyFit.ViewModels.Fasting
{
    internal partial class FastingViewModel : ObservableObject
    {
        #region Properties

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
        private DateTime _PlannedEnd;

        [ObservableProperty]
        private TimeSpan? _FastStartTime;
        [ObservableProperty]
        private DateTime? _FastStartDate;

        private System.Threading.Timer _RefreshTimer;


        partial void OnFastStartDateChanged(DateTime? oldValue, DateTime? newValue)
        {
            if (oldValue == null || newValue == null || FastStartTime == null)
                return;

            if (newValue > DateTime.Now.Date) 
                return;

            SetNewStart();
        }

        partial void OnFastStartTimeChanged(TimeSpan? value)
        {
            if (value is null || FastStartDate == null)
                return;

            DateTime startDate = FastStartDate.Value;
            DateTime Now = DateTime.Now;

            if (startDate.Date >= Now.Date && value.Value > Now.TimeOfDay)
                return;

            SetNewStart();
        }

        #endregion


        public FastingViewModel()
        {
            _RefreshTimer = new System.Threading.Timer(TimerHandler, null, Timeout.Infinite, 1000);

            RefreshFastData();
        }
        private async void RefreshFastData()
        {
            ActiveFast = await FastService.GetRunningFast();
            FastStartDate = ActiveFast.StartTime;
            FastStartTime = ActiveFast.StartTime.TimeOfDay;

            _RefreshTimer.Change(0, 1000);
        }

        private async void SetNewStart()
        {
            DateTime newStart = FastStartDate.Value.Date;
            newStart = newStart.AddTicks(FastStartTime.Value.Ticks);
            ActiveFast.StartTime = newStart;
            await FastService.UpdateFast(ActiveFast);
        }

        private void TimerHandler(object state)
        {
            PercentDone = ActiveFast.GetElapsedTimePercentage(DateTime.Now);
            PlannedEnd = ActiveFast.GetPlannedEnd();
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
        private async Task ShowStopDialog()
        {
            if (DateTime.Now < ActiveFast.GetPlannedEnd() &&
                await Shell.Current.DisplayAlert("Fail fast", "Would you like to FAIL this fast?", "STAY UNFIT", "no...sorry") == false)
            {
                return;
            }

            await StopFasting();
        }

        [RelayCommand]
        private async Task StopFasting()
        {
            _RefreshTimer.Change(Timeout.Infinite, Timeout.Infinite);
            
            await FastService.EndFast(ActiveFast);

            await MopupService.Instance.PopAsync();
            await MopupService.Instance.PushAsync(new FastingReportPage(ActiveFast.Id));
        }
    }
}
