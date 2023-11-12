using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyFit.Models
{
    public class TakenAction
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string SubjectText { get; set; }
        public string AdditionalText { get; set; }
        public string Type { get; set; }
        public Object ClassObject { get; set; }
    }
}
