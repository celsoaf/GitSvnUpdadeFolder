using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using GitSvnUpdateFolder.Enums;
using System.Windows.Media.Imaging;
using GitSvnUpdateFolder.Services;

namespace GitSvnUpdateFolder.Views.Folders.Converters
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is FolderState)
            {
                var state = (FolderState)value;

                switch (state)
                {
                    case FolderState.Updated:
                        return ImageUtils.ConvertBitmapToBitmapSource(Properties.Resources.Updated);
                    case FolderState.Updating:
                        return ImageUtils.ConvertBitmapToBitmapSource(Properties.Resources.updating);
                    case FolderState.Outdated:
                        return ImageUtils.ConvertBitmapToBitmapSource(Properties.Resources.outdated);
                    case FolderState.Error:
                        return ImageUtils.ConvertBitmapToBitmapSource(Properties.Resources.error);
                    default:
                        break;
                }

                return ImageUtils.ConvertBitmapToBitmapSource(Properties.Resources.Updated);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
