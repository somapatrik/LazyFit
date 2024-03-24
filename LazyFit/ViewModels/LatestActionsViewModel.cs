using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Messages;
using LazyFit.Models;
using LazyFit.Services;
using LazyFit.Views.Reports;
using Mopups.Services;
using System.Collections.ObjectModel;

namespace LazyFit.ViewModels
{
    partial class LatestActionsViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<ActionSquareDate> _ActionSquares;

        private int numberOfDays = 7;

        public LatestActionsViewModel() 
        {
            LoadData();
            WireMessages();
        }

        private void WireMessages()
        {
            // Drinking
            WeakReferenceMessenger.Default.Register<DrinkNewMessage>(this, async (a,b) => await LoadActions());
            WeakReferenceMessenger.Default.Register<DrinkDeleteMessage>(this, async (a, b) => await LoadActions());

            // Food
            WeakReferenceMessenger.Default.Register<FoodNewMessage>(this, async (a, b) => await LoadActions());
            WeakReferenceMessenger.Default.Register<FoodDeleteMessage>(this, async (a, b) => await LoadActions());

            // Fast
            WeakReferenceMessenger.Default.Register<FastEndMessage>(this, async (a, b) => await LoadActions());
            WeakReferenceMessenger.Default.Register<FastDeleteMessage>(this, async (a, b) => await LoadActions());

            // Weight
            WeakReferenceMessenger.Default.Register<WeightRefreshMessage>(this, async (a, b) => await LoadActions());

            // Mood
            WeakReferenceMessenger.Default.Register<MoodNewMessage>(this, async (a,b) => await LoadActions());
            WeakReferenceMessenger.Default.Register<MoodDeleteMessage>(this, async (a, b) => await LoadActions());
            WeakReferenceMessenger.Default.Register<MoodUpdateMessage>(this, async (a, b) => await LoadActions());

            // Refresh actions
            WeakReferenceMessenger.Default.Register<ActionsReloadMessages>(this, async (a,b) => await LoadActions());
        }

        private async void LoadData()
        {
            await LoadActions();
        }

        private async Task LoadActions()
        {
            ActionSquares = new ObservableCollection<ActionSquareDate>();

            DateTime now = DateTime.Now;
            
            for (int days = 0; days < numberOfDays; days++)
            {
                DateTime from = now.AddDays(-days).Date;
                DateTime to = from.AddDays(1).AddSeconds(-1);
                var actions = await DB.GetActionSquares(from, to);

                if (actions.Any())
                    ActionSquares.Add(new ActionSquareDate() { Time = from, Actions = actions });

            }            
        }

        [RelayCommand]
        private async Task OpenDay(object sender)
        {
            ActionSquareDate day = (ActionSquareDate)sender;
            await MopupService.Instance.PushAsync(new ActionDayView(day));
        }


    }
}
