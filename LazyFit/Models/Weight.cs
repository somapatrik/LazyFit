using SQLite;

namespace LazyFit.Models
{
    public class Weight
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public DateTime Time { get; set; }
        public decimal WeightValue { get; set; }
        public WeightUnit.UnitWeight WeightUnit { get; set; }


        public Weight() { }
        
        public Weight(Guid id, decimal weightValue, WeightUnit.UnitWeight unit)
        {
            Id = id;
            WeightValue = weightValue;
            Time = DateTime.Now;
            WeightUnit = unit;
        }
    }   
}
