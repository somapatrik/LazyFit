using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Models.WeightModels;
using LazyFit.Services;
using Mopups.Services;
using System.Windows.Input;

namespace LazyFit.ViewModels.WeightViewModels
{
    public class EnterWeightViewModel : PrimeViewModel
    {

        private decimal _entryWeight;
        public decimal entryWeight
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

        private bool _WeightValid;
        public bool WeightValid { get => _WeightValid; set => SetProperty(ref _WeightValid, value); }

        public ICommand SaveWeight { get; set; }

        public EnterWeightViewModel()
        {
            SaveWeight = new Command(SaveHandler, canSave);
        }

        private async void SaveHandler()
        {
            await DB.InsertWeight(new Weight(Guid.NewGuid(), entryWeight, UnitWeight.kg));
            WeakReferenceMessenger.Default.Send(new Messages.ReloadActionsMessage(0));
            await MopupService.Instance.PopAsync();
        }

        private bool canSave()
        {
            return entryWeight > 0 && entryWeight < 500;
        }

        private void RefreshCans()
        {
            ((Command)SaveWeight).ChangeCanExecute();
        }

    }
}
