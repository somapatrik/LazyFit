

namespace LazyFit.Classes
{
    public class DateString
    {
        public string Value { get; }
        public DateTime Date { get; }
        public string IconName { get; }

        public DateString(string value, DateTime date, string iconName)
        {
            Value = value;
            Date = date;
            IconName = iconName;
        }
    }
}
