using System.Windows;
using System.Windows.Controls;

namespace WPFBiblioteca.Views.TablesViews;

/// <summary>
///     Interaction logic for BooksView.xaml
/// </summary>
public partial class BooksView : UserControl
{
    public BooksView()
    {
        InitializeComponent();
    }

    public bool IsClicked { get; set; }

    private void BtnDelete_OnClick(object sender, RoutedEventArgs e)
    {
        IsClicked = true;
    }
}