using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Collections.Specialized;

public partial class User_Forgot_password : System.Web.UI.Page
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
    private void ShowError(string message)
    {
        error.Visible = true;
        lblError.InnerText = message;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["ViewStateId"] = System.Guid.NewGuid().ToString();
            Session["SessionId"] = ViewState["ViewStateId"].ToString();
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
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string userId = txtUserId.Text.Trim();
        if (userId != "")
        {
            DataTable dt = GlobalClass.LoadUser(userId);
            if (dt.Rows.Count > 0)
            {
                Session.Add("z2yts7", userId);
                Session.Add("mqsp9t", dt.Rows[0]["Mobile"].ToString().Trim());
                formSearch.Visible = false;
                formSendOTP.Visible = true;
                lblMobile.Text = "An One Time Password will be sent to your registered mobile number: " + dt.Rows[0]["Mobile"].ToString().Trim();
            }
            else
                ShowError("Invalid User Id!");
        }
        else
            ShowError("Please enter User Id.");
    }

    protected void btnSendOTP_Click(object sender, EventArgs e)
    {
        if (Session["z2yts7"] != null && Session["mqsp9t"] != null)
        {
            try
            {
                string otp = GlobalClass.GenerateOTP();
                Session.Add("3cyfta", otp);
                DataTable dt = GlobalClass.LoadMessageService();
                string key = dt.Rows[0]["ApiKey"].ToString();
                string Sendername = dt.Rows[0]["SenderName"].ToString();
                string Number = "91" + Session["mqsp9t"].ToString();
                string M = "Your one time password for reset password of your account: " + Session["z2yts7"].ToString() + "  is: " + otp + ". And it is valid for next 5 minutes.";
                String Message = HttpUtility.UrlEncode(M);
                string result;
                using (var wb = new WebClient())
                {
                    byte[] response = wb.UploadValues("https://api.textlocal.in/send/", new NameValueCollection()
                    {
                        {"apikey" , key},
                        {"numbers" , Number},
                        {"message" , Message},
                        {"sender" , Sendername}
                    });
                    result = System.Text.Encoding.UTF8.GetString(response);
                }

                formSearch.Visible = false;
                formSendOTP.Visible = false;
                formSetPassword.Visible = true;
                status.Text = "Password Reset";
                lblInfo.Text = "Password recovery for account: " + Session["z2yts7"].ToString();
                Alert(result);
            }
            catch
            {
                lblSendMessage.Text = GlobalClass.DatabaseError;
            }
        }
        else
            lblSendMessage.Text = "Your session has expired! Please try again.";
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        if (Session["z2yts7"] != null && Session["3cyfta"] != null)
        {
            string userId = Session["z2yts7"].ToString();
            string sessionOtp = Session["3cyfta"].ToString();
            string otp = txtOtp.Text.Trim();
            string password = txtPassword.Text.ToString();
            string cPassword = txtCPassword.Text.Trim();
            if (password != "" && cPassword != "")
            {
                if (password == cPassword)
                {
                    if (otp == sessionOtp)
                    {
                        try
                        {
                            using (SqlConnection con = new SqlConnection(cs))
                            {
                                SqlCommand cmd = new SqlCommand();
                                cmd.Connection = con;
                                cmd.CommandText = "update tblUser set Password = @Password where UserId = @UserId";
                                cmd.Parameters.AddWithValue("@Password", password);
                                cmd.Parameters.AddWithValue("@UserId", userId);
                                con.Open();
                                cmd.ExecuteNonQuery();
                            }
                            var m = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize("Password changed successfully. Please login to continue.");
                            var script = string.Format("alert({0});window.location.replace('Login.aspx');", m);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", script, true);
                        }
                        catch
                        {
                            lblMessage.InnerText = GlobalClass.DatabaseError;
                        }
                    }
                    else
                    {
                        OtpError.Visible = true;
                        lblMessage.InnerText = "Invalid OTP!";
                    }
                }
                else
                    lblMessage.InnerText = "Password does not match!";
            }
            else
                lblMessage.InnerText = "Please fill all fields.";
        }
        else
            lblMessage.InnerText = "You session has expired! Please try again.";
    }
}