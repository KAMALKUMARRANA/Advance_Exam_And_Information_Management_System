using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class User_Team_view : System.Web.UI.Page
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
    }
    private void ShowUser(string userId)
    {
        DataTable dtUser = GlobalClass.LoadUser(userId);
        if (dtUser.Rows.Count > 0)
        {
            lblId.Text = dtUser.Rows[0]["UserId"].ToString().Trim();
            lblName.Text = dtUser.Rows[0]["Name"].ToString().Trim();
            if (GlobalClass.CheckBoost(userId))
                lblBoost.Text = "Boosted";
            else
                lblBoost.Text = "Not Boosted";
            if (dtUser.Rows[0]["Status"].ToString().Trim() == "Active")
            {
                listUser.Attributes.Add("class", listUser.Attributes["class"] + " active");
                tagStatus.Attributes.Add("class", tagStatus.Attributes["class"] + " active");
                icon.Attributes.Add("class", icon.Attributes["class"] + " active");
                tagStatus.InnerText = "Active";
            }
            else
            {
                listUser.Attributes["class"] = listUser.Attributes["class"].Replace("active", "").Trim();
                tagStatus.Attributes["class"] = tagStatus.Attributes["class"].Replace("active", "").Trim();
                icon.Attributes["class"] = icon.Attributes["class"].Replace("active", "").Trim();
                tagStatus.InnerText = "Inactive";
            }
        }
    }
    private void gvTeamLoad(string userId)
    {
        DataTable dt = new DataTable();
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select * from tblUser where RefId = @RefId";
            cmd.Parameters.AddWithValue("@RefId", userId);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
        }
        gvTeam.DataSource = dt;
        gvTeam.DataBind();
        if (dt.Rows.Count == 0)
        {
            Label lbl = gvTeam.Controls[0].Controls[0].FindControl("lblError") as Label;
            lbl.Text = "You have not joined anyone.";
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
                    ShowUserInfo(userId);
                    ShowUser(userId);
                    gvTeamLoad(userId);
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

    protected void gvTeam_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void gvTeam_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        ShowUser(gvTeam.DataKeys[e.RowIndex].Value.ToString());
        gvTeamLoad(gvTeam.DataKeys[e.RowIndex].Value.ToString());
    }
}