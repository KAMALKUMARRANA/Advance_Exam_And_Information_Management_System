using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Admin_Activation : System.Web.UI.Page
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
    private void ShowAdminInfo(string adminId)
    {
        DataTable dt = GlobalClass.LoadAdmin(adminId);
        lblId.Text = dt.Rows[0]["AdminId"].ToString().Trim();
        lblAdminName.Text = dt.Rows[0]["Name"].ToString().Trim();
    }
    private void SearchUser()
    {
        string userId = txtUserId.Text.Trim();
        if (userId != "")
        {
            DataTable dtUser = GlobalClass.LoadUser(userId);
            if (dtUser.Rows.Count != 0 && dtUser.Rows[0]["RefId"].ToString().Trim() != "LEADER")
            {
                if (dtUser.Rows[0]["Status"].ToString().Trim() != "Active")
                {
                    panelActivation.Visible = true;
                    lblUserId.Text = dtUser.Rows[0]["UserId"].ToString();
                    lblName.Text = dtUser.Rows[0]["Name"].ToString();
                    using (SqlConnection con = new SqlConnection(cs))
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = con;
                        cmd.CommandText = "select Package, Amount from tblPackage where NOT Package = 'FREE'";
                        con.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            ddlPackage.DataSource = rdr;
                            ddlPackage.DataTextField = "Package";
                            ddlPackage.DataValueField = "Amount";
                            ddlPackage.DataBind();
                        }
                    }
                    ddlPackage.Items.Insert(0, new ListItem("Select Package", "Select Package"));
                }
                else
                {
                    panelActivation.Visible = false;
                    Alert("User already active!");
                }
            }
            else
            {
                panelActivation.Visible = false;
                Alert("Invalid User Id!");
            }
        }
        else
        {
            panelActivation.Visible = false;
            Alert("Please enter User Id.");
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

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        SearchUser();
    }

    protected void ddlPackage_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlPackage.Items.Remove("Select Package");
        lblAmount.Text = ddlPackage.SelectedValue.ToString();
        lblAmount.ForeColor = System.Drawing.Color.Red;
    }

    protected void btnActivate_Click(object sender, EventArgs e)
    {
        string adminId = Session["ywivreqi"].ToString();
        string selectedPackage = ddlPackage.SelectedItem.ToString();
        if (selectedPackage != "Select Package" && selectedPackage != "FREE")
        {
            DataTable dtUser = GlobalClass.LoadUser(lblUserId.Text.Trim());
            if (dtUser.Rows[0]["Status"].ToString().Trim() != "Active")
            {
                if (dtUser.Rows[0]["RefId"].ToString().Trim() != "LEADER")
                {
                    int cBalance = GlobalClass.LoadCompanyBalance();
                    string today = GlobalClass.CurrentDateTime();

                    //User
                    string activationTxnNo = GlobalClass.GenerateActivationTxnNo();
                    string userId = dtUser.Rows[0]["UserId"].ToString().Trim();
                    string userName = dtUser.Rows[0]["Name"].ToString().Trim().Replace(" ", string.Empty).ToUpper();
                    if (userName.Length > 10) userName = userName.Substring(0, 10);
                    DataTable dtPackage = GlobalClass.LoadPackageInfo(selectedPackage);
                    string expiryDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.AddDays(Convert.ToInt32(dtPackage.Rows[0]["Validity"].ToString().Trim())), TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")).ToString("dd-MM-yyyy");
                    string package = dtPackage.Rows[0]["Package"].ToString().Trim();
                    int packageAmount = Convert.ToInt32(dtPackage.Rows[0]["Amount"].ToString().Trim());
                    int bonus = Convert.ToInt32(dtPackage.Rows[0]["Bonus"].ToString().Trim());

                    //Level_1 referrer
                    string L1RefUserId = GlobalClass.LoadUser(userId).Rows[0]["RefId"].ToString().Trim();
                    DataTable dtL1Ref = GlobalClass.LoadUser(L1RefUserId);
                    DataTable dtL1Package = GlobalClass.LoadPackageInfo(dtL1Ref.Rows[0]["Package"].ToString().Trim());
                    int L1Ref = Convert.ToInt32(dtL1Package.Rows[0]["L1Ref"].ToString().Trim());
                    int L1RefBalance = GlobalClass.LoadUserBalance(L1RefUserId);
                    int L1RefIncome = Convert.ToInt32(Math.Ceiling(packageAmount * L1Ref / 100.0));

                    //Level_2 referrer
                    string L2RefUserId = GlobalClass.LoadUser(L1RefUserId).Rows[0]["RefId"].ToString().Trim();
                    DataTable dtL2Ref = GlobalClass.LoadUser(L2RefUserId);
                    int L2Ref = 0;
                    int L2RefBalance = 0;
                    int L2RefIncome = 0;
                    if (dtL2Ref.Rows.Count != 0)
                    {
                        DataTable dtL2Package = GlobalClass.LoadPackageInfo(dtL2Ref.Rows[0]["Package"].ToString().Trim());
                        L2Ref = Convert.ToInt32(dtL2Package.Rows[0]["L2Ref"].ToString().Trim());
                        L2RefBalance = GlobalClass.LoadUserBalance(L2RefUserId);
                        L2RefIncome = Convert.ToInt32(Math.Ceiling(packageAmount * L2Ref / 100.0));
                    }

                    using (SqlConnection con = new SqlConnection(cs))
                    {
                        con.Open();
                        SqlTransaction transaction = con.BeginTransaction();
                        try
                        {
                            SqlCommand cmdUpdateUser = new SqlCommand();
                            cmdUpdateUser.Connection = con;
                            cmdUpdateUser.Transaction = transaction;
                            cmdUpdateUser.CommandText = "update tblUser set Status = @Status, Package = @Package where UserId = @UserId";
                            cmdUpdateUser.Parameters.AddWithValue("@Status", "Active");
                            cmdUpdateUser.Parameters.AddWithValue("@Package", package);
                            cmdUpdateUser.Parameters.AddWithValue("@UserId", userId);
                            cmdUpdateUser.ExecuteNonQuery();

                            SqlCommand cmdUpdateUserInfo = new SqlCommand();
                            cmdUpdateUserInfo.Connection = con;
                            cmdUpdateUserInfo.Transaction = transaction;
                            cmdUpdateUserInfo.CommandText = "update tblUserInfo set ExpDate = @ExpDate where UserId = @UserId";
                            cmdUpdateUserInfo.Parameters.AddWithValue("@ExpDate", expiryDate);
                            cmdUpdateUserInfo.Parameters.AddWithValue("@UserId", userId);
                            cmdUpdateUserInfo.ExecuteNonQuery();

                            if (bonus != 0)
                            {
                                SqlCommand cmdUpdateUserBalance = new SqlCommand();
                                cmdUpdateUserBalance.Connection = con;
                                cmdUpdateUserBalance.Transaction = transaction;
                                cmdUpdateUserBalance.CommandText = "update tblUserBalance set Balance = @Balance where UserId = @UserId";
                                cmdUpdateUserBalance.Parameters.AddWithValue("@Balance", bonus);
                                cmdUpdateUserBalance.Parameters.AddWithValue("@UserId", userId);
                                cmdUpdateUserBalance.ExecuteNonQuery();

                                SqlCommand cmdInsertUserBalanceHistory = new SqlCommand();
                                cmdInsertUserBalanceHistory.Connection = con;
                                cmdInsertUserBalanceHistory.Transaction = transaction;
                                cmdInsertUserBalanceHistory.CommandText = "insert into tblUserBalanceHistory values(@TxnDate, @UserId, @Type, @Amount, @Description)";
                                cmdInsertUserBalanceHistory.Parameters.AddWithValue("@TxnDate", today);
                                cmdInsertUserBalanceHistory.Parameters.AddWithValue("@UserId", userId);
                                cmdInsertUserBalanceHistory.Parameters.AddWithValue("@Type", "Cr");
                                cmdInsertUserBalanceHistory.Parameters.AddWithValue("@Amount", bonus);
                                cmdInsertUserBalanceHistory.Parameters.AddWithValue("@Description", "WELCOME_BONUS.");
                                cmdInsertUserBalanceHistory.ExecuteNonQuery();
                            }

                            //Update level_1 referrer
                            if (L1Ref != 0)
                            {
                                SqlCommand cmdUpdateL1RefBalance = new SqlCommand();
                                cmdUpdateL1RefBalance.Connection = con;
                                cmdUpdateL1RefBalance.Transaction = transaction;
                                cmdUpdateL1RefBalance.CommandText = "update tblUserBalance set Balance = @Balance where UserId = @UserId";
                                cmdUpdateL1RefBalance.Parameters.AddWithValue("@Balance", L1RefBalance + L1RefIncome);
                                cmdUpdateL1RefBalance.Parameters.AddWithValue("@UserId", L1RefUserId);
                                cmdUpdateL1RefBalance.ExecuteNonQuery();

                                SqlCommand cmdInsertL1RefBalanceHistory = new SqlCommand();
                                cmdInsertL1RefBalanceHistory.Connection = con;
                                cmdInsertL1RefBalanceHistory.Transaction = transaction;
                                cmdInsertL1RefBalanceHistory.CommandText = "insert into tblUserBalanceHistory values(@TxnDate, @UserId, @Type, @Amount, @Description)";
                                cmdInsertL1RefBalanceHistory.Parameters.AddWithValue("@TxnDate", today);
                                cmdInsertL1RefBalanceHistory.Parameters.AddWithValue("@UserId", L1RefUserId);
                                cmdInsertL1RefBalanceHistory.Parameters.AddWithValue("@Type", "Cr");
                                cmdInsertL1RefBalanceHistory.Parameters.AddWithValue("@Amount", L1RefIncome);
                                cmdInsertL1RefBalanceHistory.Parameters.AddWithValue("@Description", "REF_IN_L1_" + userName + "_" + userId);
                                cmdInsertL1RefBalanceHistory.ExecuteNonQuery();
                            }

                            //Update level_2 referrer
                            if (dtL2Ref.Rows.Count != 0)
                            {
                                if (L2Ref != 0)
                                {
                                    SqlCommand cmdUpdateL2RefBalance = new SqlCommand();
                                    cmdUpdateL2RefBalance.Connection = con;
                                    cmdUpdateL2RefBalance.Transaction = transaction;
                                    cmdUpdateL2RefBalance.CommandText = "update tblUserBalance set Balance = @Balance where UserId = @UserId";
                                    cmdUpdateL2RefBalance.Parameters.AddWithValue("@Balance", L2RefBalance + L2RefIncome);
                                    cmdUpdateL2RefBalance.Parameters.AddWithValue("@UserId", L2RefUserId);
                                    cmdUpdateL2RefBalance.ExecuteNonQuery();

                                    SqlCommand cmdInsertL2RefBalanceHistory = new SqlCommand();
                                    cmdInsertL2RefBalanceHistory.Connection = con;
                                    cmdInsertL2RefBalanceHistory.Transaction = transaction;
                                    cmdInsertL2RefBalanceHistory.CommandText = "insert into tblUserBalanceHistory values(@TxnDate, @UserId, @Type, @Amount, @Description)";
                                    cmdInsertL2RefBalanceHistory.Parameters.AddWithValue("@TxnDate", today);
                                    cmdInsertL2RefBalanceHistory.Parameters.AddWithValue("@UserId", L2RefUserId);
                                    cmdInsertL2RefBalanceHistory.Parameters.AddWithValue("@Type", "Cr");
                                    cmdInsertL2RefBalanceHistory.Parameters.AddWithValue("@Amount", L2RefIncome);
                                    cmdInsertL2RefBalanceHistory.Parameters.AddWithValue("@Description", "REF_IN_L2_" + userName + "_" + userId);
                                    cmdInsertL2RefBalanceHistory.ExecuteNonQuery();
                                }
                            }

                            SqlCommand cmdInsertActivationHistory = new SqlCommand();
                            cmdInsertActivationHistory.Connection = con;
                            cmdInsertActivationHistory.Transaction = transaction;
                            cmdInsertActivationHistory.CommandText = "insert into tblActivationHistory values(@TxnNo, @TxnDate, @Type, @Id, @UserId, @Package)";
                            cmdInsertActivationHistory.Parameters.AddWithValue("@TxnNo", activationTxnNo);
                            cmdInsertActivationHistory.Parameters.AddWithValue("@TxnDate", today);
                            cmdInsertActivationHistory.Parameters.AddWithValue("@Type", "Admin");
                            cmdInsertActivationHistory.Parameters.AddWithValue("@Id", adminId);
                            cmdInsertActivationHistory.Parameters.AddWithValue("@UserId", userId);
                            cmdInsertActivationHistory.Parameters.AddWithValue("@Package", package);
                            cmdInsertActivationHistory.ExecuteNonQuery();

                            SqlCommand cmdUpdateCompanyBalance = new SqlCommand();
                            cmdUpdateCompanyBalance.Connection = con;
                            cmdUpdateCompanyBalance.Transaction = transaction;
                            cmdUpdateCompanyBalance.CommandText = "update tblCompanyBalance set Balance = @Balance";
                            cmdUpdateCompanyBalance.Parameters.AddWithValue("@Balance", cBalance + packageAmount);
                            cmdUpdateCompanyBalance.ExecuteNonQuery();

                            transaction.Commit();
                            Reload("User: " + lblName.Text.Trim() + ", activated sucessfully.");
                        }
                        catch
                        {
                            transaction.Rollback();
                            Alert(GlobalClass.DatabaseError);
                        }
                    }
                }
                else
                    Alert("This user could not be activated!");
            }
            else
                Alert("User already active!");
        }
        else
            Alert("Please select a valid package.");
    }
}