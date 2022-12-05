using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class User_Refer : System.Web.UI.Page
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
        DataTable dt = GlobalClass.LoadUser(userId);
        string name = dt.Rows[0]["Name"].ToString().Trim();
        string id = dt.Rows[0]["UserId"].ToString().Trim();
        lblInfo.Text = name + " - " + id;
        lblRefId.Text = id;
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

    protected void btnShare_Click(object sender, EventArgs e)
    {
        string userId = Request.Cookies["TVUSCK"]["xvhuqdph"].ToString();
        string url = "https://touchandview.in/User/Registration.aspx?Refercode=" + userId;
        string text = "Join%20me%20on%20Touch%20%26%20View%20and%20start%20Earning%20Real%20Cash%20today%2E%0A%0A1%2E%20Click%20here%20%F0%9F%91%89%F0%9F%8F%BB%20" + url + "%0A%0A2%2E%20Register%20using%20my%20referral%20code%3A%20*" + userId + "*%0A%0A3%2E%20Get%20Exciting%20Welcome%20Bonus%20%26%20Start%20Earning%2E";
        Response.Redirect("whatsapp://send?text=" + text);
    }

    protected void linkJoin_Click(object sender, EventArgs e)
    {
        string userId = Request.Cookies["TVUSCK"]["xvhuqdph"].ToString();
        string url = "Registration.aspx?Refercode=" + userId;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("<script>");
        sb.Append("window.open('" + url + "', '_blank');");
        sb.Append("</script>");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", sb.ToString(), false);
    }
}