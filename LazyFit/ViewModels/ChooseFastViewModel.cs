using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyFit.ViewModels
{
    public class FastingOption
    {
        public int Hours { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public FastingOption(int hours, string name, string description)
        {
            Hours = hours;
            Name = name;
            Description = description;
        }
    }

    class ChooseFastViewModel : PrimeViewModel
    {
        private List<FastingOption> _FastingOptions;
        public List<FastingOption> FastingOptions { get => _FastingOptions; set => SetProperty(ref _FastingOptions,value); }

        public ChooseFastViewModel() 
        {
            var options = new List<FastingOption>()
            {
                new FastingOption(4, "4 hours", "You pussy, that is not really a fast"),
                new FastingOption(6, "6 hours", "Good for you"),
                new FastingOption(8, "8 hours", "Nice"),
                new FastingOption(10, "10 hours", "Jesus! Why?"),
                new FastingOption(12, "12 hours", "Oh no...")
            };
        }
    }
}
