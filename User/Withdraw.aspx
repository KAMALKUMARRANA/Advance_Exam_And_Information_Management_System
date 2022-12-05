<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Withdraw.aspx.cs" Inherits="User_Withdraw" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Touch & View | Withdraw Cash</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="CSS/Withdraw.css" rel="stylesheet" type="text/css" />
    <%
        string path = HttpContext.Current.Server.MapPath("Link.txt");
        string content = System.IO.File.ReadAllText(path);
        Response.Write(content);
    %>
    <script src="JS/Withdraw.js" type="text/javascript"></script>
</head>
<body>
    <form id="redeem" runat="server">
        <asp:ScriptManager ID="scriptPage" runat="server">
        </asp:ScriptManager>
        <asp:UpdateProgress ID="updateProgress" AssociatedUpdatePanelID="pageUpdate" runat="server">
            <ProgressTemplate>
                <div id="loadingPanel">
                    <div id="loader">
                        <svg class="circular" viewbox="25 25 50 50">
                            <circle class="path" cx="50" cy="50" r="20" fill="none" stroke-width="3"></circle>
                        </svg>
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <%
            string headerpath = HttpContext.Current.Server.MapPath("Html/Header.htm");
            string headercontent = System.IO.File.ReadAllText(headerpath);
            Response.Write(headercontent);
        %>

        <asp:UpdatePanel ID="pageUpdate" runat="server">
            <ContentTemplate>
                <div id="body">
                    <div class="status">
                        <i class="fas fa-angle-double-right"></i>
                        <asp:Label ID="status" runat="server" Text="Withdraw Cash"></asp:Label>
                        <asp:Label ID="lblInfo" runat="server" Text="name (id)"></asp:Label>
                    </div>
                    <div class="balance">
                        <asp:Label ID="lblBalance" runat="server" Text="Account Balance"></asp:Label>
                    </div>

                    <div id="content">
                        <div id="btnContainer">
                            <asp:LinkButton ID="buttonWithdrawal" CssClass="button active" runat="server" OnClick="buttonWithdrawal_Click">
                                <i class="fas fa-university"></i>Withdraw
                            </asp:LinkButton>
                            <asp:LinkButton ID="buttonRecharge" CssClass="button" runat="server" OnClick="buttonRecharge_Click">
                                <i class="fas fa-mobile-alt"></i>Recharge
                            </asp:LinkButton>
                        </div>

                        <asp:Panel CssClass="tabPanel" ID="panelWithdrawal" runat="server" Visible="true">
                            <div class="intro">
                                <h>Withdraw Cash</h>
                                <ul>
                                    <li>Minimum withdrawal amount is 100</li>
                                    <li><asp:Label ID="lblTax1" runat="server" Text="TDS"></asp:Label> % TDS will be charged.</li>
                                    <li>Withdrawal request will be processed within 5-7 business working days.</li>
                                    <li>Make sure that your name and account holder name are same.</li>
                                    <li>In case withdrawal failure the amount will be credited back to your account within 72 hours.</li>
                                    <li>In case of incorrect withdrawal company will not be liable **</li>
                                </ul>
                            </div>
                            <asp:Panel ID="infoWithdrawal" CssClass="infoPanel" runat="server">
                                <div id="containerBank" class="row" runat="server">
                                    <div>Bank Name</div>
                                    <div>:</div>
                                    <span id="lblBank" runat="server">N/A</span>
                                </div>
                                <div id="containerAccount" class="row" runat="server">
                                    <div>Account No</div>
                                    <div>:</div>
                                    <span id="lblAccount" runat="server">N/A</span>
                                </div>
                                <div id="containerIfsc" class="row" runat="server">
                                    <div>IFSC Code</div>
                                    <div>:</div>
                                    <span id="lblIfsc" runat="server">N/A</span>
                                </div>
                                <asp:LinkButton ID="linkAccoutDetails" class="link" runat="server" OnClick="linkAccoutDetails_Click">Edit</asp:LinkButton>
                            </asp:Panel>

                            <asp:Panel ID="formWithdrawal" runat="server" Visible="false">
                                <div class="input-box">
                                    <asp:TextBox ID="txtBank" class="input" type="text" autocomplete="off" runat="server" required></asp:TextBox>
                                    <label for="txtBank" class="label-name">
                                        <div class="icon"><i class="fas fa-university"></i></div>
                                        <span class="content-name">Bank Name</span>
                                    </label>
                                </div>
                                <div class="input-box">
                                    <asp:TextBox ID="txtAccount" class="input" type="text" autocomplete="off" runat="server" required
                                        onkeypress='validate(event)' MaxLength="30"></asp:TextBox>
                                    <label for="txtAccount" class="label-name">
                                        <div class="icon"><i class="fas fa-book-open"></i></div>
                                        <span id="placeholderAccount" class="content-name">Bank Account Number</span>
                                    </label>
                                </div>
                                <div class="input-box">
                                    <asp:TextBox ID="txtCAccount" class="input" type="password" autocomplete="off" runat="server" required
                                        onkeypress='validate(event)' MaxLength="30"></asp:TextBox>
                                    <label for="txtCAccount" class="label-name">
                                        <div class="icon"><i class="fas fa-book-open"></i></div>
                                        <span id="placeholderCAccount" class="content-name">Confirm Account Number</span>
                                    </label>
                                </div>
                                <div class="input-box">
                                    <asp:TextBox ID="txtIfsc" class="input" type="text" autocomplete="off" runat="server" required
                                        MaxLength="11"></asp:TextBox>
                                    <label for="txtIfsc" class="label-name">
                                        <div class="icon"><i class="fas fa-key"></i></div>
                                        <span class="content-name">IFSC Code</span>
                                    </label>
                                </div>
                            </asp:Panel>

                            <div class="input-box" id="withdrawalAmount" runat="server">
                                <asp:TextBox ID="txtWithdrawalAmount" CssClass="input" type="text" autocomplete="off" runat="server" required
                                    onkeypress='validate(event)' MaxLength="4" ToolTip="Enter amount" AutoPostBack="true"
                                    OnTextChanged="txtWithdrawalAmount_TextChanged"></asp:TextBox>
                                <label for="txtWithdrawalAmount" class="label-name">
                                    <div class="icon"><i class="fas fa-rupee-sign"></i></div>
                                    <span class="content-name">Amount</span>
                                </label>
                            </div>
                            <div id="txnWithdrawalContainer" class="infoPanel amount" visible="false" runat="server">
                                <asp:Label ID="lblWithdrawalAmount" runat="server" Text="N/A"></asp:Label>
                            </div>
                            <div class="btnContainer">
                                <asp:Button ID="btnWithdrawal" CssClass="btn" runat="server" Text="Withdraw" OnClick="btnWithdrawal_Click" />
                            </div>
                        </asp:Panel>

                        <asp:Panel CssClass="tabPanel" ID="panelMobileRecharge" Visible="false" runat="server">
                            <div class="intro">
                                <h>Mobile Recharge</h>
                                <ul>
                                    <li>Minimum recharge amount is 10</li>
                                    <li><asp:Label ID="lblTax2" runat="server" Text="TDS"></asp:Label> % TDS will be charged.</li>
                                    <li>Recharge request will be processed within 72 hours.</li>
                                    <li>We support most recharge denominations, kindly verify with your operator before proceeding.</li>
                                    <li>In case recharge failure the amount will be credited back to your account within 72 hours.</li>
                                    <li>In case of incorrect recharge company will not be liable **</li>
                                </ul>
                            </div>
                            <asp:Panel ID="formRecharge" runat="server">
                                <div class="input-box">
                                    <asp:TextBox ID="txtMobile" class="input" type="text" autocomplete="off" runat="server" required
                                        onkeypress='validate(event)' MaxLength="10"></asp:TextBox>
                                    <label for="txtMobile" class="label-name">
                                        <div class="icon"><i class="fas fa-mobile-alt"></i></div>
                                        <span class="content-name">Mobile Number</span>
                                    </label>
                                </div>
                                <div class="input-box">
                                    <asp:DropDownList ID="ddlOperator" class="input" runat="server" required>
                                        <asp:ListItem Value="Select Operator" disabled Selected hidden>Select Operator</asp:ListItem>
                                        <asp:ListItem>Airtel</asp:ListItem>
                                        <asp:ListItem>Jio</asp:ListItem>
                                        <asp:ListItem>Vi</asp:ListItem>
                                    </asp:DropDownList>
                                    <label for="ddlOperator" class="label-name">
                                        <div class="icon"><i class="fas fa-list"></i></div>
                                        <span class="content-name">Operator</span>
                                    </label>
                                </div>
                                <div class="input-box">
                                    <asp:DropDownList ID="ddlState" class="input" runat="server" required>
                                        <asp:ListItem Value="Select State" disabled Selected hidden>Select State</asp:ListItem>
                                        <asp:ListItem>West Bengal</asp:ListItem>
                                        <asp:ListItem>Others</asp:ListItem>
                                    </asp:DropDownList>
                                    <label for="ddlState" class="label-name">
                                        <div class="icon"><i class="fas fa-map-marker-alt"></i></div>
                                        <span class="content-name">State</span>
                                    </label>
                                </div>
                                <div class="input-box">
                                    <asp:TextBox ID="txtRechargeAmount" class="input" type="text" autocomplete="off" runat="server" required
                                        onkeypress='validate(event)' MaxLength="3" ToolTip="Enter amount"
                                        AutoPostBack="true" OnTextChanged="txtRechargeAmount_TextChanged"></asp:TextBox>
                                    <label for="txtRechargeAmount" class="label-name">
                                        <div class="icon"><i class="fas fa-rupee-sign"></i></div>
                                        <span class="content-name">Amount</span>
                                    </label>
                                </div>
                                <div id="txnRechargeContainer" class="infoPanel amount" visible="false" runat="server">
                                    <asp:Label ID="lblRechrgeAmount" runat="server" Text="N/A"></asp:Label>
                                </div>
                            </asp:Panel>
                            <div class="btnContainer">
                                <asp:Button ID="btnRecharge" CssClass="btn" runat="server" Text="Place Request"
                                    OnClientClick="return confirm('Do you want to place a recharge request?')" OnClick="btnRecharge_Click" />
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <%
            string footerpath = HttpContext.Current.Server.MapPath("Html/Footer.htm");
            string footercontent = System.IO.File.ReadAllText(footerpath);
            Response.Write(footercontent);
        %>
    </form>
</body>
</html>
