using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ABE.LTRIC.WpfGui.Converters
{
    public class AllValidInListViewConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
                 var items = values[0] as IEnumerable<object>;
        if (items == null)
        {
            return false;
        }

        foreach (var item in items)
        {
            var hasError = (bool)values[1];
            if (hasError)
            {
                return false;
            }
        }

        return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
