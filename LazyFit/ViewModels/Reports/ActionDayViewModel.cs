using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Messages;
using LazyFit.Models;
using LazyFit.Models.Drinks;
using LazyFit.Models.Foods;
using LazyFit.Models.Moods;
using LazyFit.Services;
using LazyFit.Views;
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

        MoodService MoodService;
        DrinkService DrinkService;
        FoodService FoodService;

        public ActionDayViewModel(ActionSquareDate actionSquare) 
        {
            MoodService = new MoodService();
            DrinkService = new DrinkService();
            FoodService = new FoodService();    

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
        private async Task AddMore()
        {
            await MopupService.Instance.PushAsync(new LogButtonsDayView(_ActionDate));
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
                else if (selectedAction.ActionObject.GetType() == typeof(Food))
                {
                    await FoodService.DeleteFood((Food)selectedAction.ActionObject);
                }
                else if (selectedAction.ActionObject.GetType() == typeof(Drink))
                {
                    await DrinkService.DeleteDrink((Drink)selectedAction.ActionObject);
                }
                else 
                { 
                    //await DB.Database.DeleteAsync(selectedAction.ActionObject);
                    WeakReferenceMessenger.Default.Send(new ActionsReloadMessages(selectedAction.ActionObject));
                    
                }
                
                Actions.Remove(selectedAction);

                if (!Actions.Any())
                    await Shell.Current.Navigation.PopToRootAsync();
            }


        }
    }
}
