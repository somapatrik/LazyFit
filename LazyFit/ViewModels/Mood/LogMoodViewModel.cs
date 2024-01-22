using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Models;
using LazyFit.Services;
using LazyFit.Views;
using LazyFit.Views.Pressure;
using Mopups.Services;
using System.Windows.Input;

namespace LazyFit.ViewModels.MoodViewModels
{
    public class LogMoodViewModel : PrimeViewModel
    {
        private string _latestWeight;
        public string LatestWeight { get => _latestWeight; set => SetProperty(ref _latestWeight, value); }

        public ICommand LogMood { get; private set; }
        public ICommand ShowLogMood { get; private set; }
        public ICommand ShowDrink { get; private set; }
        public ICommand ShowFood { get; private set; }
        public ICommand ShowWeight { get; private set; }
        public ICommand ShowPressure { get; private set; }

        public LogMoodViewModel()
        {
            LogMood = new Command(LogMoodHandler);
            ShowLogMood = new Command(ShowLogMoodHandler);
            ShowDrink = new Command(ShowDrinkHandler);
            ShowFood = new Command(ShowFoodHandler);
            ShowWeight = new Command(ShowWeightHandler);
            ShowPressure = new Command(ShowLogPressureHandler);

            LoadLatestWeight();
        }

        private async void LoadLatestWeight()
        {
            var latest = await DB.GetLastWeight();
            if (latest != null)
                LatestWeight = latest.WeightValue.ToString();
            else
                LatestWeight = "";
        }

        private async void ShowWeightHandler(object obj)
        {
            var weightView = new EnterWeightView();
            weightView.EnterWeightClosed += (sender, e) => LoadLatestWeight();

            await MopupService.Instance.PushAsync(weightView);

        }

        private async void ShowFoodHandler(object obj)
        {
            await MopupService.Instance.PushAsync(new LogFoodView());
        }

        private async void ShowDrinkHandler(object obj)
        {
            await MopupService.Instance.PushAsync(new LogDrinkView());
        }

        private async void ShowLogPressureHandler()
        {
            await MopupService.Instance.PushAsync(new EnterPressureView());
        }

        private async void ShowLogMoodHandler()
        {

            var selectedMood = await Shell.Current.DisplayActionSheet("What is your mood?", "None of your business", null, Enum.GetNames(typeof(MoodName)));
            MoodName selectedType;

            if (Enum.TryParse(selectedMood, out selectedType))
            {
                await DB.InsertMood(new Mood(Guid.NewGuid(), selectedType));
                WeakReferenceMessenger.Default.Send(new Messages.ReloadActionsMessage(0));
            }
        }

        private void LogMoodHandler()
        {

        }
    }
}
