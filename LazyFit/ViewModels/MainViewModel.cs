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
    public class MainViewModel : PrimeViewModel
    {
        public ICommand OpenResults => new Command(async () => await Shell.Current.Navigation.PushModalAsync(new ResultsPage()));

        public MainViewModel() 
        {
            LazyAsync();
        }

        private async void LazyAsync()
        {
            Mood mood = new Mood(Guid.NewGuid(), MoodType.MoodName.Sick);
            await DB.InsertMood(mood);
            var gfds = await DB.GetAllMoods();
        }


    }
}
