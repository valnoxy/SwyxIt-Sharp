using System.Globalization;
using System.Windows.Data;

namespace SwyxSharp.UserInterface
{
    public class WidthToColumnsConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not (double actualWidth and > 0)) return 1;
            
            const double minWidth = 270; // 260px + Margin/Padding
            var columns = (int)Math.Max(1, Math.Floor(actualWidth / minWidth));
            return columns;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
