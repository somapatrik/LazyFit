using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Models;
using LazyFit.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LazyFit.ViewModels
{
    class LatestActionsViewModel : PrimeViewModel
    {

        private ObservableCollection<TakenAction> _LatestActions;
        public ObservableCollection<TakenAction> LatestActions { get => _LatestActions; set => SetProperty(ref _LatestActions, value); }   

        public ICommand DeleteAction { private set;get; }
        public LatestActionsViewModel() 
        {
            LoadActions();
            WeakReferenceMessenger.Default.Register<Messages.ReloadActionsMessage>(this, (r, m) => { LoadActions(); });
            DeleteAction = new Command(DeleteActionHandler);
        }

        private async void LoadActions()
        {
            DateTime now = DateTime.Now;
            LatestActions = new ObservableCollection<TakenAction>();
            (await DB.GetLatestActions(now.AddDays(-2), now)).ForEach(LatestActions.Add);
        }

        private async void DeleteActionHandler(object action)
        {
            TakenAction takenAction = (TakenAction) action;
            if (await Shell.Current.DisplayAlert("Lie...I mean delete", $"Erase '{takenAction.SubjectText}' from {takenAction.Date}?", "Delete", "No"))
            {
                await DB.DeleteItem(takenAction.ClassObject);
                LoadActions();
            }
            
        }

    }
}
