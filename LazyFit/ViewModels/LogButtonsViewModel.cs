using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Messages;
using LazyFit.Models;
using LazyFit.Services;
using LazyFit.Views;
using LazyFit.Views.Pressure;
using Mopups.Services;

namespace LazyFit.ViewModels
{
    public partial class LogButtonsViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _LatestWeight;

        [ObservableProperty]
        private bool _FastExists;

        public LogButtonsViewModel()
        {
            LoadDataAsync();
            WireMessages();
        }

        private void WireMessages()
        {
            WeakReferenceMessenger.Default.Register<WeightRefreshMessage>(this, async (a, b) => await LoadLatestWeight());
            WeakReferenceMessenger.Default.Register<FastStartMessage>(this, async (a, b) => await LoadActiveFast());
            WeakReferenceMessenger.Default.Register<FastEndMessage>(this, async (a, b) => await LoadActiveFast());
        }

        private async void LoadDataAsync()
        {
            await LoadLatestWeight();
            await LoadActiveFast();
        }

        private async Task LoadLatestWeight()
        {
            var latest = await WeightService.GetLastWeight();
            if (latest != null)
                LatestWeight = latest.WeightValue.ToString();
            else
                LatestWeight = "";
        }

        private async Task LoadActiveFast()
        {
            FastExists = await FastService.GetRunningFast() != null;
        }


        [RelayCommand]
        private async Task StartFast()
        {
            await Close();
            await MopupService.Instance.PushAsync(new StartFastingView());
        }

        [RelayCommand]
        private async Task ShowWeight(object obj)
        {
            var weightView = new EnterWeightView();

            await Close();
            await MopupService.Instance.PushAsync(weightView);

        }

        [RelayCommand]
        private async Task ShowFood(object obj)
        {
            await Close();
            await MopupService.Instance.PushAsync(new LogFoodView());
        }

        [RelayCommand]
        private async Task ShowDrink(object obj)
        {
            await Close();
            await MopupService.Instance.PushAsync(new LogDrinkView());
            
        }

        [RelayCommand]
        private async Task ShowLogPressure()
        {
            await Close();
            await MopupService.Instance.PushAsync(new EnterPressureView());
        }

        [RelayCommand]
        private async Task ShowLogMood()
        {
            var selectedMood = await Shell.Current.DisplayActionSheet("What is your mood?", "None of your business", null, Enum.GetNames(typeof(MoodName)));
            MoodName selectedType;

            if (Enum.TryParse(selectedMood, out selectedType))
            {
                await MoodService.InsertMood(new Mood(Guid.NewGuid(), selectedType));
                
            }

            await Close();
        }

        private async Task Close()
        {
            await MopupService.Instance.PopAsync();
        }
    }
}
