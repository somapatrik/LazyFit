using LazyFit.Models;
using LazyFit.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LazyFit.ViewModels
{
    public class LogMoodViewModel
    {
        public ICommand LogMood { get;private set; }
        public ICommand ShowLogMood { get; private set; }

        public ICommand ShowDrink { get; private set; }

        public LogMoodViewModel() 
        {
            LogMood = new Command(LogMoodHandler);
            ShowLogMood = new Command(ShowLogMoodHandler);
            ShowDrink = new Command(ShowDrinkHandler);
        }

        private async void ShowDrinkHandler(object obj)
        {
            var selectedDrink = await Shell.Current.DisplayActionSheet("What kind of drink?", "Nothing...my mistake", null, Enum.GetNames(typeof(DrinkType.TypeOfDrink)));
            DrinkType.TypeOfDrink drinkType;

            if (Enum.TryParse(selectedDrink, out drinkType))
            {
                await DB.InsertDrink(new Drink(Guid.NewGuid(), drinkType));
            }
        }

        private async void ShowLogMoodHandler()
        {

            var selectedMood = await Shell.Current.DisplayActionSheet("What is your mood?", "None of your business", null, Enum.GetNames(typeof(MoodType.MoodName)));
            MoodType.MoodName selectedType;
            
            if (Enum.TryParse(selectedMood,out selectedType))
            {
                await DB.InsertMood(new Mood(Guid.NewGuid(), selectedType));
            }
        }

        private void LogMoodHandler()
        {
            
        }
    }
}
