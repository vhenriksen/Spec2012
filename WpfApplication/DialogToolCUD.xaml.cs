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
    /// Interaction logic for DialogToolCUD.xaml
    /// </summary>
    public partial class DialogToolCUD : Window
    {
        public DialogToolCUD()
        {
            InitializeComponent();
        }

        private void buttonOkUpdate_Click(object sender, RoutedEventArgs e)
        {
            string error = "";

            if (textBoxDescription.Text.Length < 5) error = "You must fill in at least 5 characters";
            else if (textBoxDescription.Text.Length > 50) error = "Description can be no longer than 50 characters";
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
