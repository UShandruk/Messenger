using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace ClientDesktop.View
{
    ///// <summary>
    ///// Конвертер для биндинга.
    ///// Нужен т.к. у TextBox нет такого свойства, а оно требуется для биндинга с другим булевым свойством другого объекта
    ///// </summary>
    //public class TextToBoolConverter: IValueConverter
    //{
    //    /// <summary>
    //    /// Если текст есть - true. Eсли нет - false.
    //    /// </summary>
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        bool result = ((string)value).Length > 0;
    //        return result;
    //    }

    //    /// <summary>
    //    /// Не используется (заглушка для IValueConverter)
    //    /// </summary>
    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        //DependencyProperty.UnsetValue;
    //        return null;
    //    }
    //}
}