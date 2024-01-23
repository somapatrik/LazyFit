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
            if (await Shell.Current.DisplayAlert($"Delete '{takenAction.SubjectText}'", $"Erase action from {takenAction.Date.ToString("d")} ?", "Delete", "No"))
            {
                await DB.DeleteItem(takenAction.ClassObject);
                LatestActions.Remove(takenAction);

                if (takenAction.Type == "Weight")
                    WeakReferenceMessenger.Default.Send(new Messages.RefreshWeightMessage(true));

            }
            
        }

    }
}
