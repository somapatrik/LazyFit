using SQLite;

namespace LazyFit.Models
{
    public class Fast
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set;}
        public int PlanMinutes { get; set; }
        public bool Completed { get; set; }


        public Fast() { }

        public Fast(Guid id)
        {
            Id = id;
            StartTime = DateTime.Now;
        }

        public void End()
        {
            EndTime = DateTime.Now;
            Completed = EndTime >= GetPlannedEnd();
        }

        public void SetHours(int hours)
        {
            PlanMinutes = hours * 60;
        }

        public void GetHoursMinutes(out int hours, out int minutes)
        {
            hours = PlanMinutes / 60;
            minutes = PlanMinutes % 60;
        }

        public DateTime GetPlannedEnd()
        {
            return StartTime.AddMinutes(PlanMinutes);
        }

        public TimeSpan GetTimeSpanUntilEnd()
        {
            DateTime now = DateTime.Now;
            DateTime planEnd = GetPlannedEnd();

            if (now > planEnd)
                return TimeSpan.Zero;

            return planEnd - now;
        }

        public TimeSpan GetTimeSpanSinceEnd()
        {
            if (EndTime == null)
                return TimeSpan.Zero;

            DateTime now = DateTime.Now;
            return now - (DateTime)EndTime;
        }

        public double GetElapsedTimePercentage()
        {
            DateTime currentTime = DateTime.Now;
            TimeSpan elapsedTime = currentTime - StartTime;

            if (elapsedTime.TotalMinutes >= PlanMinutes)
            {
                return 100.0;
            }
            else if (elapsedTime.TotalMinutes <= 0)
            {
                return 0.0;
            }
            else
            {
                double percentage = (elapsedTime.TotalMinutes / PlanMinutes) * 100.0;
                return Math.Round(percentage, 2);
            }
        }

        public TimeSpan GetTimeSpanSinceStart()
        {
            return DateTime.Now - StartTime;
        }


    }

}
