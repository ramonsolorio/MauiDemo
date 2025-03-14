using MauiDemo.Models;
using System.Globalization;
namespace MauiDemo.Converters
{
    public class MessageTypeToMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MessageType messageType)
            {
                return messageType switch
                {
                    MessageType.User => new Thickness(80, 5, 10, 5),
                    MessageType.Bot => new Thickness(10, 5, 80, 5),
                    _ => new Thickness(10, 5)
                };
            }
            return new Thickness(10, 5);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}