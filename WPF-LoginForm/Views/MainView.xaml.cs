using System;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;

namespace WPFBiblioteca.Views;

/// <summary>
///     Interaction logic for MainWindowView.xaml
/// </summary>
public partial class MainView : Window
{
    //DispatcherTimer timer;

    //double panelWidth;
    //bool hidden;

    public MainView()
    {
        InitializeComponent();
        //SOLO PRUEBAS CAMBIARA LUEGO

        //timer = new DispatcherTimer();
        //timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
        //timer.Tick += Timer_Tick;

        ////grdContentMenu.Width = new GridLength(ActualWidth);

        //panelWidth = Convert.ToDouble(grdContentMenu.Width);
    }

    //private void Timer_Tick(object sender, EventArgs e)
    //{
    //    GridLengthConverter myLengthConverter = new GridLengthConverter();
    //    if (hidden)
    //    {
    //        grdContentMenu.Width = new GridLength(ActualWidth + 1);
    //        if (Convert.ToDouble(grdContentMenu.Width) >= panelWidth)
    //        {
    //            timer.Stop();
    //            hidden = false;
    //        }
    //    }
    //    else
    //    {
    //        grdContentMenu.Width = new GridLength(ActualWidth - 1);
    //        if (Convert.ToDouble(grdContentMenu.Width) <= 35)
    //        {
    //            timer.Stop();
    //            hidden = true;
    //        }
    //    }
    //}

    //private void Button_Click(object sender, RoutedEventArgs e)
    //{
    //    timer.Start();
    //}


    [DllImport("user32.dll")]
    public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

    private void pnlcontrolBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var helper = new WindowInteropHelper(this);
        SendMessage(helper.Handle, 161, 2, 0);

        //positionLeft.Text = Left.ToString();
        //positionTop.Text = Top.ToString();
        if (WindowState.Equals(WindowState.Maximized))
        {
            BorderMain.CornerRadius = new CornerRadius(0, 0, 0, 0);
            BorderContent.CornerRadius = new CornerRadius(50, 0, 0, 50);
        }
        else
        {
            BorderMain.CornerRadius = new CornerRadius(50);
            BorderContent.CornerRadius = new CornerRadius(50);
        }

        //Detects left stacked on screen
        if (Left.Equals(0) ||
            Left.Equals(1920)) // screen size for double monitor and resolution 1920 * 1080 Change if its needed
            BorderMain.CornerRadius = new CornerRadius(0, 50, 50, 0);
        //Detects right stacked on screen
        if (Left.Equals(960) || Left.Equals(2880)) BorderContent.CornerRadius = new CornerRadius(50, 0, 0, 50);
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

    #region PopUpRegion

    void ShowPopUpMethod(UIElement variable, string buttonText)
    {
        popUpMenuButtons.PlacementTarget = variable;
        popUpMenuButtons.Placement = PlacementMode.Right;
        popUpMenuButtons.IsOpen = true;
        Header.txtPopUpMenuNavigation.Text = buttonText;
    }

    void HidePopUpMethod()
    {
        popUpMenuButtons.Visibility = Visibility.Collapsed;
        popUpMenuButtons.IsOpen = false;
    }

    private void btnCurrentUser_MouseEnter(object sender, MouseEventArgs e)
    {
        popUpMenuButtons.PlacementTarget = btnCurrentUser;
        popUpMenuButtons.Placement = PlacementMode.Right;
        popUpMenuButtons.VerticalOffset = 0;
        popUpMenuButtons.IsOpen = true;
        Header.txtPopUpMenuNavigation.Text = "Usuario Actual";
        //ShowPopUpMethod(btnCurrentUser, "Usuario Actual");
    }

    private void btnCurrentUser_MouseLeave(object sender, MouseEventArgs e)
    {
        //popUpMenuButtons.Visibility = Visibility.Collapsed;
        //popUpMenuButtons.IsOpen = false;
        HidePopUpMethod();
    }

    private void btnNavigateLendings_MouseEnter(object sender, MouseEventArgs e)
    {
        ShowPopUpMethod(btnNavigateLendings, "Prestamos");
    }

    private void btnNavigateLendings_MouseLeave(object sender, MouseEventArgs e)
    {
        HidePopUpMethod();
    }

    #endregion

    private void btnNavigateMembers_MouseEnter(object sender, MouseEventArgs e)
    {
        ShowPopUpMethod(btnNavigateMembers, "Miembros");
    }

    private void btnNavigateMembers_MouseLeave(object sender, MouseEventArgs e)
    {
        HidePopUpMethod();
    }

    private void btnNavigateHome_MouseEnter(object sender, MouseEventArgs e)
    {
        ShowPopUpMethod(btnNavigateHome, "Home");
    }

    private void btnNavigateHome_MouseLeave(object sender, MouseEventArgs e)
    {
        HidePopUpMethod();
    }

    private void btnNavigateUsers_MouseEnter(object sender, MouseEventArgs e)
    {
        ShowPopUpMethod(btnNavigateUsers, "Usuarios");
    }

    private void btnNavigateUsers_MouseLeave(object sender, MouseEventArgs e)
    {
        HidePopUpMethod();
    }

    private void btnNavigateBooks_MouseEnter(object sender, MouseEventArgs e)
    {
        ShowPopUpMethod(btnNavigateBooks, "Libros");
    }

    private void btnNavigateBooks_MouseLeave(object sender, MouseEventArgs e)
    {
        HidePopUpMethod();
    }

    private void btnLogOut_MouseEnter(object sender, MouseEventArgs e)
    {
        popUpMenuButtons.PlacementTarget = btnCurrentUser;
        popUpMenuButtons.Placement = PlacementMode.Right;
        popUpMenuButtons.VerticalOffset = 0;
        popUpMenuButtons.IsOpen = true;
        Header.txtPopUpMenuNavigation.Text = "Cerrar Sesión";
    }

    private void btnLogOut_MouseLeave(object sender, MouseEventArgs e)
    {
        HidePopUpMethod();
    }
}