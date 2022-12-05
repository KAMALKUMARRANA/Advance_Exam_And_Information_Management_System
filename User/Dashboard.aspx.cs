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
        DataTable dt = GlobalClass.LoadUser(userId);
        string name = dt.Rows[0]["Name"].ToString().Trim();
        string id = dt.Rows[0]["UserId"].ToString().Trim();
        lblInfo.Text = name + " - " + id;
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
   
}