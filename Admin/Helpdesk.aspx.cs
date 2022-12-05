using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Admin_Helpdesk : System.Web.UI.Page
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
    public void gvRequestLoad()
    {
        DataTable dt = new DataTable();
        using (SqlConnection con = new SqlConnection(cs))
        {
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand("select * from tblHelp where Status = 'Processing' order by CONVERT(date, RaiseDate , 105)", con);
            da.Fill(dt);
        }
        gvRequest.DataSource = dt;
        gvRequest.DataBind();
        if (dt.Rows.Count == 0)
        {
            Label lbl = gvRequest.Controls[0].Controls[0].FindControl("lblError") as Label;
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
                gvRequestLoad();
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
        lblRequestKey.InnerText = "Enter " + ddlCategory.SelectedItem.ToString();
    }

    protected void btnRequestSearch_Click(object sender, EventArgs e)
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
                    cmd.CommandText = "select * from tblHelp where " + ddl + " like @Key";
                    cmd.Parameters.AddWithValue("@Key", "%" + key + "%");
                    da.SelectCommand = cmd;
                    con.Open();
                    da.Fill(dt);
                }
                gvRequest.DataSource = dt;
                gvRequest.DataBind();
                if (dt.Rows.Count == 0)
                {
                    Label lbl = gvRequest.Controls[0].Controls[0].FindControl("lblError") as Label;
                    lbl.Text = "No data found.";
                }
            }
            else
                Alert("Please enter search keyword.");
        }
        else
            Alert("Please select a category.");
    }

    protected void gvRequest_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}