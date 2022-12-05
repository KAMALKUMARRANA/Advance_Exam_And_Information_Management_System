using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Admin_Login : System.Web.UI.Page
{
    string cs = GlobalClass.cs;

    public void Alert(string message)
    {
        var m = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(message);
        var script = string.Format("alert({0});", m);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", script, true);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["Mode"]))
            {
                if (Request.QueryString["Mode"].ToString() == "Redirect")
                {
                    Alert("Login to continue.");
                }
            }
            Session.Abandon();
        }
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        string adminId = txtUsername.Text.Trim();
        string password = txtPassword.Text.Trim();
        if (adminId != "" && password != "")
        {
            try
            {
                if (GlobalClass.IsAdminValid(adminId, password))
                {
                    DataTable dtAdmin = GlobalClass.LoadAdmin(adminId);
                    Session.Add("ywivreqi", dtAdmin.Rows[0]["AdminId"].ToString().Trim());
                    Session.Add("rcuuyqtf", dtAdmin.Rows[0]["Password"].ToString().Trim());
                    if (string.IsNullOrEmpty(Request.QueryString["Mode"]) || string.IsNullOrEmpty(Request.QueryString["Url"]))
                        Response.Redirect("Dashboard.aspx");
                    else
                        Response.Redirect(Request.QueryString["Url"]);
                }
                else
                    Alert("Invalid Username or Password! Try again.");
            }
            catch
            {
                Alert(GlobalClass.DatabaseError);
            }
        }
        else
            Alert("Please fill all fields.");
    }
}