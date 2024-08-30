using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Messages;
using LazyFit.Models;
using LazyFit.Models.Moods;
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
            FillData(actionSquare);
        }

        private async void FillData(ActionSquareDate actionSquare)
        {
            await FillDataAsync(actionSquare);
        }

        private Task FillDataAsync(ActionSquareDate actionSquare)
        {
            ActionDate = actionSquare.Time;
            actionSquare.Actions.OrderByDescending(a => a.Time).ToList().ForEach(Actions.Add);
            return Task.CompletedTask;
        }

        [RelayCommand]
        private async Task DeleteAction(object action)
        {
            ActionSquare selectedAction = (ActionSquare)action;

            if (await Shell.Current.DisplayAlert($"Delete {selectedAction.ActionName}?",$"Remove {selectedAction.ItemName} from {selectedAction.Time.ToShortTimeString()}?","Delete", "Cancel"))
            {
                if (selectedAction.ActionObject.GetType() == typeof(Mood))
                {
                    await MoodService.DeleteMood((Mood)selectedAction.ActionObject);
                }
                else 
                { 
                    await DB.Database.DeleteAsync(selectedAction.ActionObject);
                    WeakReferenceMessenger.Default.Send(new ActionsReloadMessages(selectedAction.ActionObject));
                }

                Actions.Remove(selectedAction);
                if (!Actions.Any())
                    await MopupService.Instance.PopAsync();
            }


        }
    }
}
