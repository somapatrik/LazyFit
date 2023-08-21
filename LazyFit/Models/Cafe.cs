using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyFit.Models
{
    public class Cafe
    {
        [PrimaryKey]
        public Guid Id { get; set; }    
        public DateTime CafeTime { get; set; }

        public Cafe() { }

        public Cafe(Guid id, DateTime cafeTime)
        {
            Id = id;
            CafeTime = cafeTime;
        }
    }
}
