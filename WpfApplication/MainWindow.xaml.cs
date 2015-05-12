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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClassLibrarySpec2012;
using System.Data;
using System.Windows.Controls.Primitives;
using System.Data.SqlClient;

namespace WpfApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                Service.getInstance().testConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Connection to database failed", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                loadTools();
                loadEmployees();

                //Load locations
                DataTable locationTable = Service.getInstance().getLocations();
                comboBoxLocation.ItemsSource = locationTable.DefaultView;
                comboBoxLocation.DisplayMemberPath = "location";
                comboBoxLocation.SelectedValuePath = "id";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Failed to load data", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }

            //Load times
            comboBoxStartHourOfDay.ItemsSource = Service.getInstance().getStartTimes();
            comboBoxEndHourOfDay.ItemsSource = Service.getInstance().getEndTimes();

            //Select service to no
            radioButtonServiceNo.IsChecked = true;

            //Set selected date to avoid it beeing null
            calendarDateStart.SelectedDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            calendarDateStart.DisplayDate = DateTime.Parse(DateTime.Now.ToShortDateString());

            calendarDateEnd.SelectedDate = DateTime.Parse(DateTime.Now.ToShortDateString()).AddDays(1);
            calendarDateEnd.DisplayDate = DateTime.Parse(DateTime.Now.ToShortDateString()).AddDays(1);
        }

        private void loadEmployees()
        {
            DataTable employeeTable = Service.getInstance().getEmployees();
            comboBoxEmployee.ItemsSource = employeeTable.DefaultView;
            comboBoxEmployee.DisplayMemberPath = "name";
            comboBoxEmployee.SelectedValuePath = "id";
        }

        private void loadTools()
        {
            DataTable toolsTable = Service.getInstance().getTools();
            comboBoxTool.ItemsSource = toolsTable.DefaultView;
            comboBoxTool.DisplayMemberPath = "description";
            comboBoxTool.SelectedValuePath = "id";
        }

        private void buttonCreateTool_Click(object sender, RoutedEventArgs e)
        {
            DialogToolCUD dialog = new DialogToolCUD();
            dialog.Owner = this;
            dialog.labelToolId.Visibility = Visibility.Collapsed;
            dialog.checkBoxIsInUse.Visibility = Visibility.Collapsed;

            dialog.calendarBuydate.SelectedDate = DateTime.Now;
            dialog.calendarBuydate.DisplayDate = DateTime.Now;

            dialog.ShowDialog();

            if (dialog.DialogResult == true)
            {
                try
                {
                    Service.getInstance().createTool(dialog.textBoxDescription.Text, dialog.calendarBuydate.SelectedDate.Value);
                    loadTools();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonUpdateTool_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxTool.SelectedIndex > -1)
            {
                DialogToolCUD dialog = new DialogToolCUD();
                dialog.Owner = this;
                dialog.Title = "Update tool information";
                dialog.textBoxDescription.Text = comboBoxTool.Text;
                dialog.labelToolId.Content = "ID: " + comboBoxTool.SelectedValue;

                dialog.buttonOkUpdate.Content = "Update";

                Tool selectedTool = Service.getInstance().getTool((int)comboBoxTool.SelectedValue);

                dialog.calendarBuydate.SelectedDate = selectedTool.buyDate;
                dialog.calendarBuydate.DisplayDate = selectedTool.buyDate;
                dialog.checkBoxIsInUse.IsChecked = selectedTool.in_use;

                dialog.ShowDialog();

                if (dialog.DialogResult == true)
                {
                    try
                    {
                        Service.getInstance().UpdateTool(selectedTool.id, dialog.textBoxDescription.Text, dialog.calendarBuydate.SelectedDate.Value, dialog.checkBoxIsInUse.IsChecked.Value);
                        loadTools();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show(this, "No tool selected", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void buttonDeleteTool_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxTool.SelectedIndex > -1)
            {
                MessageBoxResult result = MessageBox.Show(this, "Are you sure you want to delete this tool:\n\n" + comboBoxTool.Text + " ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                switch (result)
                {
                    case MessageBoxResult.Yes:

                        try
                        {
                            Service.getInstance().deleteTool(Convert.ToInt32(comboBoxTool.SelectedValue));
                            loadTools();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                        break;

                    case MessageBoxResult.No:
                        break;
                }
            }
            else
            {
                MessageBox.Show(this, "No tool selected", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void buttonComment_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxTool.SelectedIndex > -1)
            {
                DialogComment dialog = new DialogComment();
                dialog.labelSelectedTool.Content = "Tool: " + comboBoxTool.Text;
                dialog.ShowDialog();

                if (dialog.DialogResult == true)
                {
                    try
                    {
                        Service.getInstance().CommentOnTool(Convert.ToInt32(comboBoxTool.SelectedValue), Convert.ToInt32(dialog.comboBoxEmployee.SelectedValue), dialog.textBoxComment.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
            }
            else
            {
                MessageBox.Show(this, "No tool selected", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

        }

        private void buttonCreateEmployee_Click(object sender, RoutedEventArgs e)
        {
            DialogEmployeeCUD dialog = new DialogEmployeeCUD();
            dialog.Owner = this;
            dialog.labelEmployeeId.Visibility = Visibility.Collapsed;
            dialog.checkBoxIsActive.Visibility = Visibility.Collapsed;

            dialog.calendarHireDate.SelectedDate = DateTime.Now;
            dialog.calendarHireDate.DisplayDate = DateTime.Now;

            dialog.ShowDialog();

            if (dialog.DialogResult == true)
            {
                try
                {
                    Service.getInstance().createEmployee(dialog.textBoxName.Text, dialog.textBoxInitials.Text, dialog.textBoxPassword.Text, dialog.calendarHireDate.SelectedDate.Value);
                    loadEmployees();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonUpdateEmployee_Click(object sender, RoutedEventArgs e)
        {
            DialogEmployeeCUD dialog = new DialogEmployeeCUD();
            dialog.Owner = this;
            dialog.Title = "Update employee information";
            dialog.buttonOkUpdate.Content = "Update";

            Employee selectedEmployee = Service.getInstance().getEmployee((int)comboBoxEmployee.SelectedValue);

            dialog.labelEmployeeId.Content = "ID: " + comboBoxEmployee.SelectedValue;
            dialog.textBoxName.Text = comboBoxEmployee.Text;
            dialog.textBoxInitials.Text = selectedEmployee.initials;
            dialog.textBoxPassword.Text = selectedEmployee.password;

            dialog.checkBoxIsActive.IsChecked = selectedEmployee.is_active;

            dialog.calendarHireDate.SelectedDate = selectedEmployee.hireDate;
            dialog.calendarHireDate.DisplayDate = selectedEmployee.hireDate;

            dialog.ShowDialog();

            if (dialog.DialogResult == true)
            {
                try
                {
                    Service.getInstance().UpdateEmployee(selectedEmployee.id, dialog.textBoxName.Text, dialog.textBoxInitials.Text, dialog.textBoxPassword.Text, dialog.calendarHireDate.SelectedDate.Value, dialog.checkBoxIsActive.IsChecked.Value);
                    loadEmployees();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonDeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxEmployee.SelectedIndex > -1)
            {
                MessageBoxResult result = MessageBox.Show(this, "Are you sure you want to delete " + comboBoxEmployee.Text + " ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);


                switch (result)
                {
                    case MessageBoxResult.Yes:
                        try
                        {
                            Service.getInstance().deleteEmployee(Convert.ToInt32(comboBoxEmployee.SelectedValue));
                            loadEmployees();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
            else
            {
                MessageBox.Show(this, "No employee selected", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void buttonFindDate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int hours = Convert.ToInt32(textBoxHoursWanted.Text);
                int toolid = 0;

                if (comboBoxTool.SelectedIndex > -1) toolid = Convert.ToInt32(comboBoxTool.SelectedValue);
                else throw new Exception("No tool selected");

                DateTime start = Service.getInstance().findNextDate(toolid, hours);

                calendarDateStart.SelectedDate = DateTime.Parse(start.ToShortDateString());
                calendarDateStart.DisplayDate = DateTime.Parse(start.ToShortDateString());

                comboBoxStartHourOfDay.SelectedValue = start.ToShortTimeString();

                DateTime end = Service.getInstance().getValidEndDate(start, hours);

                calendarDateEnd.SelectedDate = DateTime.Parse(end.ToShortDateString());
                calendarDateEnd.DisplayDate = DateTime.Parse(end.ToShortDateString());

                comboBoxEndHourOfDay.SelectedValue = end.ToShortTimeString();
            }
            catch (FormatException ex)
            {
                MessageBox.Show(this, "Use only whole numbers in the hour field", "Wrong dataformat", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (SqlException ex)
            {
                //database error
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                //intet værktøj valgt
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private MessageBoxResult confirmSelection()
        {
            string text = "Are you sure you want to create the following reservation?\n\n";

            text += "START          " + calendarDateStart.SelectedDate.Value.ToLongDateString() + "   Kl." + comboBoxStartHourOfDay.Text + "\n";
            text += "END              " + calendarDateEnd.SelectedDate.Value.ToLongDateString() + "   Kl." + comboBoxEndHourOfDay.Text + "\n";
            text += "TOOL            " + comboBoxTool.Text + "\n";

            if (radioButtonServiceYes.IsChecked.Value) text += "\nSERVICE RESERVATION";
            else text += "LOCATION    " + comboBoxLocation.Text + "\nEMPLOYEE    " + comboBoxEmployee.Text;

            return MessageBox.Show(this, text, "Confirm reservation", MessageBoxButton.YesNo, MessageBoxImage.Question);
        }

        private void buttonCreateReservation_Click(object sender, RoutedEventArgs e)
        {
            string error = "";

            if (comboBoxTool.SelectedIndex < 0) error = "No tool selected";
            else if (comboBoxStartHourOfDay.SelectedIndex < 0) error = "No starttime selected";
            else if (comboBoxEndHourOfDay.SelectedIndex < 0) error = "No endtime selected";
            else if (confirmSelection().Equals(MessageBoxResult.Yes))
            {
                int toolId = Convert.ToInt32(comboBoxTool.SelectedValue);

                int startHour = Convert.ToInt32(comboBoxStartHourOfDay.SelectedValue.ToString().Substring(0, 2));
                int endHour = Convert.ToInt32(comboBoxEndHourOfDay.SelectedValue.ToString().Substring(0, 2));

                DateTime start = calendarDateStart.SelectedDate.Value.AddHours(startHour);
                DateTime end = calendarDateEnd.SelectedDate.Value.AddHours(endHour);

                if (radioButtonServiceYes.IsChecked.Value)
                {
                    try
                    {
                        Service.getInstance().createReservation(start, end, toolId, -1, -1);
                    }
                    catch (Exception ex)
                    {
                        error = ex.Message;
                    }
                }
                else
                {
                    if (comboBoxEmployee.SelectedIndex < 0) error = "No employee selected";
                    else if (comboBoxLocation.SelectedIndex < 0) error = "No locaion selected";
                    else
                    {
                        try
                        {   
                            int employeeId = Convert.ToInt32(comboBoxEmployee.SelectedValue);
                            int locationId = Convert.ToInt32(comboBoxLocation.SelectedValue);

                            Service.getInstance().createReservation(start, end, toolId, employeeId, locationId);
                        }
                        catch (Exception ex)
                        {
                            error = ex.Message;
                        }
                    }
                }
                if (error == "") MessageBox.Show(this, "Reservation was created succesfully", "Reservation created ", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            if (error != "") MessageBox.Show(this, error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void radioButtonServiceYes_Checked(object sender, RoutedEventArgs e)
        {
            switchService(true);
        }

        private void radioButtonServiceNo_Checked(object sender, RoutedEventArgs e)
        {
            switchService(false);
        }

        private void switchService(bool isService)
        {
            groupBoxEmployee.IsEnabled = !isService;
            groupBoxLocation.IsEnabled = !isService;
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
