using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Models;
using LazyFit.Services;
using System.Collections.ObjectModel;

namespace LazyFit.ViewModels
{
    class LatestActionsViewModel : PrimeViewModel
    {

        private ObservableCollection<TakenAction> _LatestActions;
        public ObservableCollection<TakenAction> LatestActions { get => _LatestActions; set => SetProperty(ref _LatestActions, value); }   

        public LatestActionsViewModel() 
        {
            LoadActions();
            WeakReferenceMessenger.Default.Register<Messages.ReloadActionsMessage>(this, (r, m) => { LoadActions(); });
        }

        private async void LoadActions()
        {
            DateTime now = DateTime.Now;
            LatestActions = new ObservableCollection<TakenAction>();
            (await DB.GetLatestActions(now.AddDays(-2), now)).ForEach(LatestActions.Add);
        }

    }
}
