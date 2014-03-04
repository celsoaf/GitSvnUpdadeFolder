using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using GitSvnUpdateFolder.Enums;

namespace GitSvnUpdateFolder.Views.Output.Converters
{
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is MessageType)
            {
                var state = (MessageType)value;

                switch (state)
                {
                    case MessageType.Info:
                        return Brushes.Blue;
                    case MessageType.Error:
                        return Brushes.Red;
                    default:
                        return Brushes.Black;
                }
            }

            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
