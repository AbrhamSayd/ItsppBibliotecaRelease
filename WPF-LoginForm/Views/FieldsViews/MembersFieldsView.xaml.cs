using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFBiblioteca.Views.FieldsViews;

/// <summary>
///     Lógica de interacción para MembersFieldsView.xaml
/// </summary>
public partial class MembersFieldsView : UserControl
{
    public MembersFieldsView()
    {
        InitializeComponent();
    }

    private void TextIntegerPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex("[^0-9]+");
        e.Handled = regex.IsMatch(e.Text);
    }

    private void TextStringOnPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        if (!Regex.IsMatch(e.Text, "^[a-zA-Z]")) e.Handled = true;
    }
}