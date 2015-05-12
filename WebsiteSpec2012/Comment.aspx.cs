using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClassLibrarySpec2012;

public partial class Comment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string name = (string)Session["employee"];
        string tool = (string) Session["Description"];

        if (name == null || tool == null) Server.Transfer("Default.aspx");

        LabelTool.Text = "TOOL: " + (string)Session["Description"];
    }
    protected void ButtonCancel_Click(object sender, EventArgs e)
    {
        Server.Transfer("Tools.aspx");
    }
    protected void ButtonOk_Click(object sender, EventArgs e)
    {
        int toolid = (int)Session["ToolId"];
        int employeeid = (int)Session["employeeid"];

        string error = "";

        if (TextBoxComment.Text.Length < 5) error = "You must fill in at least 5 characters";
        else if (TextBoxComment.Text.Length > 250) error = "A comment can be no longer than 250 characters";
        else
        {
            try
            {
                Service.getInstance().CommentOnTool(toolid, employeeid, TextBoxComment.Text);
                Server.Transfer("Tools.aspx");
            }
            catch (Exception ex)
            {
                error = "Server error: " + ex.Message;
            }
        }
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