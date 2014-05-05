using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using GitSvnUpdateFolder.Enums;

namespace GitSvnUpdateFolder.Views.Folders.Converters
{
    public class StateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is FolderState)
            {
                var state = (FolderState)value;

                switch (state)
                {
                    case FolderState.Updated:
                        return Brushes.LightGreen;
                    case FolderState.Updating:
                        return Brushes.LightGray;
                    case FolderState.Outdated:
                        break;
                    case FolderState.Error:
                        return Brushes.LightSalmon;
                    case FolderState.Info:
                        return Brushes.LightBlue;
                    case FolderState.Initializing:
                        break;
                    default:
                        break;
                }
            }
             
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
