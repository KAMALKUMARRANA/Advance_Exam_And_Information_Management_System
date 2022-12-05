using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class User_Fund : System.Web.UI.Page
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
        var script = string.Format("alert({0});window.location.replace('Withdrawal-details.aspx');", m);
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
    private void ShowUserInfo(string userId)
    {
        DataTable dt = GlobalClass.LoadUser(userId);
        string name = dt.Rows[0]["Name"].ToString().Trim();
        string id = dt.Rows[0]["UserId"].ToString().Trim();
        lblInfo.Text = name + " - " + id;
        lblUserId.Text = id;
        lblSender.InnerText = "Sender: " + name;
    }
    private void ShowUserBalance(string userId)
    {
        string balance = GlobalClass.LoadUserBalance(userId).ToString();
        lblBalance.Text = "Available Balance " + "<i class='fas fa-rupee-sign'></i> " + balance;
        lblAccountBalance.Text = balance;
    }
    private void ShowUserFund(string userId)
    {
        string fund = GlobalClass.LoadUserFund(userId).ToString();
        lblFund.Text = "Available Fund " + "<i class='fas fa-coins'></i> " + fund;
        lblFund1.Text = fund;
    }
    private void ShowWithdrawalTax(string userId)
    {
        DataTable dtPackage = GlobalClass.LoadPackageInfo(GlobalClass.LoadUser(userId).Rows[0]["Package"].ToString().Trim());
        lblTax.Text = dtPackage.Rows[0]["WithdrawalTax"].ToString().Trim();
    }
    public void gvFundHistoryLoad(string userId)
    {
        DataTable dt = new DataTable();
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select * from tblFundHistory where UserId = @UserId order by CONVERT(date, TxnDate , 105) DESC";
            cmd.Parameters.AddWithValue("@UserId", userId);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
        }
        gvHistory.DataSource = dt;
        gvHistory.DataBind();
        if (dt.Rows.Count == 0)
        {
            Label lbl = gvHistory.Controls[0].Controls[0].FindControl("lblError") as Label;
            lbl.Text = "No history foud.";
        }
    }
    public void ToggleTab(int state)
    {
        string Id = Request.Cookies["TVUSCK"]["xvhuqdph"].ToString();
        switch (state)
        {
            default:
                buttonFund.CssClass = "button active";
                buttonHistory.CssClass = "button";
                panelFund.Visible = true;
                panelHistory.Visible = false;
                ShowWithdrawalTax(Id);
                Session["pks29y"] = "0";
                break;
            case 1:
                buttonFund.CssClass = "button";
                buttonHistory.CssClass = "button active";
                panelFund.Visible = false;
                panelHistory.Visible = true;
                Session["pks29y"] = "1";
                gvFundHistoryLoad(Id);
                break;
        }
    }
    private void ShowError(string message)
    {
        error.Visible = true;
        success.Visible = false;
        lblMessage.Visible = true;
        lblMessage.InnerText = message;
        lblReceiver.InnerText = "Receiver: Name";
        txtFund.Enabled = btnSend.Enabled = false;
    }
    private void ShowSuccess()
    {
        error.Visible = false;
        success.Visible = true;
        lblMessage.Visible = false;
        txtFund.Enabled = btnSend.Enabled = true;
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
                    ShowUserBalance(userId);
                    ShowUserFund(userId);
                    if (Session["pks29y"] != null)
                    {
                        ToggleTab(Convert.ToInt32(Session["pks29y"].ToString()));
                    }
                    else
                        ToggleTab(0);
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

    protected void buttonFund_Click(object sender, EventArgs e)
    {
        ToggleTab(0);
    }

    protected void buttonHistory_Click(object sender, EventArgs e)
    {
        ToggleTab(1);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string receiverUserId = txtReceiverId.Text.Trim();
        if (receiverUserId != "")
        {
            string userId = Request.Cookies["TVUSCK"]["xvhuqdph"].ToString();
            if (userId != receiverUserId)
            {
                DataTable dt = GlobalClass.LoadUser(receiverUserId);
                if (dt.Rows.Count != 0)
                {
                    if (dt.Rows[0]["Status"].ToString().Trim() == "Active")
                    {
                        ShowSuccess();
                        lblReceiver.InnerText = "Receiver: " + dt.Rows[0]["Name"].ToString().Trim();
                    }
                    else
                        ShowError("Receiver is unable to receive fund!");
                }
                else
                    ShowError("Incorrect receiver user id!");
            }
            else
                ShowError("You can not transfer fund to your own account!");
        }
        else
            ShowError("Please enter receiver user id.");
    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        string senderId = Request.Cookies["TVUSCK"]["xvhuqdph"].ToString();
        string receiverId = txtReceiverId.Text.Trim();
        string senderName = GlobalClass.LoadUser(senderId).Rows[0]["Name"].ToString().Trim().Replace(" ", string.Empty).ToUpper();
        if (senderName.Length > 10) senderName = senderName.Substring(0, 10);
        string receiverName = GlobalClass.LoadUser(receiverId).Rows[0]["Name"].ToString().Trim().Replace(" ", string.Empty).ToUpper();
        if (receiverName.Length > 10) receiverName = receiverName.Substring(0, 10);
        int amount = Convert.ToInt32(txtFund.Text.Trim());
        int senderFund = GlobalClass.LoadUserFund(senderId);
        int receiverFund = GlobalClass.LoadUserFund(receiverId);
        string datetime = GlobalClass.CurrentDateTime();

        if (amount >= 100)
        {
            if (senderFund >= amount)
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();
                    try
                    {
                        SqlCommand cmdUpdateSenderFund = new SqlCommand();
                        cmdUpdateSenderFund.Connection = con;
                        cmdUpdateSenderFund.Transaction = transaction;
                        cmdUpdateSenderFund.CommandText = "update tblFund set Balance = @Balance where UserId = @UserId";
                        cmdUpdateSenderFund.Parameters.AddWithValue("@Balance", senderFund - amount);
                        cmdUpdateSenderFund.Parameters.AddWithValue("@UserId", senderId);
                        cmdUpdateSenderFund.ExecuteNonQuery();

                        SqlCommand cmdInsertSenderFundHistory = new SqlCommand();
                        cmdInsertSenderFundHistory.Connection = con;
                        cmdInsertSenderFundHistory.Transaction = transaction;
                        cmdInsertSenderFundHistory.CommandText = "insert into tblFundHistory values(@TxnDate, @UserId, @Type, @Amount, @Description)";
                        cmdInsertSenderFundHistory.Parameters.AddWithValue("@TxnDate", datetime);
                        cmdInsertSenderFundHistory.Parameters.AddWithValue("@UserId", senderId);
                        cmdInsertSenderFundHistory.Parameters.AddWithValue("@Type", "Dr");
                        cmdInsertSenderFundHistory.Parameters.AddWithValue("@Amount", amount);
                        cmdInsertSenderFundHistory.Parameters.AddWithValue("@Description", "SENT_TO_: " + receiverName + receiverId);
                        cmdInsertSenderFundHistory.ExecuteNonQuery();

                        SqlCommand cmdUpdateReceiverFund = new SqlCommand();
                        cmdUpdateReceiverFund.Connection = con;
                        cmdUpdateReceiverFund.Transaction = transaction;
                        cmdUpdateReceiverFund.CommandText = "update tblFund set Balance = @Balance where UserId = @UserId";
                        cmdUpdateReceiverFund.Parameters.AddWithValue("@Balance", receiverFund + amount);
                        cmdUpdateReceiverFund.Parameters.AddWithValue("@UserId", receiverId);
                        cmdUpdateReceiverFund.ExecuteNonQuery();

                        SqlCommand cmdInsertReceiverFundHistory = new SqlCommand();
                        cmdInsertReceiverFundHistory.Connection = con;
                        cmdInsertReceiverFundHistory.Transaction = transaction;
                        cmdInsertReceiverFundHistory.CommandText = "insert into tblFundHistory values(@TxnDate, @UserId, @Type, @Amount, @Description)";
                        cmdInsertReceiverFundHistory.Parameters.AddWithValue("@TxnDate", datetime);
                        cmdInsertReceiverFundHistory.Parameters.AddWithValue("@UserId", receiverId);
                        cmdInsertReceiverFundHistory.Parameters.AddWithValue("@Type", "Cr");
                        cmdInsertReceiverFundHistory.Parameters.AddWithValue("@Amount", amount);
                        cmdInsertReceiverFundHistory.Parameters.AddWithValue("@Description", "RECEIVED_FROM_" + senderName + senderId);
                        cmdInsertReceiverFundHistory.ExecuteNonQuery();

                        transaction.Commit();
                        Reload("Fund successfully transfered.");
                    }
                    catch
                    {
                        transaction.Rollback();
                        Alert(GlobalClass.DatabaseError);
                    }
                }
            }
            else
                Alert("Insufficient fund!");
        }
        else
            Alert("Minimum fund transfer amount is 100");
    }

    protected void txtConvertAmount_TextChanged(object sender, EventArgs e)
    {
        DataTable dtPackage = GlobalClass.LoadPackageInfo(GlobalClass.LoadUser(Request.Cookies["TVUSCK"]["xvhuqdph"].ToString()).Rows[0]["Package"].ToString().Trim());
        int amount = Convert.ToInt32(txtConvertAmount.Text.Trim());
        int txnAmount = amount + Convert.ToInt32(Math.Ceiling((amount * Convert.ToInt32(dtPackage.Rows[0]["WithdrawalTax"].ToString().Trim())) / 100.0));
        amountPanel.Visible = true;
        lblConvertAmount.Text = "Total Payable Amount = " + txnAmount;
    }

    protected void btnConvert_Click(object sender, EventArgs e)
    {
        string userId = Request.Cookies["TVUSCK"]["xvhuqdph"].ToString();
        int userBalance = GlobalClass.LoadUserBalance(userId);
        int userFund = GlobalClass.LoadUserFund(userId);
        DataTable dtPackage = GlobalClass.LoadPackageInfo(GlobalClass.LoadUser(userId).Rows[0]["Package"].ToString().Trim());
        int amount = Convert.ToInt32(txtConvertAmount.Text.Trim());
        int txnAmount = amount + Convert.ToInt32(Math.Ceiling((amount * Convert.ToInt32(dtPackage.Rows[0]["WithdrawalTax"].ToString().Trim())) / 100.0));
        string datetime = GlobalClass.CurrentDateTime();

        if (amount != 0)
        {
            if (amount >= 100)
            {
                if (userBalance >= txnAmount)
                {
                    using (SqlConnection con = new SqlConnection(cs))
                    {
                        con.Open();
                        SqlTransaction transaction = con.BeginTransaction();
                        try
                        {
                            SqlCommand cmdUpdateUserBalance = new SqlCommand();
                            cmdUpdateUserBalance.Connection = con;
                            cmdUpdateUserBalance.Transaction = transaction;
                            cmdUpdateUserBalance.CommandText = "update tblUserBalance set Balance = @Balance where UserId = @UserId";
                            cmdUpdateUserBalance.Parameters.AddWithValue("@Balance", userBalance - txnAmount);
                            cmdUpdateUserBalance.Parameters.AddWithValue("@UserId", userId);
                            cmdUpdateUserBalance.ExecuteNonQuery();

                            SqlCommand cmdInsertUserBalanceHistory = new SqlCommand();
                            cmdInsertUserBalanceHistory.Connection = con;
                            cmdInsertUserBalanceHistory.Transaction = transaction;
                            cmdInsertUserBalanceHistory.CommandText = "insert into tblUserBalanceHistory values(@TxnDate, @UserId, @Type, @Amount, @Description)";
                            cmdInsertUserBalanceHistory.Parameters.AddWithValue("@TxnDate", datetime);
                            cmdInsertUserBalanceHistory.Parameters.AddWithValue("@UserId", userId);
                            cmdInsertUserBalanceHistory.Parameters.AddWithValue("@Type", "Dr");
                            cmdInsertUserBalanceHistory.Parameters.AddWithValue("@Amount", txnAmount);
                            cmdInsertUserBalanceHistory.Parameters.AddWithValue("@Description", "CONVERTED_TO_FUND.");
                            cmdInsertUserBalanceHistory.ExecuteNonQuery();


                            SqlCommand cmdUpdateUserFund = new SqlCommand();
                            cmdUpdateUserFund.Connection = con;
                            cmdUpdateUserFund.Transaction = transaction;
                            cmdUpdateUserFund.CommandText = "update tblFund set Balance = @Balance where UserId = @UserId";
                            cmdUpdateUserFund.Parameters.AddWithValue("@Balance", userFund + amount);
                            cmdUpdateUserFund.Parameters.AddWithValue("@UserId", userId);
                            cmdUpdateUserFund.ExecuteNonQuery();

                            SqlCommand cmdInsertUserFundHistory = new SqlCommand();
                            cmdInsertUserFundHistory.Connection = con;
                            cmdInsertUserFundHistory.Transaction = transaction;
                            cmdInsertUserFundHistory.CommandText = "insert into tblFundHistory values(@TxnDate, @UserId, @Type, @Amount, @Description)";
                            cmdInsertUserFundHistory.Parameters.AddWithValue("@TxnDate", datetime);
                            cmdInsertUserFundHistory.Parameters.AddWithValue("@UserId", userId);
                            cmdInsertUserFundHistory.Parameters.AddWithValue("@Type", "Cr");
                            cmdInsertUserFundHistory.Parameters.AddWithValue("@Amount", amount);
                            cmdInsertUserFundHistory.Parameters.AddWithValue("@Description", "CONVERTED_FROM_BALANCE.");
                            cmdInsertUserFundHistory.ExecuteNonQuery();

                            transaction.Commit();
                            Reload("Balance successfully converted to fund.");
                        }
                        catch
                        {
                            transaction.Rollback();
                            Alert(GlobalClass.DatabaseError);
                        }
                    }
                }
                else
                    Alert("Insufficient balance!");
            }
            else
                Alert("Minimum convert amount is 100");
        }
        else
            Alert("Amount should not be 0 (Zero)!");
    }
}