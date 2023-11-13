using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Models;
using LazyFit.Services;
using Mopups.Services;
using System.Windows.Input;

namespace LazyFit.ViewModels
{
    public class EnterWeightViewModel : PrimeViewModel
    {
        

        //public List<string> unitOptions { get; set; }
        //public string selectedUnit { get; set; }

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
           // LoadUnits(); 

            SaveWeight = new Command(SaveHandler, canSave);
        }

        private async void SaveHandler()
        {
            // UnitWeight unittype = (UnitWeight)Enum.Parse(typeof(UnitWeight), selectedUnit);

            // Save kg, calulate others later
            //if (unittype == UnitWeight.lb)
            //    await DB.InsertWeight(new Weight(Guid.NewGuid(), LazyUnitConvertes.LbsToKg(entryWeight), UnitWeight.kg));
            //else
            //    await DB.InsertWeight(new Weight(Guid.NewGuid(), entryWeight, UnitWeight.kg));

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

        //private void LoadUnits()
        //{
        //    unitOptions = new List<string>();

        //    var vals = Enum.GetNames(typeof(UnitWeight));
        //    foreach(string unit in vals)
        //    {
        //        unitOptions.Add(unit);
        //    }
        //    selectedUnit = vals[0];
        //}
    }
}
