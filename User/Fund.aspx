<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Fund.aspx.cs" Inherits="User_Fund" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Touch & View | Manage Fund</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="CSS/Fund.css" rel="stylesheet" type="text/css" />
    <%
        string path = HttpContext.Current.Server.MapPath("Link.txt");
        string content = System.IO.File.ReadAllText(path);
        Response.Write(content);
    %>
    <script src="JS/Fund.js" type="text/javascript"></script>
</head>
<body>
    <form id="fund" runat="server">
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
                        <asp:Label ID="status" runat="server" Text="Fund Management"></asp:Label>
                        <asp:Label ID="lblInfo" runat="server" Text="name (id)"></asp:Label>
                    </div>
                    <div class="balance">
                        <asp:Label ID="lblBalance" runat="server" Text="Available Balance"></asp:Label>
                    </div>
                    <div class="balance">
                        <asp:Label ID="lblFund" runat="server" Text="Available Fund"></asp:Label>
                    </div>

                    <div id="content">
                        <div id="btnContainer">
                            <asp:LinkButton ID="buttonFund" CssClass="button active" runat="server" OnClick="buttonFund_Click">
                                <i class="fas fa-tasks"></i>Manage Fund
                            </asp:LinkButton>
                            <asp:LinkButton ID="buttonHistory" CssClass="button" runat="server" OnClick="buttonHistory_Click">
                                <i class="fas fa-history"></i>History
                            </asp:LinkButton>
                        </div>

                        <asp:Panel CssClass="tabPanel" ID="panelFund" runat="server" Visible="true">
                            <div class="intro">
                                <h><i class="fas fa-arrow-alt-circle-up"></i> Fund transfer</h>
                                <ul>
                                    <li>Minimum fund transfer amount is 100</li>
                                </ul>
                            </div>

                            <div class="input-box inactive">
                                <asp:TextBox ID="lblFund1" CssClass="input" type="text" Enabled="false" runat="server" ToolTip="Available fund.">00</asp:TextBox>
                                <label for="lblFund1" class="label-name">
                                    <div class="icon"><i class="fas fa-coins"></i></div>
                                    <span class="content-name">Available Fund</span>
                                </label>
                            </div>

                            <div class="input-box inactive" runat="server">
                                <asp:TextBox ID="lblUserId" CssClass="input" type="text" Enabled="false" runat="server" ToolTip="Sender user id.">00</asp:TextBox>
                                <label for="lblUserId" class="label-name">
                                    <div class="icon"><i class="fas fa-user-circle"></i></div>
                                    <span id="lblSender" class="content-name" runat="server">Sender User Id</span>
                                </label>
                            </div>

                            <div class="input-box" runat="server">
                                <asp:TextBox ID="txtReceiverId" CssClass="input" type="text" autocomplete="off" runat="server" placeholder="Receiver User Id"
                                    MaxLength="10" ToolTip="Enter receiver user id."></asp:TextBox>
                                <label for="txtReceiverId" class="label-name">
                                    <div class="icon"><i class="fas fa-user"></i></div>
                                    <span id="lblReceiver" class="content-name" runat="server">Receiver: Name</span>
                                    <span class="msgicon">
                                        <asp:Label ID="error" class="fas fa-exclamation-circle" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="success" class="fas fa-check-circle" runat="server" Visible="false"></asp:Label>
                                    </span>
                                    <asp:Button ID="btnSearch" CssClass="btn" runat="server" Text="Verify" OnClick="btnSearch_Click" />
                                </label>
                                <small id="lblMessage" runat="server"></small>
                            </div>

                            <div class="input-box" runat="server">
                                <asp:TextBox ID="txtFund" CssClass="input" type="text" autocomplete="off" runat="server" placeholder="Fund Amount" Enabled="false"
                                    onkeypress='validate(event)' MaxLength="4" ToolTip="Enter fund amount."></asp:TextBox>
                                <label for="txtFund" class="label-name">
                                    <div class="icon"><i class="fas fa-coins"></i></div>
                                    <span class="content-name">Fund Amount</span>
                                </label>
                            </div>

                            <div class="btnContainer">
                                <asp:LinkButton ID="btnSend" CssClass="btn" runat="server" ToolTip="Transfer fund." Enabled="false"
                                    OnClientClick="return confirm('Do you want to Peroceed?')" OnClick="btnSend_Click">
                                    <i class="fas fa-paper-plane"></i>Transfer</asp:LinkButton>
                            </div>

                            <div class="intro second">
                                <h>Balance <i class="fa fa-arrow-alt-circle-right"></i> Fund</h>
                                <ul>
                                    <li>Minimum convert amount is 100</li>
                                    <li><asp:Label ID="lblTax" runat="server" Text="TDS"></asp:Label> % TDS will be charged.</li>
                                </ul>
                            </div>

                            <div class="input-box inactive">
                                <asp:TextBox ID="lblAccountBalance" CssClass="input" type="text" Enabled="false" runat="server" ToolTip="Available account balance.">00</asp:TextBox>
                                <label for="lblAccountBalance" class="label-name">
                                    <div class="icon"><i class="fas fa-rupee-sign"></i></div>
                                    <span class="content-name">Available Balance</span>
                                </label>
                            </div>

                            <div class="input-box" runat="server">
                                <asp:TextBox ID="txtConvertAmount" CssClass="input" type="text" autocomplete="off" runat="server" placeholder="Amount" AutoPostBack="true"
                                    onkeypress='validate(event)' MaxLength="4" ToolTip="Enter amount" OnTextChanged="txtConvertAmount_TextChanged"></asp:TextBox>
                                <label for="txtConvertAmount" class="label-name">
                                    <div class="icon"><i class="fas fa-rupee-sign"></i></div>
                                    <span class="content-name">Amount</span>
                                </label>
                            </div>

                            <div id="amountPanel" class="infoPanel amount" visible="false" runat="server">
                                <asp:Label ID="lblConvertAmount" runat="server" Text="N/A"></asp:Label>
                            </div>

                            <div class="btnContainer">
                                <asp:LinkButton ID="btnConvert" CssClass="btn" runat="server" ToolTip="Convert balance to fund."
                                    OnClientClick="return confirm('Do you want to Peroceed?')" OnClick="btnConvert_Click"><i class="fas fa-arrow-circle-down"></i>Convert</asp:LinkButton>
                            </div>
                        </asp:Panel>

                        <asp:Panel CssClass="tabPanel" ID="panelHistory" Visible="false" runat="server">
                            <div class="intro">
                                <ul><li>Fund history.</li></ul>
                            </div>

                            <div class="gvContainer">
                                <asp:GridView ID="gvHistory" CssClass="gv" runat="server" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Date & Time" ItemStyle-CssClass="gvitem" HeaderStyle-CssClass="gvheader">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("TxnDate") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="gvheader"></HeaderStyle>
                                            <ItemStyle CssClass="gvitem"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Type" ItemStyle-CssClass="gvitem" HeaderStyle-CssClass="gvheader">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("Type") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="gvheader"></HeaderStyle>
                                            <ItemStyle CssClass="gvitem"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Amount" ItemStyle-CssClass="gvitem" HeaderStyle-CssClass="gvheader">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("Amount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="gvheader"></HeaderStyle>
                                            <ItemStyle CssClass="gvitem"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Description" ItemStyle-CssClass="gvitem" HeaderStyle-CssClass="gvheader">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="gvheader"></HeaderStyle>
                                            <ItemStyle CssClass="gvitem"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div class="error">
                                            <asp:Label ID="lblError" runat="server" Text="No data found"></asp:Label>
                                        </div>
                                    </EmptyDataTemplate>
                                </asp:GridView>
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
