

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
