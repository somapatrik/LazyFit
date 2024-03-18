using SQLite;

namespace LazyFit.Models.Pressure
{
    public class BloodPressure : LogAction
    {
        public int High { get; set; }
        public int Low { get; set; }
        public int Pulse { get; set; }
        public PressureType Type { get; set; }
        
        public BloodPressure() { }

        public BloodPressure(Guid id,int high, int low)
        {
            Id = id;
            High = high;
            Low = low;
            Time = DateTime.Now;

            Diagnose();
        }

        private void Diagnose()
        {
            if ((High <= 90 ) || (Low <= 60))
            {
                Type = PressureType.Low;
                return;
            }

            if ((High >= 180) || (Low >= 120))
            {
                Type = PressureType.Hypertensive;
                return;
            }


            if ((High > 90 && High < 120) && (Low > 60 && Low <= 80))
            {
                Type = PressureType.Normal;
                return;
            }

            if ((High >= 120 && High < 130) && (Low > 60 && Low <= 80))
            {
                Type = PressureType.Elevated;
                return;
            }

            if ((High >= 130 && High < 140) || (Low > 80 && Low < 90))
            {
                Type = PressureType.High1;
                return;
            }

            if ((High >= 140 && High < 180) || (Low >= 90 && Low < 120))
            {
                Type = PressureType.High2;
                return;
            }


        }

    }
}
