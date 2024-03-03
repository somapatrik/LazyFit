using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Models.WeightModels;
using LazyFit.Services;
using Mopups.Services;
using System.Windows.Input;

namespace LazyFit.ViewModels.WeightViewModels
{
    public class EnterWeightViewModel : PrimeViewModel
    {

        private string _entryWeight;
        public string entryWeight
        {
            get
            {
                return _entryWeight;
            }
            set
            {
                SetProperty(ref _entryWeight, value);
                RefreshCans();
            }
        }

        private decimal inputWeight;

        private bool _WeightValid;
        public bool WeightValid { get => _WeightValid; set => SetProperty(ref _WeightValid, value); }

        public ICommand SaveWeight { get; set; }

        public EnterWeightViewModel()
        {
            SaveWeight = new Command(SaveHandler, canSave);
        }

        private async void SaveHandler()
        {
            Weight newWeight = new Weight(Guid.NewGuid(), inputWeight, UnitWeight.kg);
            //await DB.InsertWeight(newWeight);
            await WeightService.InsertWeight(newWeight);
            WeakReferenceMessenger.Default.Send(new Messages.RefreshWeightMessage(true));

            await MopupService.Instance.PopAsync();
        }

        private bool canSave()
        {
            bool canParse = decimal.TryParse(entryWeight, out inputWeight);
            return canParse && inputWeight > 0 && inputWeight < 500;
        }

        private void RefreshCans()
        {
            ((Command)SaveWeight).ChangeCanExecute();
        }

    }
}
