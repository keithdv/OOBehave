using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Wpf
{
    [ValueConversion(typeof(bool), typeof(Brush))]
    public class BooleanBackground : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(Brush))
                throw new InvalidOperationException("The target must be a Visibility");

            if (parameter != null) { value = !(bool)value; }

            return (bool)value ? Brushes.Green : Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
