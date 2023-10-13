using CommunityToolkit.Mvvm.Messaging;
using LazyFit.Classes;
using LazyFit.Models;
using LazyFit.Services;
using System.Collections.ObjectModel;

namespace LazyFit.ViewModels
{
    internal class MixResultsViewModel : ResultComponent
    {
        private ObservableCollection<DateResult> _MixedResults;
        public ObservableCollection<DateResult> MixedResults { get => _MixedResults; set => SetProperty(ref _MixedResults, value); }

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

            MixedResults = new ObservableCollection<DateResult>();
            List<Drink> drinks = await DB.GetDrinks(monday, sunday);
            List<Mood> moods = await DB.GetMoods(monday, sunday);
            List<Food> foods = await DB.GetFoods(monday, sunday);

            var mixed = new List<MixResult>();

            mixed.AddRange(drinks.Select(drink => new MixResult() { EventTime = drink.Time, EventTitle = drink.TypeOfDrink.ToString() }));
            mixed.AddRange(moods.Select(mood => new MixResult() { EventTime = mood.Time, EventTitle = mood.TypeOfMood.ToString() }));
            mixed.AddRange(foods.Select(foods => new MixResult() { EventTime = foods.Time, EventTitle = foods.TypeOfFood.ToString() }));


            DateTime actDate = monday;
            while (actDate.Date <= sunday.Date)
            {
                var found = mixed.Find(mix=>mix.EventTime.Date == actDate.Date);
                if (found != null)
                {
                    MixedResults.Add(new DateResult()
                    { 
                        Date=actDate.Date,
                        Results= mixed.Where(m=> m.EventTime.Date == actDate.Date).OrderByDescending(d => d.EventTime).ToList() 
                    });
                }
                actDate = actDate.AddDays(1);
            }



        }
    }
}
