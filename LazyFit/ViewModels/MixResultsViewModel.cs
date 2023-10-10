using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Classes;
using LazyFit.Models;
using LazyFit.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyFit.ViewModels
{
    internal class MixResultsViewModel : ResultComponent
    {
        private ObservableCollection<MixResult> _MixedResults;
        public ObservableCollection<MixResult> MixedResults { get => _MixedResults; set => SetProperty(ref _MixedResults, value); }

        public MixResultsViewModel() 
        {
            LoadResults();
            WeakReferenceMessenger.Default.Register<Messages.ShowPageMessage>(this, (r, m) => { ShowPage(m.Value); });
        }

        protected override async void LoadResults()
        {
            DateTime today = DateTime.Today.AddDays(7 * PageNumber);
            int dayofWeek = today.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)today.DayOfWeek;

            DateTime monday = today.AddDays(-(dayofWeek - 1));
            DateTime sunday = monday.AddDays(6);

            MixedResults = new ObservableCollection<MixResult>();
            List<Drink> drinks = await DB.GetDrinks(monday, sunday);
            try { 
            drinks.ForEach(d =>
                MixedResults.Add(new MixResult()
                {
                    EventTime = d.Time,
                    EventTitle = d.TypeOfDrink.ToString()
                })
            ) ;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            //results.ForEach(MixedResults.Add);


        }
    }
}
