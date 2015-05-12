using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClassLibrarySpec2012;

public partial class Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void ButtonLogin_Click(object sender, EventArgs e)
    {
        try
        {
            int id = Convert.ToInt32(TextBoxId.Text);
            string password = TextBoxPassword.Text;

            string name = Service.getInstance().getLoginName(id, password);

            if (name == null)
            {
                LabelError.Text = "Employee not found";
                LabelError.Visible = true;
            }
            else
            {
                Session["employee"] = name;
                Session["employeeid"] = id;
                Server.Transfer("Tools.aspx");
            }
        }
        catch (Exception ex)
        {
            LabelError.Text = "Server error: " + ex.Message;
            LabelError.Visible = true;
        }
    }
}