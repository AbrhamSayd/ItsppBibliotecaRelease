using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace WPFBiblioteca.Views.TablesViews;

/// <summary>
///     Interaction logic for MembersView.xaml
/// </summary>
public class BooleanToYesNoConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((bool)value) return "Debe";
        return "No debe";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
        /* not sure if you need convert back */
    }
}

public partial class MembersView : UserControl
{
    public MembersView()
    {
        InitializeComponent();
    }
}