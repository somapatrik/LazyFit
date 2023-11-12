using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyFit.Models.Foods
{
    public class FoodProperty
    {
        [PrimaryKey]
        public TypeOfFood FoodId { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set;}

    }
}
