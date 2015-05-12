using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClassLibrarySpec2012;
using System.Data.SqlClient;

public partial class Reservation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string name = (string)Session["employee"];
        string tool = (string)Session["Description"];

        if (name == null || tool == null) Server.Transfer("Default.aspx");

        if (!IsPostBack)
        {
            LabelTool.Text = "TOOL: " + tool;

            CalendarDateStart.SelectedDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            CalendarDateEnd.SelectedDate = DateTime.Parse(DateTime.Now.ToShortDateString()).AddDays(1);

            DropDownListStartHour.DataSource = Service.getInstance().getStartTimes();
            DropDownListStartHour.DataBind();
            DropDownListEndhour.DataSource = Service.getInstance().getEndTimes();
            DropDownListEndhour.DataBind();

            DropDownListLocation.DataSource = Service.getInstance().getLocations();
            DropDownListLocation.DataTextField = "location";
            DropDownListLocation.DataValueField = "id";
            DropDownListLocation.DataBind();
        }
    }

    protected void ButtonConfirm_Click(object sender, EventArgs e)
    {

        string error = "";

        int toolId = (int)Session["ToolId"];
        int startHour = Convert.ToInt32(DropDownListStartHour.SelectedValue.ToString().Substring(0, 2));
        int endHour = Convert.ToInt32(DropDownListEndhour.SelectedValue.ToString().Substring(0, 2));

        DateTime start = CalendarDateStart.SelectedDate.AddHours(startHour);
        
        DateTime end = CalendarDateEnd.SelectedDate.AddHours(endHour);

        Session["start"] = start;
        Session["end"] = end;

        if (CheckBoxIsService.Checked)
        {
            try
            {
                Service.getInstance().createReservation(start, end, toolId, -1, -1);

                Session["isService"] = true;
                Server.Transfer("Reciet.aspx");
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
        }
        else
        {
            try
            {
                int employeeId = (int)Session["employeeid"];
                int locationId = Convert.ToInt32(DropDownListLocation.SelectedValue);

                Service.getInstance().createReservation(start, end, toolId, employeeId, locationId);

                Session["isService"] = false;
                Session["location"] = DropDownListLocation.SelectedItem.Text;
                Server.Transfer("Reciet.aspx");
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
        }

        showError(error);
    }

    protected void ButtonCancel_Click(object sender, EventArgs e)
    {
        Server.Transfer("Tools.aspx");
    }
    protected void ButtonFindDate_Click(object sender, EventArgs e)
    {
        string error = "";

        try
        {
            int hours = Convert.ToInt32(TextBoxHoursWanted.Text);
            int toolid = (int)Session["ToolId"];

            DateTime start = Service.getInstance().findNextDate(toolid, hours);

            CalendarDateStart.SelectedDate = DateTime.Parse(start.ToShortDateString());
            CalendarDateStart.VisibleDate = DateTime.Parse(start.ToShortDateString());
            DropDownListStartHour.SelectedValue = start.ToShortTimeString();

            DateTime end = Service.getInstance().getValidEndDate(start, hours);

            CalendarDateEnd.SelectedDate = DateTime.Parse(end.ToShortDateString());
            CalendarDateEnd.VisibleDate = DateTime.Parse(start.ToShortDateString());
            DropDownListEndhour.SelectedValue = end.ToShortTimeString();
        }
        catch (FormatException ex)
        {
            error = "Use only whole numbers in the hour field";
        }
        catch (Exception ex)
        {
            //Server error
            error = "Server error: " + ex.Message;
        }

        showError(error);
    }

    protected void CheckBoxIsService_CheckedChanged(object sender, EventArgs e)
    {
        LabelLocation.Enabled = !LabelLocation.Enabled;
        DropDownListLocation.Enabled = !DropDownListLocation.Enabled;
    }

    private void showError(string error)
    {
        if (error != "")
        {
            LabelError.Text = error;
            LabelError.Visible = true;
        }
        else
        {
            LabelError.Visible = false;
        }
    }
}