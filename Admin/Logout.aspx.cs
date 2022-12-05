using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Logout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session.Abandon();
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        Response.Redirect("Login.aspx?Mode=Logout");
    }
}