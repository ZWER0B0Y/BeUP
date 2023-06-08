using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeUP.Converters
{
    public class ChosenImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value == 1)
                return true;
            else 
            {
                if ((int)value == 0)
                {
                    return false;
                }
                else 
                {
                    return Shell.Current.DisplayAlert("!", "!", "OK");
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
