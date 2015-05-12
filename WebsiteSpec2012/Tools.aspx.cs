using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClassLibrarySpec2012;

public partial class Tools : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string name = (string)Session["employee"];

        if (name == null) Server.Transfer("Default.aspx");

        LoginName.InnerText = "Logged in as: " + name;

        fillGrid();
    }

    private void fillGrid() 
    {
        try
        {
            GridViewTools.DataSource = Service.getInstance().getToolsLocationATM();
            GridViewTools.DataBind();
            LabelError.Visible = false;
        }
        catch (Exception ex)
        {
            LabelError.Visible = true;
            GridViewTools.Visible = false;
            LabelTools.Visible = false;
            LabelError.Text = "Server error: " + ex.Message;
        }
    }

    protected void ButtonLogout_Click(object sender, EventArgs e)
    {
        Session["employee"] = null;
        Server.Transfer("Default.aspx");
    }
 
    protected void GridViewTools_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridViewTools.PageIndex = e.NewPageIndex;
        fillGrid();
    }

    protected void GridViewTools_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "AddComment") 
        {
            setValues(e);
            Server.Transfer("Comment.aspx");
        }
        else if (e.CommandName == "createReservation")
        {
            setValues(e);
            Server.Transfer("Reservation.aspx");
        }   
    }

    private void setValues(GridViewCommandEventArgs e) 
    {
        int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
        Session["ToolId"] = (int)GridViewTools.DataKeys[currentRowIndex].Values["ID"];
        Session["Description"] = (string)GridViewTools.DataKeys[currentRowIndex].Values["Tool"];
    }
}