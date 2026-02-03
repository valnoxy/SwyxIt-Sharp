using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using static SwyxSharp.Common.SwyxEnums;
using Brushes = System.Drawing.Brushes;

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

    public class StateToColorConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is SpeedDialState state)
            {
                return state switch
                {
                    SpeedDialState.Unknown => new SolidColorBrush(Colors.White),
                    SpeedDialState.LoggedOut => new SolidColorBrush(Colors.Gray),
                    SpeedDialState.Online => new SolidColorBrush(Colors.Green),
                    SpeedDialState.Calling => new SolidColorBrush(Colors.DarkRed),
                    SpeedDialState.GroupCallNotification => new SolidColorBrush(Colors.Magenta),
                    SpeedDialState.Away => new SolidColorBrush(Colors.Gold),
                    SpeedDialState.DoNotDisturb => new SolidColorBrush(Colors.Red),
                    _ => new SolidColorBrush(Colors.Gray)
                };
            }
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
