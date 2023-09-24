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
                SetHeader();
            }
        }

        public ResultComponent()
        {
            ShowOlder = new Command(ShowOlderHandler);
            ShowNewer = new Command(ShowNewerHandler);
            SetHeader();
        }

        protected virtual void SetHeader()
        {
            DateTime headerDate = DateTime.Now.AddMonths(PageNumber);
            PeriodText = headerDate.ToString("Y");
        }

        protected virtual void ShowOlderHandler(object obj)
        {
            PageNumber--;
            LoadResults();
        }

        protected virtual void ShowNewerHandler(object obj)
        {
            PageNumber++;
            LoadResults();
        }

        protected virtual void ShowPage(int pageNum)
        {
            PageNumber = pageNum;
            LoadResults();

        }

        protected abstract void LoadResults();
    }
}
