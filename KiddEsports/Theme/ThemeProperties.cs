using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace KiddEsports.Theme
{
    public static class ThemeProperties
    {
        public static string GetTextBlock(DependencyObject obj)
        {
            return (string)obj.GetValue(TextBlockProperty);
        }

        public static void SetTextBlock(DependencyObject obj, string value)
        {
            obj.SetValue(TextBlockProperty, value);
        }

        public static readonly DependencyProperty TextBlockProperty =
            DependencyProperty.RegisterAttached
            (
                "TextBlock",
                typeof(string),
                typeof(ThemeProperties),
                new FrameworkPropertyMetadata("")
            );
    }
}