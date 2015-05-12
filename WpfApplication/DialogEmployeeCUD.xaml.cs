using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;

namespace WpfApplication
{
    /// <summary>
    /// Interaction logic for DialogEmployeeCUD.xaml
    /// </summary>
    public partial class DialogEmployeeCUD : Window
    {
        public DialogEmployeeCUD()
        {
            InitializeComponent();
        }

        private void buttonOkUpdate_Click(object sender, RoutedEventArgs e)
        {
            string error = "";

            if (textBoxName.Text.Length < 1 ) error = "You must fill in a name";
            else if (textBoxInitials.Text.Length < 1) error = "You must fill in intials that is at most 5 characters long";
            else if (textBoxInitials.Text.Length > 5) error = "Initials should be max 5 characters";
            else if (textBoxPassword.Text.Length != 8) error = "Password should be 8 characters long";
            else this.DialogResult = true;

            if (error != "") MessageBox.Show(this, error, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        //Noget voodoo til at omgå en bug hvorved musen ikke mister focus
        //når der efter klik på en kalender klikkes på en knap
        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);
            if (Mouse.Captured is CalendarItem)
            {
                Mouse.Capture(null);
            }
        }
    }
}
