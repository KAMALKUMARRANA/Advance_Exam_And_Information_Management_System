using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Admin_Users : System.Web.UI.Page
{
    string cs = GlobalClass.cs;

    public void Alert(string message)
    {
        var m = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(message);
        var script = string.Format("alert({0});", m);
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
    public void gvShow()
    {
        DataTable dt = new DataTable();
        gvUser.DataSource = dt;
        gvUser.DataBind();
        Label lbl = gvUser.Controls[0].Controls[0].FindControl("lblError") as Label;
        lbl.Text = "Search user.";
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
                gvShow();
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

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlCategory.Items.Remove("Select a option");
        lblKey.InnerText = "Enter " + ddlCategory.SelectedItem.ToString();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string ddl = ddlCategory.SelectedValue.ToString();
        if (ddl != "Select a option")
        {
            string key = txtKey.Text.Trim();
            if (key != "")
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlDataAdapter da = new SqlDataAdapter();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "select * from tblUser where " + ddl + " like @Key";
                    cmd.Parameters.AddWithValue("@Key", "%" + key + "%");
                    da.SelectCommand = cmd;
                    con.Open();
                    da.Fill(dt);
                }
                gvUser.DataSource = dt;
                gvUser.DataBind();
                if (dt.Rows.Count == 0)
                {
                    Label lbl = gvUser.Controls[0].Controls[0].FindControl("lblError") as Label;
                    lbl.Text = "No data found.";
                }
            }
            else
                Alert("Please enter search keyword.");
        }
        else
            Alert("Please select a category.");
    }

    protected void gvUser_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvUser.EditIndex = e.NewEditIndex;
        btnSearch_Click(sender, e);
        gvUser.EditRowStyle.BackColor = System.Drawing.Color.LightPink;
    }

    protected void gvUser_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvUser.EditIndex = -1;
        btnSearch_Click(sender, e);
    }

    protected void gvUser_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string name = (gvUser.Rows[e.RowIndex].FindControl("txtEditName") as TextBox).Text.Trim();
        string mobile = (gvUser.Rows[e.RowIndex].FindControl("txtEditMobile") as TextBox).Text.Trim();
        string email = (gvUser.Rows[e.RowIndex].FindControl("txtEditEmail") as TextBox).Text.Trim();
        string userId = gvUser.DataKeys[e.RowIndex].Value.ToString();
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
                    gvUser.EditIndex = -1;
                    btnSearch_Click(sender, e);
                    string message = "User updated successfully.";
                    Alert(message);
                    gvUser.Rows[e.RowIndex].BackColor = System.Drawing.Color.LightGreen;
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