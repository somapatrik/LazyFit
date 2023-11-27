using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyFit.Models.Pressure
{
    public class BloodPressure
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public int High { get; set; }
        public int Low { get; set; }
        public int Pulse { get; set; }
        
        public BloodPressure() { }

        public BloodPressure(Guid id,int high, int low)
        {
            Id = id;
            High = high;
            Low = low;
        }

    }
}
