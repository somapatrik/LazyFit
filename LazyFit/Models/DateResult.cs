using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyFit.Models
{
    public class DateResult
    {
        public DateTime Date { get; set; }
        public List<MixResult> Results { get; set; }
    }
}
