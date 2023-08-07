using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyFit.Models
{
    public class Fast
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set;}
        public int PlanMinutes { get; set; }
        public bool Completed { get; set; }


        public void SetHours(int hours)
        {
            PlanMinutes = hours * 60;
        }

        public void GetHoursMinutes(out int hours, out int minutes)
        {
            hours = PlanMinutes / 60;
            minutes = PlanMinutes % 60;
        }
    }
}
