using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class User_Help : System.Web.UI.Page
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
    private void ShowUserInfo(string userId)
    {
        DataTable dtUser = GlobalClass.LoadUser(userId);
        if (dtUser.Rows.Count > 0)
        {
            string name = dtUser.Rows[0]["Name"].ToString().Trim();
            string id = dtUser.Rows[0]["UserId"].ToString().Trim();
            lblInfo.Text = name + " - " + id;
            lblName.InnerText = name;
            lblId.InnerText = id;
            lblMobile.InnerText = dtUser.Rows[0]["Mobile"].ToString().Trim();
            lblEmail.InnerText = dtUser.Rows[0]["Email"].ToString().Trim();

            DataTable dtUserInfo = GlobalClass.LoadUserInfo(userId);
            if (dtUserInfo.Rows[0]["Address"].ToString().Trim() != "")
                lblAddress.InnerText = dtUserInfo.Rows[0]["Address"].ToString().Trim();
        }
    }
    private void ShowReferrerInfo(string userId)
    {
        string id = GlobalClass.LoadUser(userId).Rows[0]["RefId"].ToString().Trim();
        DataTable dt = GlobalClass.LoadUser(id);
        if (dt.Rows.Count > 0)
        {
            name.InnerText = dt.Rows[0]["Name"].ToString().Trim();
            userid.InnerText = dt.Rows[0]["UserId"].ToString().Trim();
            mobile.InnerText = dt.Rows[0]["Mobile"].ToString().Trim();
            email.InnerText = dt.Rows[0]["Email"].ToString().Trim();
            DataTable dtUserInfo = GlobalClass.LoadUserInfo(id);
            if (dtUserInfo.Rows[0]["Address"].ToString().Trim() != "")
                address.InnerText = dtUserInfo.Rows[0]["Address"].ToString().Trim();
        }
        else
            referrerInfo.Visible = false;
    }
    private string GenerateRefNo()
    {
        const String alpha = "0123456789";
        string id = "HELP";
        int exist = 1;
        do
        {
            Random ran = new Random();
            for (int i = 0; i < 6; i++)
            {
                int a = ran.Next(10);
                id = id + alpha.ElementAt(a);
            }
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "select count(RefNo) from tblHelp where RefNo = @RefNo";
                cmd.Parameters.AddWithValue("@RefNo", id);
                con.Open();
                exist = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        while (exist != 0);
        return id;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie userCookies = Request.Cookies["TVUSCK"];
        if (!IsPostBack)
        {
            ViewState["ViewStateId"] = System.Guid.NewGuid().ToString();
            Session["SessionId"] = ViewState["ViewStateId"].ToString();
            if (userCookies != null)
            {
                if (userCookies["xvhuqdph"] != null && userCookies["qbttxpse"] != null)
                {
                    string userId = userCookies["xvhuqdph"].ToString();
                    ShowUserInfo(userId);
                    ShowReferrerInfo(userId);
                }
                else
                    Response.Redirect("Login.aspx?Mode=Redirect&Url=" + Request.Url.AbsoluteUri);
            }
        }
        else
        {
            if (ViewState["ViewStateId"].ToString() != Session["SessionId"].ToString())
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            Session["SessionId"] = System.Guid.NewGuid().ToString();
            ViewState["ViewStateId"] = Session["SessionId"].ToString();
        }

        if (userCookies == null)
            Response.Redirect("Login.aspx?Mode=Redirect&Url=" + Request.Url.AbsoluteUri);
    }

    protected void btnRegister_Click(object sender, EventArgs e)
    {
        string userId = Request.Cookies["TVUSCK"]["xvhuqdph"].ToString();
        string subject = ddlSubject.SelectedItem.ToString();
        string message = txtMessage.Text.Trim();
        string today = GlobalClass.CurrentDateTime();
        if (subject != "Select Subject" && message != "")
        {
            try
            {
                string refno = GenerateRefNo();
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "insert into tblHelp values(@RefNo, @RaiseDate, @Status, @ProcessDate, @UserId, @Subject, @Message)";
                    cmd.Parameters.AddWithValue("@RefNo", refno);
                    cmd.Parameters.AddWithValue("@RaiseDate", today);
                    cmd.Parameters.AddWithValue("@Status", "Processing");
                    cmd.Parameters.AddWithValue("@ProcessDate", "");
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Subject", subject);
                    cmd.Parameters.AddWithValue("@Message", message);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                Reload("Message successfully sent with Refference No: " + refno);
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