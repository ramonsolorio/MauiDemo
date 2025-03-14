using MauiDemo.Models;
using System.Globalization;
namespace MauiDemo.Converters
{
    public class MessageTypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MessageType messageType)
            {
                return messageType switch
                {
                    MessageType.User => Color.FromArgb("#DCF8C6"),
                    MessageType.Bot => Colors.White,
                    _ => Colors.Gray
                };
            }
            return Colors.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}