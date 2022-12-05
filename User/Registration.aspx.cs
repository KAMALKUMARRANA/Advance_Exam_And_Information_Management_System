using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

public partial class User_Registration : System.Web.UI.Page
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
    private string CreateUserId()
    {
        System.Text.StringBuilder UserId = new System.Text.StringBuilder();
        const String alpha = "ABCDEFGHIJKLMNPQRSTUVWXYZ";
        const string beta = "0123456789";
        string prefix = "";
        string postfix = "";
        int exist = 1;
        do
        {
            Random ran = new Random();
            for (int i = 0; i < 4; i++)
            {
                int a = ran.Next(25);
                prefix = prefix + alpha.ElementAt(a);
            }
            for (int i = 0; i < 6; i++)
            {
                int a = ran.Next(10);
                postfix = postfix + beta.ElementAt(a);
            }
            UserId.Append(prefix);
            UserId.Append(postfix);

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "select count(UserId) from tblUser where UserId = @UserId";
                cmd.Parameters.AddWithValue("@UserId", UserId.ToString());
                con.Open();
                exist = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        while (exist != 0);

        return UserId.ToString();
    }
    private string CreatePassword()
    {
        const String store = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnPpQqRrSsTtUuVvWwXxYyZz0123456789";
        string password = "";
        Random ran = new Random();
        for (int i = 0; i < 8; i++)
        {
            int a = ran.Next(60);
            password = password + store.ElementAt(a);
        }
        return password;
    }
    private void ShowError(string message)
    {
        error.Visible = true;
        success.Visible = false;
        lblError.Visible = true;
        lblError.InnerText = message;
        formRegistration.Visible = false;
    }
    private void ShowSuccess()
    {
        error.Visible = false;
        success.Visible = true;
        lblError.Visible = false;
        formRegistration.Visible = true;
    }
    private void SearchReferrer()
    {
        string refercode = txtReferCode.Text.Trim();
        if (refercode != "")
        {
            DataTable dt = GlobalClass.LoadUser(refercode);
            if (refercode.Length == 10 && dt.Rows.Count != 0)
            {
                if (dt.Rows[0]["Status"].ToString().Trim() == "Active")
                {
                    ShowSuccess();
                    lblId.Text = dt.Rows[0]["UserId"].ToString().Trim();
                    lblName.Text = dt.Rows[0]["Name"].ToString().Trim();
                }
                else
                    ShowError("Referrer user inactive!");
            }
            else
                ShowError("Invalid refer code!");

        }
        else
            ShowError("Please enter refer code!");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["ViewStateId"] = System.Guid.NewGuid().ToString();
            Session["SessionId"] = ViewState["ViewStateId"].ToString();
            if (!string.IsNullOrEmpty(Request.QueryString["Refercode"]))
            {
                txtReferCode.Text = Request.QueryString["Refercode"].ToString();
                SearchReferrer();
            }
            else
            {
                txtReferCode.Text = "LEADER7097";
                SearchReferrer();
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
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        SearchReferrer();
    }

    protected void txtReferCode_TextChanged(object sender, EventArgs e)
    {
        SearchReferrer();
    }

    protected void btnRegister_Click(object sender, EventArgs e)
    {
        DataTable dtReferrer = GlobalClass.LoadUser(lblId.Text.Trim());
        if (dtReferrer.Rows.Count > 0)
        {
            string refId = dtReferrer.Rows[0]["UserId"].ToString().Trim();
            string fname = txtFirstName.Text.Trim();
            string lname = txtLastName.Text.Trim();
            string gender = ddlGender.SelectedItem.ToString();
            string dob = txtDob.Text.Trim();
            string mobile = txtMobile.Text.Trim();
            string email = txtEmail.Text.Trim();
            string today = GlobalClass.CurrentDateOnly();
            if (fname != "" && lname != "" && gender != "Select Gender" && dob != "" && mobile != "" && email != "")
            {
                dob = Convert.ToDateTime(dob).ToString("dd-MM-yyyy");
                if (mobile.Length == 10)
                {
                    if (checkPC.Checked == true && CheckTC.Checked == true)
                    {
                        Thread.Sleep(2000);
                        string UserId = CreateUserId();
                        string password = CreatePassword();
                        using (SqlConnection con = new SqlConnection(cs))
                        {
                            con.Open();
                            SqlTransaction transaction = con.BeginTransaction();
                            try
                            {
                                SqlCommand cmdInsertUser = new SqlCommand();
                                cmdInsertUser.Connection = con;
                                cmdInsertUser.Transaction = transaction;
                                cmdInsertUser.CommandText = "insert into tblUser values(@RefId, @UserId, @Password, @Status, @Package, @Name, @Mobile, @Email)";
                                cmdInsertUser.Parameters.AddWithValue("@RefId", refId);
                                cmdInsertUser.Parameters.AddWithValue("@UserId", UserId);
                                cmdInsertUser.Parameters.AddWithValue("@Password", password);
                                cmdInsertUser.Parameters.AddWithValue("@Status", "Inactive");
                                cmdInsertUser.Parameters.AddWithValue("@Package", "FREE");
                                cmdInsertUser.Parameters.AddWithValue("@Name", fname + " " + lname);
                                cmdInsertUser.Parameters.AddWithValue("@Mobile", mobile);
                                cmdInsertUser.Parameters.AddWithValue("@Email", email);
                                cmdInsertUser.ExecuteNonQuery();

                                SqlCommand cmdInsertInfo = new SqlCommand();
                                cmdInsertInfo.Connection = con;
                                cmdInsertInfo.Transaction = transaction;
                                cmdInsertInfo.CommandText = "insert into tblUserInfo values(@UserId, @ExpDate, NULL, @DOB, @Gender, NULL, NULL, NULL, NULL, @KycVerified, @MobileVerified, @EmailVerified)";
                                cmdInsertInfo.Parameters.AddWithValue("@UserId", UserId);
                                cmdInsertInfo.Parameters.AddWithValue("@ExpDate", today);
                                cmdInsertInfo.Parameters.AddWithValue("@DOB", dob);
                                cmdInsertInfo.Parameters.AddWithValue("@Gender", gender);
                                cmdInsertInfo.Parameters.AddWithValue("@KycVerified", "False");
                                cmdInsertInfo.Parameters.AddWithValue("@MobileVerified", "False");
                                cmdInsertInfo.Parameters.AddWithValue("@EmailVerified", "False");
                                cmdInsertInfo.ExecuteNonQuery();

                                SqlCommand cmdCreateAccount = new SqlCommand();
                                cmdCreateAccount.Connection = con;
                                cmdCreateAccount.Transaction = transaction;
                                cmdCreateAccount.CommandText = "insert into tblUserBalance values(@UserId, @Balance)";
                                cmdCreateAccount.Parameters.AddWithValue("@UserId", UserId);
                                cmdCreateAccount.Parameters.AddWithValue("@Balance", 0);
                                cmdCreateAccount.ExecuteNonQuery();

                                SqlCommand cmdCreateTempBalance = new SqlCommand();
                                cmdCreateTempBalance.Connection = con;
                                cmdCreateTempBalance.Transaction = transaction;
                                cmdCreateTempBalance.CommandText = "insert into tblUserTempBalance values(@UserId, @Balance, @Expiry)";
                                cmdCreateTempBalance.Parameters.AddWithValue("@UserId", UserId);
                                cmdCreateTempBalance.Parameters.AddWithValue("@Balance", 0);
                                cmdCreateTempBalance.Parameters.AddWithValue("@Expiry", TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.AddDays(-1), TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")).ToString("dd-MM-yyyy"));
                                cmdCreateTempBalance.ExecuteNonQuery();

                                SqlCommand cmdCreateFund = new SqlCommand();
                                cmdCreateFund.Connection = con;
                                cmdCreateFund.Transaction = transaction;
                                cmdCreateFund.CommandText = "insert into tblFund values(@UserId, @Balance)";
                                cmdCreateFund.Parameters.AddWithValue("@UserId", UserId);
                                cmdCreateFund.Parameters.AddWithValue("@Balance", 0);
                                cmdCreateFund.ExecuteNonQuery();

                                SqlCommand cmdInsertTask = new SqlCommand();
                                cmdInsertTask.Connection = con;
                                cmdInsertTask.Transaction = transaction;
                                cmdInsertTask.CommandText = "insert into tblTask values(@UserId, @Expiry)";
                                cmdInsertTask.Parameters.AddWithValue("@UserId", UserId);
                                cmdInsertTask.Parameters.AddWithValue("@Expiry", DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy"));
                                cmdInsertTask.ExecuteNonQuery();

                                transaction.Commit();
                                string message = "User: " + fname + ", registered successfully with User Id: " + UserId + ", and Password: " + password;
                                var m = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(message);
                                var script = string.Format("alert({0});window.location.replace('Login.aspx?Mode=Redirect');", m);
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", script, true);
                            }
                            catch
                            {
                                transaction.Rollback();
                                Alert(GlobalClass.DatabaseError);
                            }
                        }
                    }
                    else
                        Alert("Please read and accept all the Privacy Policy and Terms and Conditions.");
                }
                else
                    Alert("Please enter a valid mobile number.");
            }
            else
                Alert("Please fill all fields.");
        }
    }
}