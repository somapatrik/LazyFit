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
    }
}
