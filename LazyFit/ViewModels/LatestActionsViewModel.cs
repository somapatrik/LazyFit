using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Messages;
using LazyFit.Models;
using LazyFit.Services;
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
            WeakReferenceMessenger.Default.Register<DrinkRefreshMessage>(this, async (a,b) => await LoadActions());
            WeakReferenceMessenger.Default.Register<FoodRefreshMessage>(this, async (a, b) => await LoadActions());
            WeakReferenceMessenger.Default.Register<FastEndMessage>(this, async (a, b) => await LoadActions());

            WeakReferenceMessenger.Default.Register<FastDeleteMessage>(this, async (a, b) => await LoadActions());
            WeakReferenceMessenger.Default.Register<WeightRefreshMessage>(this, async (a, b) => await LoadActions());

            WeakReferenceMessenger.Default.Register<RefreshWeightMessage>(this, async (a, b) => await LoadActions());
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
                    ActionSquares.Add(new ActionSquareDate() { DateTime = from, Actions = actions });

            }            
        }


    }
}
