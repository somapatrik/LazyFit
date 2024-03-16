using SQLite;

namespace LazyFit.Models
{
    public abstract class LogAction
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public DateTime Time { get; set; }

        [Ignore]
        public string Color { get; set; }
        [Ignore]
        public string DisplayName { get; set; }
    }
}
