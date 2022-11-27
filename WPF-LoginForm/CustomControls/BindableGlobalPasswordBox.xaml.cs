using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WPFBiblioteca.CustomControls;

/// <summary>
///     Lógica de interacción para BindableGlobalPasswordBox.xaml
/// </summary>
public partial class BindableGlobalPasswordBox : UserControl
{
    public static readonly DependencyProperty PasswordProperty =
        DependencyProperty.Register("Password", typeof(string), typeof(BindableGlobalPasswordBox),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                PasswordPropertyChanged, null, false, UpdateSourceTrigger.PropertyChanged));

    private bool _isPasswordChanging;

    public BindableGlobalPasswordBox()
    {
        InitializeComponent();
    }

    public string Password
    {
        get => (string)GetValue(PasswordProperty);
        set => SetValue(PasswordProperty, value);
    }

    private static void PasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is BindableGlobalPasswordBox passwordBox) passwordBox.UpdatePassword();
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        _isPasswordChanging = true;
        Password = PasswordBox.Password;
        _isPasswordChanging = false;
    }

    private void UpdatePassword()
    {
        if (!_isPasswordChanging) PasswordBox.Password = Password;
    }
}