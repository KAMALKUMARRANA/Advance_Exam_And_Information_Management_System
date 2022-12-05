using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

public partial class User_Task : System.Web.UI.Page
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
    public void RedirectWithMessage(string msg)
    {
        var m = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(msg);
        var script = string.Format("alert({0});window.location.replace('Dashboard.aspx');", m);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", script, true);
    }
    private void ShowUserInfo(string userId)
    {
        DataTable dtUserInfo = GlobalClass.LoadUser(userId);
        string name = dtUserInfo.Rows[0]["Name"].ToString().Trim();
        string id = dtUserInfo.Rows[0]["UserId"].ToString().Trim();
        lblInfo.Text = name + " - " + id;
    }
    private void ShowTaskInfo(string userId)
    {
        DataTable dtUser = GlobalClass.LoadUser(userId);
        if (dtUser.Rows[0]["RefId"].ToString().Trim() != "LEADER")
        {
            if (dtUser.Rows[0]["Status"].ToString().Trim() == "Active")
            {
                if (!GlobalClass.IsUserExpire(userId))
                {
                    string day = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")).ToString("dddd");
                    if (day != "Sunday")
                    {
                        if (GlobalClass.IsTaskPending(userId))
                        {
                            lblMessage.Visible = false;
                            btnCheckIn.Visible = true;
                        }
                        else
                        {
                            lblMessage.Visible = true;
                            lblMessage.Text = "Daily Task Completed...";
                            lblMessage.ForeColor = System.Drawing.Color.Green;
                            btnCheckIn.Visible = false;
                        }
                    }
                    else
                    {
                        lblMessage.Text = "Today is Holiday.";
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                    }
                }
                else
                {
                    lblMessage.Text = "Your account validity has expired.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
            }
            else
            {
                lblMessage.Text = "Activate your account for Daily Task.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }
        else
            RedirectWithMessage("Daily task is not available for leaders.");
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
                    ShowTaskInfo(userId);
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

    protected void btnCheckIn_Click(object sender, EventArgs e)
    {
        string userId = Request.Cookies["TVUSCK"]["xvhuqdph"].ToString();
        Thread.Sleep(2000);
        if (GlobalClass.LoadUser(userId).Rows[0]["Status"].ToString().Trim() == "Active")
        {
            if (GlobalClass.IsTaskPending(userId))
            {
                string today = GlobalClass.CurrentDateOnly();
                int balance = GlobalClass.LoadUserBalance(userId);
                int income = GlobalClass.DailyIncome(userId);
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();
                    try
                    {
                        SqlCommand cmdUpdateTask = new SqlCommand();
                        cmdUpdateTask.Connection = con;
                        cmdUpdateTask.Transaction = transaction;
                        cmdUpdateTask.CommandText = "update tblTask set Expiry = @Expiry where UserId = @UserId";
                        cmdUpdateTask.Parameters.AddWithValue("@Expiry", today);
                        cmdUpdateTask.Parameters.AddWithValue("@UserId", userId);
                        cmdUpdateTask.ExecuteNonQuery();

                        SqlCommand cmdUpdateBalance = new SqlCommand();
                        cmdUpdateBalance.Connection = con;
                        cmdUpdateBalance.Transaction = transaction;
                        cmdUpdateBalance.CommandText = "update tblUserBalance set Balance = @Balance where UserId = @UserId";
                        cmdUpdateBalance.Parameters.AddWithValue("@Balance", balance + income);
                        cmdUpdateBalance.Parameters.AddWithValue("@UserId", userId);
                        cmdUpdateBalance.ExecuteNonQuery();

                        SqlCommand cmdInsertUserBalanceHistory = new SqlCommand();
                        cmdInsertUserBalanceHistory.Connection = con;
                        cmdInsertUserBalanceHistory.Transaction = transaction;
                        cmdInsertUserBalanceHistory.CommandText = "insert into tblUserBalanceHistory values(@TxnDate, @UserId, @Type, @Amount, @Description)";
                        cmdInsertUserBalanceHistory.Parameters.AddWithValue("@TxnDate", GlobalClass.CurrentDateTime());
                        cmdInsertUserBalanceHistory.Parameters.AddWithValue("@UserId", userId);
                        cmdInsertUserBalanceHistory.Parameters.AddWithValue("@Type", "Cr");
                        cmdInsertUserBalanceHistory.Parameters.AddWithValue("@Amount", income);
                        cmdInsertUserBalanceHistory.Parameters.AddWithValue("@Description", "DAILY_INCOME.");
                        cmdInsertUserBalanceHistory.ExecuteNonQuery();

                        transaction.Commit();
                        Reload("Check in successfully.");
                    }
                    catch
                    {
                        transaction.Rollback();
                        Alert(GlobalClass.DatabaseError);
                    }
                }
            }
            else
                Alert("Unable to check in! Please try again later.");
        }
        else
            Alert(GlobalClass.DatabaseError);
    }
}