using SQLite;

namespace LazyFit.Models.WeightModels
{
    public class Weight : LogAction
    {
        public decimal WeightValue { get; set; }
        public UnitWeight WeightUnit { get; set; }


        public Weight() { }

        public Weight(Guid id, decimal weightValue, UnitWeight unit)
        {
            Id = id;
            WeightValue = weightValue;
            Time = DateTime.Now;
            WeightUnit = unit;
        }
    }
}
