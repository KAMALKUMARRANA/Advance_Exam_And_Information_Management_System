using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class User_Withdraw : System.Web.UI.Page
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
    public void ToggleTab(int state)
    {
        switch (state)
        {
            default:
                buttonRecharge.CssClass = "button";
                buttonWithdrawal.CssClass = "button active";
                panelMobileRecharge.Visible = false;
                panelWithdrawal.Visible = true;
                Session["eyf02z"] = "0";
                break;
            case 1:
                buttonRecharge.CssClass = "button active";
                buttonWithdrawal.CssClass = "button";
                panelMobileRecharge.Visible = true;
                panelWithdrawal.Visible = false;
                Session["eyf02z"] = "1";
                break;
        }
    }
    private void ShowUserInfo(string userId)
    {
        DataTable dt = GlobalClass.LoadUser(userId);
        string name = dt.Rows[0]["Name"].ToString().Trim();
        string id = dt.Rows[0]["UserId"].ToString().Trim();
        lblInfo.Text = name + " - " + id;
    }
    private void ShowUserBalance(string userId)
    {
        string balance = GlobalClass.LoadUserBalance(userId).ToString();
        lblBalance.Text = "Available Balance " + "<i class='fas fa-rupee-sign'></i> " + balance;
    }
    private void ShowUserDetails(string userId)
    {
        DataTable dtUserInfo = GlobalClass.LoadUserInfo(userId);
        if (dtUserInfo.Rows.Count > 0)
        {
            if (dtUserInfo.Rows[0]["Bank"].ToString().Trim() != "")
            {
                lblBank.InnerText = dtUserInfo.Rows[0]["Bank"].ToString().Trim();
                lblAccount.InnerText = dtUserInfo.Rows[0]["Account"].ToString().Trim();
                lblIfsc.InnerText = dtUserInfo.Rows[0]["IFSC"].ToString().Trim();
                btnWithdrawal.Attributes.Add("onclick", Confirm("Do you want to place a Withdraw request?"));
            }
            else
            {
                infoWithdrawal.Visible = false;
                formWithdrawal.Visible = true;
                withdrawalAmount.Visible = false;
                btnWithdrawal.Text = "Save";
                btnWithdrawal.Attributes.Add("onclick", Confirm("Do you want to save the details?"));
            }
        }
    }
    private void ShowWithdrawalTax(string userId)
    {
        DataTable dtPackage = GlobalClass.LoadPackageInfo(GlobalClass.LoadUser(userId).Rows[0]["Package"].ToString().Trim());
        lblTax1.Text = lblTax2.Text = dtPackage.Rows[0]["WithdrawalTax"].ToString().Trim();
    }
    private string GenerateWithdrawalTxnNo()
    {
        const String alpha = "0123456789";
        string TxnNo = "TXN";
        int exist = 1;
        do
        {
            Random ran = new Random();
            for (int i = 0; i < 7; i++)
            {
                int a = ran.Next(10);
                TxnNo = TxnNo + alpha.ElementAt(a);
            }
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "select count(TxnNo) from tblWithdraw where TxnNo = @TxnNo";
                cmd.Parameters.AddWithValue("@TxnNo", TxnNo);
                con.Open();
                exist = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        while (exist != 0);
        return TxnNo;
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
                    ShowWithdrawalTax(userId);
                    ShowUserDetails(userId);
                    if (Session["eyf02z"] != null)
                    {
                        ToggleTab(Convert.ToInt32(Session["eyf02z"].ToString()));
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

    protected void buttonWithdrawal_Click(object sender, EventArgs e)
    {
        ToggleTab(0);
    }

    protected void buttonRecharge_Click(object sender, EventArgs e)
    {
        ToggleTab(1);
        Alert("This features is coming soon.");
    }

    protected void linkAccoutDetails_Click(object sender, EventArgs e)
    {
        string userId = Request.Cookies["TVUSCK"]["xvhuqdph"].ToString();
        DataTable dt = GlobalClass.LoadUserInfo(userId);

        txtBank.Text = dt.Rows[0]["Bank"].ToString().Trim();
        txtAccount.Text = dt.Rows[0]["Account"].ToString().Trim();
        txtIfsc.Text = dt.Rows[0]["IFSC"].ToString().Trim();
        infoWithdrawal.Visible = false;
        formWithdrawal.Visible = true;
        withdrawalAmount.Visible = false;
        btnWithdrawal.Text = "Save";
        btnWithdrawal.Attributes.Add("onclick", Confirm("Do you want to save the details?"));
        txnWithdrawalContainer.Visible = false;
    }

    protected void txtWithdrawalAmount_TextChanged(object sender, EventArgs e)
    {
        DataTable dtPackage = GlobalClass.LoadPackageInfo(GlobalClass.LoadUser(Request.Cookies["TVUSCK"]["xvhuqdph"].ToString()).Rows[0]["Package"].ToString().Trim());
        int amount = Convert.ToInt32(txtWithdrawalAmount.Text.Trim());
        int txnAmount = amount + Convert.ToInt32(Math.Ceiling((amount * Convert.ToInt32(dtPackage.Rows[0]["WithdrawalTax"].ToString().Trim())) / 100.0));
        txnWithdrawalContainer.Visible = true;
        lblWithdrawalAmount.Text = "Total Payable Amount = " + txnAmount;
    }

    protected void btnWithdrawal_Click(object sender, EventArgs e)
    {
        string userId = Request.Cookies["TVUSCK"]["xvhuqdph"].ToString();
        if (btnWithdrawal.Text == "Withdraw")
        {
            string amount = txtWithdrawalAmount.Text.Trim();
            int txnAmount = Convert.ToInt32(amount);
            if (amount != "")
            {
                if (Convert.ToInt32(amount) >= 100)
                {
                    DataTable dtPackage = GlobalClass.LoadPackageInfo(GlobalClass.LoadUser(userId).Rows[0]["Package"].ToString().Trim());
                    txnAmount = txnAmount + Convert.ToInt32(Math.Ceiling((txnAmount * Convert.ToInt32(dtPackage.Rows[0]["WithdrawalTax"].ToString().Trim())) / 100.0));
                    int userBalance = GlobalClass.LoadUserBalance(userId);
                    if (userBalance >= txnAmount)
                    {
                        string withdrawalTxnNo = GenerateWithdrawalTxnNo();
                        string datetime = GlobalClass.CurrentDateTime();
                        DataTable dtUser = GlobalClass.LoadUser(userId);
                        DataTable dtUserInfo = GlobalClass.LoadUserInfo(userId);
                        userId = dtUser.Rows[0]["UserId"].ToString().Trim();
                        using (SqlConnection con = new SqlConnection(cs))
                        {
                            con.Open();
                            SqlTransaction transaction = con.BeginTransaction();
                            try
                            {
                                SqlCommand cmdUpdate = new SqlCommand();
                                cmdUpdate.Connection = con;
                                cmdUpdate.Transaction = transaction;
                                cmdUpdate.CommandText = "update tblUserBalance set Balance = @Balance where UserId = @UserId";
                                cmdUpdate.Parameters.AddWithValue("@Balance", userBalance - txnAmount);
                                cmdUpdate.Parameters.AddWithValue("@UserId", userId);
                                cmdUpdate.ExecuteNonQuery();

                                SqlCommand cmdInsertUserBalanceHistory = new SqlCommand();
                                cmdInsertUserBalanceHistory.Connection = con;
                                cmdInsertUserBalanceHistory.Transaction = transaction;
                                cmdInsertUserBalanceHistory.CommandText = "insert into tblUserBalanceHistory values(@TxnDate, @UserId, @Type, @Amount, @Description)";
                                cmdInsertUserBalanceHistory.Parameters.AddWithValue("@TxnDate", datetime);
                                cmdInsertUserBalanceHistory.Parameters.AddWithValue("@UserId", userId);
                                cmdInsertUserBalanceHistory.Parameters.AddWithValue("@Type", "Dr");
                                cmdInsertUserBalanceHistory.Parameters.AddWithValue("@Amount", txnAmount);
                                cmdInsertUserBalanceHistory.Parameters.AddWithValue("@Description", "BANK_WITHDRAWAL_" + withdrawalTxnNo);
                                cmdInsertUserBalanceHistory.ExecuteNonQuery();

                                SqlCommand cmdInsert = new SqlCommand();
                                cmdInsert.Connection = con;
                                cmdInsert.Transaction = transaction;
                                cmdInsert.CommandText = "insert into tblWithdraw values(@TxnNo, @TxnDate, @Status, NULL, @UserId, @Bank, @Account, @IFSC, @Amount, @TxnAmount)";
                                cmdInsert.Parameters.AddWithValue("@TxnNo", withdrawalTxnNo);
                                cmdInsert.Parameters.AddWithValue("@TxnDate", datetime);
                                cmdInsert.Parameters.AddWithValue("@Status", "Processing");
                                cmdInsert.Parameters.AddWithValue("@UserId", userId);
                                cmdInsert.Parameters.AddWithValue("@Bank", dtUserInfo.Rows[0]["Bank"].ToString().Trim());
                                cmdInsert.Parameters.AddWithValue("@Account", dtUserInfo.Rows[0]["Account"].ToString().Trim());
                                cmdInsert.Parameters.AddWithValue("@IFSC", dtUserInfo.Rows[0]["IFSC"].ToString().Trim());
                                cmdInsert.Parameters.AddWithValue("@Amount", Convert.ToInt32(amount));
                                cmdInsert.Parameters.AddWithValue("@TxnAmount", txnAmount);
                                cmdInsert.ExecuteNonQuery();
                                transaction.Commit();
                                string message = "Your Withdrawal request has been placed successfully with Transaction No: " + withdrawalTxnNo;
                                RedirectWithMessage(message);
                            }
                            catch
                            {
                                transaction.Rollback();
                                Alert(GlobalClass.DatabaseError);
                            }
                        }
                    }
                    else
                        Alert("Insufficiant balance!");
                }
                else
                    Alert("Minimum withdraw amount is 100");
            }
            else
                Alert("Please fill all fields.");
        }
        else if (btnWithdrawal.Text == "Save")
        {
            string bank = txtBank.Text.Trim();
            string account = txtAccount.Text.Trim();
            string cAccount = txtCAccount.Text.Trim();
            string ifsc = txtIfsc.Text.Trim();
            if (bank != "" && account != "" && cAccount != "" && ifsc != "")
            {
                if (account == cAccount)
                {
                    try
                    {
                        using (SqlConnection con = new SqlConnection(cs))
                        {
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = con;
                            cmd.CommandText = "update tblUserInfo set Bank = @Bank, Account = @Account, IFSC = @IFSC where UserId = @UserId";
                            cmd.Parameters.AddWithValue("@Bank", bank);
                            cmd.Parameters.AddWithValue("@Account", account);
                            cmd.Parameters.AddWithValue("@IFSC", ifsc);
                            cmd.Parameters.AddWithValue("@UserId", userId);
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                        Reload("Account details saved successfully.");
                    }
                    catch
                    {
                        Alert(GlobalClass.DatabaseError);
                    }
                }
                else
                    Alert("Account number does not match!");
            }
            else
                Alert("Please fill all the fields.");
        }
    }

    protected void txtRechargeAmount_TextChanged(object sender, EventArgs e)
    {
        DataTable dtPackage = GlobalClass.LoadPackageInfo(GlobalClass.LoadUser(Request.Cookies["TVUSCK"]["xvhuqdph"].ToString()).Rows[0]["Package"].ToString().Trim());
        int amount = Convert.ToInt32(txtRechargeAmount.Text.Trim());
        int txnAmount = amount + Convert.ToInt32(Math.Ceiling((amount * Convert.ToInt32(dtPackage.Rows[0]["WithdrawalTax"].ToString().Trim())) / 100.0));
        txnRechargeContainer.Visible = true;
        lblRechrgeAmount.Text = "Total Payable Amount = " + txnAmount;
    }

    protected void btnRecharge_Click(object sender, EventArgs e)
    {
        /*HttpCookie userCookies = Request.Cookies["userInfo"];
        string userId = userCookies["UserId"].ToString();
        string mobileno = txtMobile.Text.Trim();
        string op = ddlOperator.SelectedItem.ToString();
        string state = ddlState.SelectedItem.ToString();
        string amount = txtRechargeAmount.Text.Trim();
        int txnAmount = Convert.ToInt32(amount);
        if (mobileno != "" && op != "Select Operator" && state != "Select State" && amount != "")
        {
            if (mobileno.Length.ToString() == "10")
            {
                DataTable dtPackage = GlobalClass.LoadPackageInfo(GlobalClass.LoadUser(userId).Rows[0]["Package"].ToString().Trim());
                txnAmount = txnAmount + Convert.ToInt32(Math.Ceiling((txnAmount * Convert.ToInt32(dtPackage.Rows[0]["WithdrawalTax"].ToString().Trim())) / 100.0));
                if (Convert.ToInt32(amount) >= 10)
                {
                    int bal = GlobalClass.LoadUserBalance(userId);
                    if (bal >= txnAmount)
                    {
                        DataTable dtUser = GlobalClass.LoadUser(userId);
                        string orderId = GenerateOrderId();
                        using (SqlConnection con = new SqlConnection(cs))
                        {
                            con.Open();
                            SqlTransaction transaction = con.BeginTransaction();
                            try
                            {
                                SqlCommand cmdUpdate = new SqlCommand();
                                cmdUpdate.Connection = con;
                                cmdUpdate.Transaction = transaction;
                                cmdUpdate.CommandText = "update tblUserBalance set Balance = @Balance where UserId = @UserId";
                                cmdUpdate.Parameters.AddWithValue("@Balance", bal - txnAmount);
                                cmdUpdate.Parameters.AddWithValue("@UserId", dtUser.Rows[0]["UserId"].ToString().Trim());
                                cmdUpdate.ExecuteNonQuery();

                                SqlCommand cmdInsert = new SqlCommand();
                                cmdInsert.Connection = con;
                                cmdInsert.Transaction = transaction;
                                cmdInsert.CommandText = "insert into tblRecharge values(@OrderID, @OrderDate, @Status, NULL, @UserId, @MobileNumber, @Operator, @State, @Amount, @TxnAmount)";
                                cmdInsert.Parameters.AddWithValue("@OrderID", orderId);
                                cmdInsert.Parameters.AddWithValue("@OrderDate", TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")).ToString("dd-MM-yyyy HH:mm:ss"));
                                cmdInsert.Parameters.AddWithValue("@Status", "Processing");
                                cmdInsert.Parameters.AddWithValue("@UserId", dtUser.Rows[0]["UserId"].ToString().Trim());
                                cmdInsert.Parameters.AddWithValue("@MobileNumber", mobileno);
                                cmdInsert.Parameters.AddWithValue("@Operator", op);
                                cmdInsert.Parameters.AddWithValue("@State", state);
                                cmdInsert.Parameters.AddWithValue("@Amount", Convert.ToInt32(amount));
                                cmdInsert.Parameters.AddWithValue("@TxnAmount", txnAmount);
                                cmdInsert.ExecuteNonQuery();
                                transaction.Commit();
                                string message = "Your Recharge request has been placed successfully with OrderId: " + orderId;
                                RedirectWithMessage(message);
                            }
                            catch
                            {
                                transaction.Rollback();
                                Alert("Unable to process this request. Please try again.");
                            }
                        }
                    }
                    else
                        Alert("You do not have sufficient balance in your account for Recharge.");
                }
                else
                    Alert("Please enter amount more than 10");
            }
            else
                Alert("Please enter a valid mobile number!");
        }
        else
            Alert("Please fill all fields!");*/
        Alert("This features is coming soon.");
    }
}