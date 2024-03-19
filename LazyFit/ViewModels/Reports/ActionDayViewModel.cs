using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Messages;
using LazyFit.Models;
using LazyFit.Services;
using Mopups.Services;
using System.Collections.ObjectModel;

namespace LazyFit.ViewModels.Reports
{
    public partial class ActionDayViewModel : ObservableObject
    {
        [ObservableProperty]
        private DateTime _ActionDate;

        [ObservableProperty]
        private ObservableCollection<ActionSquare> _Actions = new ObservableCollection<ActionSquare>();

        public ActionDayViewModel(ActionSquareDate actionSquare) 
        {
            ActionDate = actionSquare.Time;
            actionSquare.Actions.ForEach(Actions.Add);
        }

        [RelayCommand]
        private async Task DeleteAction(object action)
        {
            ActionSquare selectedAction = (ActionSquare)action;

            if (await Shell.Current.DisplayAlert($"Delete {selectedAction.ActionName}?",$"Remove {selectedAction.ItemName}?","Delete", "Cancel"))
            {
                await DB.Database.DeleteAsync(selectedAction.ActionObject);
                Actions.Remove(selectedAction);
                WeakReferenceMessenger.Default.Send(new ActionsReloadMessages(selectedAction.ActionObject));

                if (!Actions.Any())
                    await MopupService.Instance.PopAsync();
            }


        }
    }
}
