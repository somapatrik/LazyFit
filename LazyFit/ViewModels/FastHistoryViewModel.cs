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
        public ObservableCollection<Fast> FastHistory { get => _FastHistory; set => SetProperty(ref _FastHistory,value); }

        public ICommand RefreshHistory { private set; get; }
        public FastHistoryViewModel() 
        {
            //RefreshHistory = new Command(LoadHistory);
            //LoadHistory();
        }

        private async void LoadHistory()
        {
            var history = new ObservableCollection<Fast>();
            (await DB.GetFastHistory()).ForEach(history.Add);
            FastHistory = history;
        }
    }
}
