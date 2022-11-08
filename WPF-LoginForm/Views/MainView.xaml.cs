using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.Xml;
using System.Windows;

using System.Windows.Input;
using System.Windows.Interop;

namespace WPFBiblioteca.Views
{
    /// <summary>
    /// Interaction logic for MainWindowView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            
            //clase1();//SOLO PRUEBAS CAMBIARA LUEGO
        }
        

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void pnlcontrolBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
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
            if (Left.Equals(0) || Left.Equals(1920)) // screen size for double monitor and resolution 1920 * 1080 Change if its needed
            {
                BorderMain.CornerRadius = new CornerRadius(0, 50, 50, 0);
                
            }
            //Detects right stacked on screen
            if (Left.Equals(960) || Left.Equals(2880))
            {
                BorderContent.CornerRadius = new CornerRadius(50, 0, 0, 50);
            }





        }
        
        private void pnlcontrolBar_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        //Action buttons functions
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else this.WindowState = WindowState.Normal;

            
        }
        //public void clase1()
        //{
        //    CustomControls.customRows obj = new CustomControls.customRows();
        //    obj.Margin = new Thickness(465, 372, 0, 0);
        //    gridPrincipal.Children.Add(obj);
        //}
    }
}
