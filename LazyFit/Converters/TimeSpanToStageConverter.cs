using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyFit.Converters
{
    class TimeSpanToStageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string outMsg = string.Empty;

            if (value is TimeSpan)
            {
                TimeSpan sinceStart = (TimeSpan)value;

                if (sinceStart.TotalHours < 4)
                    outMsg = "Anabolic stage";
                else if (sinceStart.TotalHours >= 4 && sinceStart.TotalHours < 17)
                    outMsg = "Catabolic stage";
                else if (sinceStart.TotalHours >= 17 && sinceStart.TotalHours < 25)
                    outMsg = "Fat-burning stage";
                else if (sinceStart.TotalHours >= 25 && sinceStart.TotalHours < 73)
                    outMsg = "Ketosis  stage";
                else if (sinceStart.TotalHours >= 73 && sinceStart.TotalDays <= 30)
                    outMsg = "Deep ketosis";
                else if (sinceStart.TotalDays > 30)
                    outMsg = "Dead";


            }

            return outMsg;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
