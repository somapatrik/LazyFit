using LazyFit.Models;
using LazyFit.Services;
using Mopups.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LazyFit.ViewModels
{
    public class EnterWeightViewModel : PrimeViewModel
    {
        

        public List<string> unitOptions { get; set; }
        public string selectedUnit { get; set; }

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

        public decimal realWeight { get; set; }

        public ICommand SaveWeight { get; set; }

        public EnterWeightViewModel() 
        {
            LoadUnits(); 

            SaveWeight = new Command(SaveHandler, canSave);
        }

        private async void SaveHandler()
        {
            WeightUnit.UnitWeight unittype = (WeightUnit.UnitWeight)Enum.Parse(typeof(WeightUnit.UnitWeight), selectedUnit);

            await DB.InsertWeight(new Weight(Guid.NewGuid(), realWeight, unittype));
            await MopupService.Instance.PopAsync();
        }

        private bool canSave()
        {
            decimal val; 
            if (decimal.TryParse(entryWeight,out val) && val > 0)
            {
                realWeight = val;
                return true;
            }
            return false;

        }

        private void RefreshCans()
        {
           // MainThread.BeginInvokeOnMainThread(() =>
           // {
                ((Command)SaveWeight).ChangeCanExecute();
           // });
        }

        private void LoadUnits()
        {
            unitOptions = new List<string>();

            var vals = Enum.GetNames(typeof(WeightUnit.UnitWeight));
            foreach(string unit in vals)
            {
                unitOptions.Add(unit);
            }
            selectedUnit = vals[0];
        }
    }
}
