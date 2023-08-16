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
        private Guid Id { get;  }    
        private DateTime CafeTime { get; }

        public Cafe() { }

        public Cafe(Guid id, DateTime cafeTime)
        {
            Id = id;
            CafeTime = cafeTime;
        }
    }
}
