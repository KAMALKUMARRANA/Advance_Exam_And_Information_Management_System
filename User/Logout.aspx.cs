using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class User_Logout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Cookies["userInfo"] != null)
        {
            Response.Cookies["userInfo"].Expires = DateTime.Now.AddDays(-30);
        }
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        Response.Redirect("Login.aspx?Mode=Logout");
    }
}