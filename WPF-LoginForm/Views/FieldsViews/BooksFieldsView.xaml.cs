using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFBiblioteca.Views.FieldsViews;

/// <summary>
///     Interaction logic for BooksView.xaml
/// </summary>
public partial class BooksFieldsView : UserControl
{
    private bool max;
    private bool runtime;

    public BooksFieldsView()
    {
        InitializeComponent();
    }

    private void IntInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex("[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
    }

    private void IntScanInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex("[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
    }


    private void TxtIsbn_OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (max || runtime)
        {
            var a = (TextBox)sender;
            a.Text = string.Empty;
            runtime = false;
            max = false;
        }

        if (((TextBox)sender).MaxLength == ((TextBox)sender).Text.Length)
        {
            max = true;
            runtime = true;
            e.Handled = true;
        }
    }
}