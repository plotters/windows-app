using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace wallabag.Converters
{
    class ItemClickEventArgsToItemViewModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value as ItemClickEventArgs).ClickedItem;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
