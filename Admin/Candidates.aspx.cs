using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Admin_Leaders : System.Web.UI.Page
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
    public string Confirm(string message)
    {
        System.Text.StringBuilder str = new System.Text.StringBuilder();
        str.Append("return confirm('");
        str.Append(message);
        str.Append("')");
        return (str.ToString());
    }
    private void ShowAdminInfo(string adminId)
    {
        DataTable dt = GlobalClass.LoadAdmin(adminId);
        lblId.Text = dt.Rows[0]["AdminId"].ToString().Trim();
        lblName.Text = dt.Rows[0]["Name"].ToString().Trim();
    }
    private string CreateLeaderID()
    {
        System.Text.StringBuilder UserId = new System.Text.StringBuilder();
        const string alpha = "0123456789";
        string prefix = "CAND";
        string postfix = "";
        int exist = 1;
        do
        {
            Random ran = new Random();
            for (int i = 0; i < 4; i++)
            {
                int a = ran.Next(10);
                postfix = postfix + alpha.ElementAt(a);
            }
            UserId.Append(prefix);
            UserId.Append(postfix);

            DataTable dtUser = GlobalClass.LoadUser(UserId.ToString());
            exist = dtUser.Rows.Count;
        }
        while (exist != 0);
        return UserId.ToString();
    }
    public void gvLeaderLoad()
    {
        DataTable dt = new DataTable();
        using (SqlConnection con = new SqlConnection(cs))
        {
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand("select * from tblUser where RefId = 'LEADER'", con);
            da.Fill(dt);
        }
        gvLeader.DataSource = dt;
        gvLeader.DataBind();
        if (dt.Rows.Count == 0)
        {
            Label lbl = gvLeader.Controls[0].Controls[0].FindControl("lblError") as Label;
            lbl.Text = "No data found.";
        }
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
                gvLeaderLoad();
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

    protected void linkAdd_Click(object sender, EventArgs e)
    {
        panelForm.Visible = true;
        panelView.Visible = false;
    }

    protected void btnRegister_Click(object sender, EventArgs e)
    {
        string name = txtName.Text.Trim();
        string mobile = txtMobile.Text.Trim();
        string email = txtEmail.Text.Trim();
        string gender = ddlGender.SelectedItem.ToString();
        string dob = txtDOB.Text.Trim();
        string password = txtPassword.Text.Trim();
        if (name != "" && mobile != "" && email != "" && gender != "Gender" && dob != "" && password != "")
        {
            dob = Convert.ToDateTime(dob).ToString("dd-MM-yyyy");
            string UserId = CreateLeaderID();
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
                    cmdInsertUser.Parameters.AddWithValue("@RefId", "LEADER");
                    cmdInsertUser.Parameters.AddWithValue("@UserId", UserId);
                    cmdInsertUser.Parameters.AddWithValue("@Password", password);
                    cmdInsertUser.Parameters.AddWithValue("@Status", "Active");
                    cmdInsertUser.Parameters.AddWithValue("@Package", "FREE");
                    cmdInsertUser.Parameters.AddWithValue("@Name", name);
                    cmdInsertUser.Parameters.AddWithValue("@Mobile", mobile);
                    cmdInsertUser.Parameters.AddWithValue("@Email", email);
                    cmdInsertUser.ExecuteNonQuery();

                    SqlCommand cmdInsertInfo = new SqlCommand();
                    cmdInsertInfo.Connection = con;
                    cmdInsertInfo.Transaction = transaction;
                    cmdInsertInfo.CommandText = "insert into tblUserInfo values(@UserId, @ExpDate, NULL, @DOB, @Gender, NULL, NULL, NULL, NULL, @KycVerified, @MobileVerified, @EmailVerified)";
                    cmdInsertInfo.Parameters.AddWithValue("@UserId", UserId);
                    cmdInsertInfo.Parameters.AddWithValue("@ExpDate", TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.AddDays(365), TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")).ToString("dd-MM-yyyy"));
                    cmdInsertInfo.Parameters.AddWithValue("@DOB", dob);
                    cmdInsertInfo.Parameters.AddWithValue("@Gender", gender);
                    cmdInsertInfo.Parameters.AddWithValue("@KycVerified", "True");
                    cmdInsertInfo.Parameters.AddWithValue("@MobileVerified", "True");
                    cmdInsertInfo.Parameters.AddWithValue("@EmailVerified", "True");
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
                    cmdInsertTask.Parameters.AddWithValue("@Expiry", TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.AddDays(-1), TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")).ToString("dd-MM-yyyy"));
                    cmdInsertTask.ExecuteNonQuery();

                    transaction.Commit();
                    string message = "Leader: " + name + ", registered successfully with User Id: " + UserId + ", and Password: " + password;
                    Reload(message);
                }
                catch
                {
                    transaction.Rollback();
                    Alert(GlobalClass.DatabaseError);
                }
            }
        }
        else
            Alert("Please fill all fields.");
    }

    protected void linkCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.AbsoluteUri);
    }

    protected void gvLeader_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvLeader.EditIndex = e.NewEditIndex;
        gvLeaderLoad();
        gvLeader.EditRowStyle.BackColor = System.Drawing.Color.LightPink;
    }

    protected void gvLeader_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvLeader.EditIndex = -1;
        gvLeaderLoad();
    }

    protected void gvLeader_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string userId = gvLeader.DataKeys[e.RowIndex].Value.ToString();
        string name = (gvLeader.Rows[e.RowIndex].FindControl("txtEditName") as TextBox).Text.Trim();
        string mobile = (gvLeader.Rows[e.RowIndex].FindControl("txtEditMobile") as TextBox).Text.Trim();
        string email = (gvLeader.Rows[e.RowIndex].FindControl("txtEditEmail") as TextBox).Text.Trim();

        if (name != "" && mobile != "" && email != "")
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                try
                {
                    SqlCommand cmdUpdateUser = new SqlCommand();
                    cmdUpdateUser.Connection = con;
                    cmdUpdateUser.Transaction = transaction;
                    cmdUpdateUser.CommandText = "update tblUser set Name = @Name, Mobile = @Mobile, Email = @Email where UserId = @UserId";
                    cmdUpdateUser.Parameters.AddWithValue("@Name", name);
                    cmdUpdateUser.Parameters.AddWithValue("@Mobile", mobile);
                    cmdUpdateUser.Parameters.AddWithValue("@Email", email);
                    cmdUpdateUser.Parameters.AddWithValue("@UserId", userId);
                    cmdUpdateUser.ExecuteNonQuery();

                    transaction.Commit();
                    gvLeader.EditIndex = -1;
                    gvLeaderLoad();
                    string message = "Leader updated successfully.";
                    Alert(message);
                    gvLeader.Rows[e.RowIndex].BackColor = System.Drawing.Color.LightGreen;
                }
                catch
                {
                    transaction.Rollback();
                    Alert(GlobalClass.DatabaseError);
                }
            }
        }
        else
            Alert("Please fill all fields.");
    }
}