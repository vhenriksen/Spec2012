using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reciet : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string name = (string)Session["employee"];

        if (name == null) Server.Transfer("Default.aspx");

        string tool = (string)Session["Description"];
        DateTime start = (DateTime)Session["start"];
        DateTime end = (DateTime)Session["end"];
        bool service = (bool) Session["isService"];

        LabelResume.Text = "<html>TOOL: " + tool +"<br/>";
        LabelResume.Text += "START: " + start.ToShortDateString() + " Kl." + start.ToShortTimeString() +"<br/>";
        LabelResume.Text += "END: " + end.ToShortDateString() + " Kl." + end.ToShortTimeString() +"<br/><br/>";

        if(service)
        {
        LabelResume.Text += "SERVICE RESERVATION";
        }
        else
        {
            LabelResume.Text += "EMPLOYEE: " + name +"<br/>";
            LabelResume.Text += "LOCATION: " + (string)Session["location"];
        }
        
        LabelResume.Text += "<br/></html>";
    }

    protected void ButtonOk_Click(object sender, EventArgs e)
    {
        Server.Transfer("Tools.aspx");
    }
}