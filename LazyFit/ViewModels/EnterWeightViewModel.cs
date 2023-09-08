using LazyFit.Models;
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

        private void SaveHandler()
        {
            throw new NotImplementedException();
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
