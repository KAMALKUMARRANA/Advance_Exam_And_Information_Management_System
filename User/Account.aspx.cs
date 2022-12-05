using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class User_Account : System.Web.UI.Page
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
    private void ShowUser(string userId)
    {
        DataTable dtUser = GlobalClass.LoadUser(userId);
        string name = dtUser.Rows[0]["Name"].ToString().Trim();
        string id = dtUser.Rows[0]["UserId"].ToString().Trim();
        lblInfo.Text = name + " - " + id;

        lblEmail.Text = dtUser.Rows[0]["Email"].ToString();
        lblMobile.Text = dtUser.Rows[0]["Mobile"].ToString();
        lblUserId.Text = dtUser.Rows[0]["UserId"].ToString();
        lblName.InnerText = dtUser.Rows[0]["Name"].ToString();
        if (dtUser.Rows[0]["RefId"].ToString().Trim() != "LEADER")
        {
            DataTable refUser = GlobalClass.LoadUser(dtUser.Rows[0]["RefId"].ToString().Trim());
            tagReferrerId.InnerText = refUser.Rows[0]["UserId"].ToString().Trim();
            lblReferrerName.Text = refUser.Rows[0]["Name"].ToString().Trim();
        }
        else
        {
            tagReferrerId.Visible = false;
            lblReferrerName.Text = "You are a Leader.";
            linkContact.Visible = false;
        }
        if (dtUser.Rows[0]["Status"].ToString().Trim() == "Active")
        {
            if (dtUser.Rows[0]["Package"].ToString().Trim() != "")
            {
                lblPackage.Text = dtUser.Rows[0]["Package"].ToString();
            }
            if (GlobalClass.CheckBoost(userId))
            {
                lblBoost.Text = "Boosted";
            }
            listStatus.Attributes.Add("class", listStatus.Attributes["class"] + " active");
            tagStatus.Attributes.Add("class", tagStatus.Attributes["class"] + " active");
            tagStatus.InnerText = "Active";
        }
    }
    private void ShowUserInfo(string userId)
    {
        DataTable dtUserInfo = GlobalClass.LoadUserInfo(userId);
        if (dtUserInfo.Rows[0]["DOB"].ToString() != "")
            lblDob.InnerText = dtUserInfo.Rows[0]["DOB"].ToString();
        if (dtUserInfo.Rows[0]["Gender"].ToString() != "")
            lblGender.InnerText = dtUserInfo.Rows[0]["Gender"].ToString();
        if (dtUserInfo.Rows[0]["MobileVerified"].ToString().Trim() == "True")
        {
            listMobile.Attributes.Add("class", listMobile.Attributes["class"] + " active");
            tagMobile.Attributes.Add("class", tagMobile.Attributes["class"] + " active");
            tagMobile.InnerText = "Verified";
            linkMobile.Text = "Change";
        }
        if (dtUserInfo.Rows[0]["EmailVerified"].ToString().Trim() == "True")
        {
            listEmail.Attributes.Add("class", listEmail.Attributes["class"] + " active");
            tagEmail.Attributes.Add("class", tagEmail.Attributes["class"] + " active");
            tagEmail.InnerText = "Verified";
            linkEmail.Text = "Change";
        }
        if (dtUserInfo.Rows[0]["KycVerified"].ToString().Trim() == "True")
        {
            listKyc.Attributes.Add("class", listKyc.Attributes["class"] + " active");
            tagKyc.Attributes.Add("class", tagKyc.Attributes["class"] + " active");
            tagKyc.InnerText = "Verified";
            linkKyc.Visible = false;
        }
        if (dtUserInfo.Rows[0]["Address"].ToString().Trim() != "")
        {
            lblAddress.InnerText = dtUserInfo.Rows[0]["Address"].ToString().Trim();
            linkAddress.Visible = false;
        }
        if (dtUserInfo.Rows[0]["Account"].ToString().Trim() != "")
        {
            lblAccountNumber.InnerText = dtUserInfo.Rows[0]["Account"].ToString().Trim();
            lblIfscCode.InnerText = dtUserInfo.Rows[0]["IFSC"].ToString().Trim();
            lblBankName.InnerText = dtUserInfo.Rows[0]["Bank"].ToString().Trim();
            linkBank.Visible = false;
        }
        else
        {
            linkBank.Visible = true;
            linkBank.Text = "Provide Bank Details";
        }
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
                    ShowUser(userId);
                    ShowUserInfo(userId);
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

    protected void linkRsend_Click(object sender, EventArgs e)
    {
        Alert(GlobalClass.CustomError);
    }

    protected void btnVerify_Click(object sender, EventArgs e)
    {
        if (Session["OTP"] != null)
        {
            string userId = Request.Cookies["TVUSCK"]["xvhuqdph"].ToString();
            if (btnVerify.Text == "Verify Email")
            {
                if (txtOtp.Text.Trim() == Session["OTP"].ToString())
                {
                    try
                    {
                        using (SqlConnection con = new SqlConnection(cs))
                        {
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = con;
                            cmd.CommandText = "update tblUserInfo set EmailVerified = @EmailVerified where UserId = @UserId";
                            cmd.Parameters.AddWithValue("@EmailVerified", "True");
                            cmd.Parameters.AddWithValue("@UserId", userId);
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                        Reload("Email verified successfully.");
                    }
                    catch
                    {
                        Alert(GlobalClass.DatabaseError);
                    }
                }
                else
                    Alert("Incorrect OTP entered!");
            }
            else if (btnVerify.Text == "Verify Mobile")
            {
                if (txtOtp.Text.Trim() == Session["OTP"].ToString())
                {
                    try
                    {
                        using (SqlConnection con = new SqlConnection(cs))
                        {
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = con;
                            cmd.CommandText = "update tblUserInfo set MobileVerified = @MobileVerified where UserId = @UserId";
                            cmd.Parameters.AddWithValue("@MobileVerified", "True");
                            cmd.Parameters.AddWithValue("@UserId", userId);
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                        Reload("Mobile verified successfully.");
                    }
                    catch
                    {
                        Alert(GlobalClass.DatabaseError);
                    }
                }
                else
                    Alert("Incorrect OTP entered!");
            }
        }
        else
            Alert("OTP expired!");
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        string userId = Request.Cookies["TVUSCK"]["xvhuqdph"].ToString();
        DataTable dtUser = GlobalClass.LoadUser(userId);
        string password = dtUser.Rows[0]["Password"].ToString().Trim();
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
                            cmd.CommandText = "update tblUser set Password = @Password where UserId = @UserId";
                            cmd.Parameters.AddWithValue("@Password", newpassword);
                            cmd.Parameters.AddWithValue("@UserId", userId);
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
                    Alert("Invalid password!");
            }
            else
                Alert("New password and confirm password does not match!");
        }
        else
            Alert("Please fill all fields.");
    }

    protected void linkClose_Click(object sender, EventArgs e)
    {
        txtOldPassword.Text = txtPassword.Text = txtCPassword.Text = "";
        passwordUpdatePanel.Visible = false;
    }

    protected void linkContact_Click(object sender, EventArgs e)
    {
        Response.Redirect("Help.aspx");
    }

    protected void linkEmail_Click(object sender, EventArgs e)
    {
        /*if (linkEmail.Text == "Verify")
        {
            HttpCookie userCookies = Request.Cookies["userInfo"];
            string userId = userCookies["UserId"].ToString();
            DataTable dtUser = GlobalClass.LoadUser(userId);
            string otp = GlobalClass.GenerateOTP();
            Session["OTP"] = otp;
            string email = dtUser.Rows[0]["Email"].ToString().Trim();
            string subject = "OTP Verification";
            string message = "One time password (OTP) for email verification for User Id " + userId + ", is " + otp + ". And it is valid for next 5 minutes.";

            string response = GlobalClass.SendEmail(email, subject, message);
            if (response == null)
            {
                verifyPanel.Visible = true;
                lblMessage.Text = "Email - " + email;
                btnVerify.Text = "Verify Email";
            }
            else
                Alert(response);
        }
        else
            Alert("Please contact Admin.");*/
        Alert(GlobalClass.CustomError);
    }

    protected void linkMobile_Click(object sender, EventArgs e)
    {
        /*if (linkMobile.Text == "Verify")
        {
            HttpCookie userCookies = Request.Cookies["userInfo"];
            string userId = userCookies["UserId"].ToString();
            DataTable dtUser = GlobalClass.LoadUser(userId);
            string otp = GlobalClass.GenerateOTP();
            Session["OTP"] = otp;
            string number = dtUser.Rows[0]["Mobile"].ToString().Trim();
            string message = "One Time Password (OTP) for mobile verification for User Id " + userId + " is " + otp + ". And it is valid for next 5 minutes.";

            string response = GlobalClass.SendMessage(number, message);
            if (response == null)
            {
                verifyPanel.Visible = true;
                lblMessage.Text = "Mobile - " + number;
                btnVerify.Text = "Verify Mobile";
            }
            else
                Alert(response);
        }
        else
            Alert("Please contact Admin.");*/
        Alert(GlobalClass.CustomError);
    }

    protected void linkKyc_Click(object sender, EventArgs e)
    {
        Alert("This features is coming soon.");
    }

    protected void linkChangePassword_Click(object sender, EventArgs e)
    {
        passwordUpdatePanel.Visible = true;
    }

    protected void linkAddress_Click(object sender, EventArgs e)
    {
        Alert("This features is coming soon.");
    }

    protected void linkBank_Click(object sender, EventArgs e)
    {
        Response.Redirect("Withdraw.aspx");
    }
}