using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DogsitterCRUD.Helpers
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Objet { get; set; }

        public static OperationResult Ok(string message = "", object objet = null) => new OperationResult { Success = true, Message = message, Objet = objet };
        public static OperationResult Fail(string message = "", object objet = null) => new OperationResult { Success = false, Message = message, Objet = objet };
    }

    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool val = (bool)value;
            return val ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Visibility)value == Visibility.Visible;
        }
    }

}
