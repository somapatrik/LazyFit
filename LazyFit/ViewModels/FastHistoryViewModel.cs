
using LazyFit.Models;
using LazyFit.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private bool _isRefreshing;
        public bool IsRefreshing { get => _isRefreshing; set => SetProperty(ref _isRefreshing, value); }

        private int _pageNumber;

        public FastHistoryViewModel() 
        {
            RefreshList = new Command(LoadFastList);
            ShowOlder = new Command(ShowOlderHandler);
            ShowNewer = new Command(ShowNewerHandler);

            LoadFastList();
        }

        private void ShowOlderHandler(object obj)
        {
            _pageNumber--;
            LoadFastList();
        }

        private void ShowNewerHandler(object obj)
        {
            _pageNumber++;
            LoadFastList();
        }

        private async void LoadFastList()
        {
            IsRefreshing = true;
            FastHistory = new ObservableCollection<Fast>();
            List<Fast> fasts = (await DB.GetFastsByPage(_pageNumber)).OrderByDescending(f=>f.EndTime).ToList();
           // fasts.ForEach(FastHistory.Add);
            IsRefreshing = false;
        }
    }
}
