using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class User_Dashboard : System.Web.UI.Page
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
        string name = dtUser.Rows[0]["Name"].ToString().Trim();
        string id = dtUser.Rows[0]["UserId"].ToString().Trim();
        lblInfo.Text = name + " - " + id;
        lblStatus.Text = dtUser.Rows[0]["Status"].ToString().Trim();
        lblBalance.Text = GlobalClass.LoadUserBalance(userId).ToString();
        lblFund.Text = GlobalClass.LoadUserFund(userId).ToString();
        int team = GlobalClass.CountActiveTeam(userId);
        if (team > 0)
            lblBoost.Text = "Yes";
        else
            lblBoost.Text = "No";
        lblTeam.Text = team.ToString();
        if (dtUser.Rows[0]["RefId"].ToString().Trim() != "LEADER")
        {
            if (dtUser.Rows[0]["Status"].ToString().Trim() == "Active")
            {
                task.Visible = true;
                lblIncome.Text = GlobalClass.DailyIncome(userId).ToString();
            }
        }
        else
            DaiyIncomeTab.Visible = false;
        lblValidity.Text = GlobalClass.CheckUserValidity(userId);
    }
    private void ShowUpdate()
    {
        DataTable dt = new DataTable();
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select * from tblNotice";
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
        }
        StringBuilder updatehtml = new StringBuilder();
        updatehtml.Append("<ul>");
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                updatehtml.Append("<li>");
                updatehtml.Append(row["Message"].ToString().Trim());
                updatehtml.Append("<br>Updated on : ");
                updatehtml.Append(row["Date"].ToString().Trim());
                updatehtml.Append("</li>");
            }
        }
        else
        {
            updatehtml.Append("<li style='color: green;'>There is no new update.</li>");
        }
        updatehtml.Append("</ul>");
        update.InnerHtml += updatehtml.ToString();
    }
    private void ShowTaskInfo(string userId)
    {
        DataTable dtUser = GlobalClass.LoadUser(userId);
        if (dtUser.Rows[0]["RefId"].ToString().Trim() != "LEADER" && dtUser.Rows[0]["Status"].ToString().Trim() == "Active" && !GlobalClass.IsUserExpire(userId))
        {
            string day = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")).ToString("dddd");
            if (day != "Sunday")
            {
                if (GlobalClass.IsTaskPending(userId))
                {
                    lblDailyTask.Text = "Daily task is Pending...";
                    lblDailyTask.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    lblDailyTask.Text = "Daily Task Completed...";
                    lblDailyTask.ForeColor = System.Drawing.Color.Green;
                }
            }
            else
            {
                lblDailyTask.Text = "Today is Holiday.";
                lblDailyTask.ForeColor = System.Drawing.Color.Green;
            }
        }
        else
            task.Visible = false;
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
                    ShowUpdate();
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
}