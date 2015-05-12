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
using System.Data;
using ClassLibrarySpec2012;

namespace WpfApplication
{
    /// <summary>
    /// Interaction logic for DialogComment.xaml
    /// </summary>
    public partial class DialogComment : Window
    {

        public DialogComment()
        {
            InitializeComponent();
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            string error = "";

            if (comboBoxEmployee.SelectedIndex < 0) error = "No employee selected";
            else if (textBoxComment.Text.Length < 5) error = "You must fill in at least 5 characters"; 
            else if (textBoxComment.Text.Length > 250) error = "A comment can be no longer than 250 characters"; 
            else this.DialogResult = true;
            
            if (error != "") MessageBox.Show(this, error, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);     
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataTable employeeTable = Service.getInstance().getEmployees();
            comboBoxEmployee.ItemsSource = employeeTable.DefaultView;
            comboBoxEmployee.DisplayMemberPath = "name";
            comboBoxEmployee.SelectedValuePath = "id";
        }
    }
}
