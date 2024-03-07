using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyFit.Converters
{
    class TimeSpanToCountdown : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = "";
            if (value is TimeSpan)
            {
                var span = (TimeSpan)value;
                result = $"{span.Hours}h {span.Minutes}m {span.Seconds}s";
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
