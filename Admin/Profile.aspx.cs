using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Admin_Profile : System.Web.UI.Page
{
    string cs = GlobalClass.cs;

    public void Alert(string message)
    {
        var m = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(message);
        var script = string.Format("alert({0});", m);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", script, true);
    }
    public void Reload(string message)
    {
        var m = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(message);
        var script = string.Format("alert({0});window.location.replace(window.location.href);", m);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", script, true);
    }
    private void ShowAdminInfo(string adminId)
    {
        DataTable dt = GlobalClass.LoadAdmin(adminId);
        lblAdminId.Text = dt.Rows[0]["AdminId"].ToString().Trim();
        lblAdminName.Text = dt.Rows[0]["Name"].ToString().Trim();
        lblId.Text = dt.Rows[0]["AdminId"].ToString();
        lblName.InnerText = dt.Rows[0]["Name"].ToString();
        lblMobile.InnerText = dt.Rows[0]["Mobile"].ToString();
        lblEmail.InnerText = dt.Rows[0]["Email"].ToString();
        lblType.InnerText = dt.Rows[0]["Type"].ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["ViewStateId"] = System.Guid.NewGuid().ToString();
            Session["SessionId"] = ViewState["ViewStateId"].ToString();
            if (Session["ywivreqi"] != null && Session["rcuuyqtf"] != null)
            {
                string adminId = Session["ywivreqi"].ToString();
                ShowAdminInfo(adminId);
            }
        }
        else
        {
            if (ViewState["ViewStateId"].ToString() != Session["SessionId"].ToString())
                Response.Redirect(Request.Url.AbsoluteUri);
            Session["SessionId"] = System.Guid.NewGuid().ToString();
            ViewState["ViewStateId"] = Session["SessionId"].ToString();
        }

        if (Session["ywivreqi"] == null || Session["rcuuyqtf"] == null)
        {
            string prevPage = Request.Url.AbsoluteUri;
            Response.Redirect("Login.aspx?Mode=Redirect&Url=" + prevPage);
        }
    }

    protected void btnEditPassword_Click(object sender, EventArgs e)
    {
        passwordUpdatePanel.Visible = true;
    }

    protected void linkClose_Click(object sender, EventArgs e)
    {
        passwordUpdatePanel.Visible = false;
        txtPassword.Text = txtCPassword.Text = txtOldPassword.Text = "";
    }

    protected void linkEdit_Click(object sender, EventArgs e)
    {
        Alert("This features is coming soon......");
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        string adminId = Session["ywivreqi"].ToString();
        DataTable dt = GlobalClass.LoadAdmin(adminId);
        string password = dt.Rows[0]["Password"].ToString().Trim();
        string oldpassword = txtOldPassword.Text.Trim();
        string newpassword = txtPassword.Text.Trim();
        string confirmpassword = txtCPassword.Text.Trim();
        if (oldpassword != "" && newpassword != "" && confirmpassword != "")
        {
            if (newpassword == confirmpassword)
            {
                if (password == oldpassword)
                {
                    try
                    {
                        using (SqlConnection con = new SqlConnection(cs))
                        {
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = con;
                            cmd.CommandText = "update tblAdmin set Password = @Password where AdminId = @AdminId";
                            cmd.Parameters.AddWithValue("@Password", newpassword);
                            cmd.Parameters.AddWithValue("@AdminId", adminId);
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                        Reload("Password changed successfully.");
                    }
                    catch
                    {
                        Alert(GlobalClass.DatabaseError);
                    }
                }
                else
                    Alert("Current password does not match!");
            }
            else
                Alert("New password and confirm password does not match!");
        }
        else
            Alert("Please fill all fields.");
    }
}