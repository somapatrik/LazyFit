using CommunityToolkit.Mvvm.Messaging;
using System.Windows.Input;

namespace LazyFit.ViewModels
{
    class ResultsViewModel : PrimeViewModel
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
                SetHeader();
                WeakReferenceMessenger.Default.Send(new Messages.ShowPageMessage(value));
            }
        }

        public ResultsViewModel() 
        {
            ShowOlder = new Command(ShowOlderHandler);
            ShowNewer = new Command(ShowNewerHandler);
            SetHeader();
        }

        private void SetHeader()
        {

            DateTime today = DateTime.Today.AddDays(7 * PageNumber);
            int dayofWeek = today.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)today.DayOfWeek;

            DateTime monday = today.AddDays(-(dayofWeek - 1));
            DateTime sunday = monday.AddDays(6);

            //DateTime headerDate = DateTime.Now.AddMonths(PageNumber);
            //PeriodText = headerDate.ToString("Y");

            PeriodText = $"{monday.ToString("d")} - {sunday.ToString("d")}";
        }

        private void ShowOlderHandler(object obj)
        {
            PageNumber--;
        }

        private void ShowNewerHandler(object obj)
        {
            PageNumber++;
        }
    }
}
