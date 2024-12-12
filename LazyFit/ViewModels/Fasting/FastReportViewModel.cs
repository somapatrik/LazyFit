using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Messages;
using LazyFit.Models;
using LazyFit.Services;

namespace LazyFit.ViewModels.Fasting
{
    internal partial class FastReportViewModel : ObservableObject
    {
        [ObservableProperty]
        private Fast _FinishedFast;
        [ObservableProperty]
        private TimeSpan _FastSpan;
        [ObservableProperty]
        private TimeSpan _PlanSpan;
        [ObservableProperty]
        private DateTime? _EndDate;
        [ObservableProperty]
        private TimeSpan? _EndTime;
        [ObservableProperty]
        private bool _FastCompleted;

        async partial void OnEndDateChanged(DateTime? oldValue, DateTime? newValue)
        {
            if (oldValue == null )
                return;

            if (newValue.Value.Date < FinishedFast.StartTime.Date)
                return;

            if (newValue.Value.Date > DateTime.Now.Date)
                return;

            await UpdateEnd();
        }

        async partial void OnEndTimeChanged(TimeSpan? oldValue, TimeSpan? newValue)
        {
            if (oldValue == null)
                return;

            if (EndDate == null)
                return;

            if (EndDate.Value.Date == FinishedFast.StartTime.Date && newValue.Value < FinishedFast.StartTime.TimeOfDay)
                return;

            if (EndDate.Value.Date == DateTime.Now.Date && newValue.Value > DateTime.Now.TimeOfDay)
                return;

            await UpdateEnd();


        }

        FastService FastService;
        public FastReportViewModel(Fast finishedFast) 
        {
            FastService = new FastService();

            FinishedFast = finishedFast;
            LoadSpan();
            LoadEnd();
        }

        private void LoadSpan()
        {
            FastSpan = (DateTime)FinishedFast.EndTime - FinishedFast.StartTime;
            PlanSpan = FinishedFast.GetPlannedEnd() - FinishedFast.StartTime;
            FastCompleted = FinishedFast.Completed;
            
        }

        private void LoadEnd()
        {
            EndDate = FinishedFast.EndTime.Value;
            EndTime = FinishedFast.EndTime.Value.TimeOfDay;
        }

        private async Task UpdateEnd()
        {
            var newEnd = EndDate.Value.Date;
            newEnd = newEnd.AddTicks(EndTime.Value.Ticks);

            FinishedFast.EndTime = newEnd;

            await FastService.UpdateFast(FinishedFast);
            LoadSpan();
            WeakReferenceMessenger.Default.Send(new FastUpdateMessage(FinishedFast));
        }
    }
}
