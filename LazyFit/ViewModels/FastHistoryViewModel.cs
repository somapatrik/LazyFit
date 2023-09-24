using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Messages;
using LazyFit.Models;
using LazyFit.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LazyFit.ViewModels
{
    class FastHistoryViewModel : PrimeViewModel
    {
        private ObservableCollection<Fast> _FastHistory;
        public ObservableCollection<Fast> FastHistory { get => _FastHistory; set => SetProperty(ref _FastHistory, value); }

        public ICommand RefreshList { private set; get; }
        public ICommand ShowOlder { private set; get; }
        public ICommand ShowNewer { private set; get; }

        private string _Header;

        public string Header { get => _Header; set => SetProperty(ref _Header, value); }

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

        public FastHistoryViewModel() 
        {
            RefreshList = new Command(LoadFastList);
            ShowOlder = new Command(ShowOlderHandler);
            ShowNewer = new Command(ShowNewerHandler);
            SetHeader();
            LoadFastList();

            WeakReferenceMessenger.Default.Register<Messages.ShowPageMessage>(this, (a,b) => {ShowPage(b.Value);});
        }

        private void ShowPage(int value)
        {
            PageNumber = value;
            LoadFastList();
        }

        private void SetHeader()
        {
            DateTime headerDate = DateTime.Now.AddMonths(PageNumber);
            Header = headerDate.ToString("Y");
        }

        private void ShowOlderHandler(object obj)
        {
            PageNumber--;
            LoadFastList();
        }

        private void ShowNewerHandler(object obj)
        {
            PageNumber++;
            LoadFastList();
        }

        private async void LoadFastList()
        {
            FastHistory = new ObservableCollection<Fast>();
            List<Fast> fasts = (await DB.GetFastsByPage(_pageNumber)).OrderByDescending(f=>f.EndTime).ToList();
            fasts.ForEach(FastHistory.Add);
        }
    }
}
