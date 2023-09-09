using LazyFit.Models;
using LazyFit.Services;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static Android.Security.Identity.CredentialDataResult;

namespace LazyFit.ViewModels
{
    public class MainViewModel : PrimeViewModel
    {
        private Chart _mychart;
        public Chart mychart { get => _mychart; set => SetProperty(ref _mychart, value); }


        public ICommand OpenResults => new Command(async () => await Shell.Current.Navigation.PushModalAsync(new ResultsPage()));
        public MainViewModel() 
        {
//            var entries = new[]
//{
//    new ChartEntry(212)
//    {
//        Label = "UWP",
//        ValueLabel = "112",
//        Color = SKColor.Parse("#2c3e50")
//    },
//    new ChartEntry(248)
//    {
//        Label = "Android",
//        ValueLabel = "648",
//        Color = SKColor.Parse("#77d065")
//    },
//    new ChartEntry(128)
//    {
//        Label = "iOS",
//        ValueLabel = "428",
//        Color = SKColor.Parse("#b455b6")
//    },
//    new ChartEntry(514)
//    {
//        Label = "Forms",
//        ValueLabel = "214",
//        Color = SKColor.Parse("#3498db")
//    }
//};
//            mychart = new BarChart { Entries = entries };
            LazyAsync();
        }

        private async void LazyAsync()
        {
            
        }


    }
}
