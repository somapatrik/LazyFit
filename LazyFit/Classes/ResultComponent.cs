using LazyFit.ViewModels;
using System.Windows.Input;

namespace LazyFit.Classes
{
    abstract class ResultComponent : PrimeViewModel
    {
        public ICommand ShowOlder { private set; get; }
        public ICommand ShowNewer { private set; get; }

        private string _PeriodText;

        public string PeriodText
        {
            get => _PeriodText;
            set
            {
                SetProperty(ref _PeriodText, value);
            }
        }

        private int _pageNumber;

        public int PageNumber
        {
            get => _pageNumber;
            set
            {
                SetProperty(ref _pageNumber, value);
                SetDates();
            }
        }

        public DateTime StartDate { get; set; }
        public DateTime FirstDateTime { get; set; }
        public DateTime LastDateTime { get; set; }
        public DateTime PreviousFirstDate { get; private set; }
        public DateTime PreviousLastDate { get; private set; }

        public ResultComponent()
        {
            SetDates();
        }

        protected virtual void SetDates()
        {
            // Start point inside week
            StartDate = DateTime.Today.AddDays(7 * PageNumber);
            int dayofWeek = StartDate.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)StartDate.DayOfWeek;

            // Get first / last datetime of that week 
            FirstDateTime = StartDate.AddDays(-(dayofWeek - 1));
            LastDateTime = FirstDateTime.AddDays(7).AddMinutes(-1);

            // Previous period
            PreviousFirstDate = FirstDateTime.AddDays(-7);
            PreviousLastDate = LastDateTime.AddDays(-7);
        }

        protected virtual void ShowPage(int pageNum)
        {
            PageNumber = pageNum;
            LoadResults();

        }

        protected abstract void LoadResults();
    }
}
