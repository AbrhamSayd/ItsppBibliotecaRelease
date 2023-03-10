using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace WPFBiblioteca.Views;

/// <summary>
///     Interaction logic for LoginView.xaml
/// </summary>
public partial class LoginView : Window
{
    public LoginView()
    {
        InitializeComponent();
    }

    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
            DragMove();
    }

    [DllImport("user32.dll")]
    public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

    private void pnlcontrolBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var helper = new WindowInteropHelper(this);
        SendMessage(helper.Handle, 161, 2, 0);

        //positionLeft.Text = Left.ToString();
        //positionTop.Text = Top.ToString();
        if (WindowState.Equals(WindowState.Maximized))
            BorderMainLogin.CornerRadius = new CornerRadius(0, 0, 0, 0);
        else
            BorderMainLogin.CornerRadius = new CornerRadius(50);
        //Detects left stacked on screen
        if (Left.Equals(0) ||
            Left.Equals(1920)) // screen size for double monitor and resolution 1920 * 1080 Change if its needed
            BorderMainLogin.CornerRadius = new CornerRadius(0, 50, 50, 0);
    }

    private void pnlcontrolBar_MouseEnter(object sender, MouseEventArgs e)
    {
        MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
    }

    //Action buttons functions
    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void btnMaximize_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Maximized;
    }

    private void btnMinimize_Click(object sender, RoutedEventArgs e)
    {
        if (WindowState == WindowState.Normal)
            WindowState = WindowState.Maximized;
        else WindowState = WindowState.Normal;
    }
}