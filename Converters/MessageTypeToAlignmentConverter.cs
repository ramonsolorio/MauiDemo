using MauiDemo.Models;
using System.Globalization;
namespace MauiDemo.Converters
{
    public class MessageTypeToAlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MessageType messageType)
            {
                return messageType switch
                {
                    MessageType.User => LayoutOptions.End,
                    MessageType.Bot => LayoutOptions.Start,
                    _ => LayoutOptions.Center
                };
            }
            return LayoutOptions.Center;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}