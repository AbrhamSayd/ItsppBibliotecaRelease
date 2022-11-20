using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFBiblioteca.Views.FieldsViews
{
    /// <summary>
    /// Interaction logic for UsersFieldsView.xaml
    /// </summary>
    public partial class UsersFieldsView : UserControl
    {
        public UsersFieldsView()
        {
            
            InitializeComponent();
            
        }

        private void TxtId_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void TextStringOnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[a-zA-Z]"))
            {
                
                e.Handled = true;
            }

        }
    }
}
