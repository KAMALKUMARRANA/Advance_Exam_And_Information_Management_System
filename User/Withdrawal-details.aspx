<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Withdrawal-details.aspx.cs" Inherits="User_Withdrawal_details" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Touch & View | Withdrawal History</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="CSS/Withdrawal-details.css" rel="stylesheet" type="text/css" />
    <%
        string path = HttpContext.Current.Server.MapPath("Link.txt");
        string content = System.IO.File.ReadAllText(path);
        Response.Write(content);
    %>
</head>
<body>
    <form id="redeemhistory" runat="server">
        <asp:ScriptManager ID="scriptPage" runat="server">
        </asp:ScriptManager>
        <asp:UpdateProgress ID="progressUpdate" AssociatedUpdatePanelID="pageUpdate" runat="server">
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
                        <asp:Label ID="status" runat="server" Text="Withdrawal Status"></asp:Label>
                        <asp:Label ID="lblInfo" runat="server" Text="name (id)"></asp:Label>
                    </div>
                    <div id="content">
                        <div class="intro">
                            <ul>
                                <li>Bank withdrawal history.</li>
                            </ul>
                        </div>
                        <div class="gvContainer">
                            <asp:GridView ID="gvBankHistory" CssClass="gv" runat="server" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:TemplateField HeaderText="Transaction No" ItemStyle-CssClass="gvitem" HeaderStyle-CssClass="gvheader">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("TxnNo") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="gvheader"></HeaderStyle>
                                        <ItemStyle CssClass="gvitem"></ItemStyle>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Transaction Date" ItemStyle-CssClass="gvitem" HeaderStyle-CssClass="gvheader">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("TxnDate") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="gvheader"></HeaderStyle>
                                        <ItemStyle CssClass="gvitem"></ItemStyle>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Status" ItemStyle-CssClass="gvitem" HeaderStyle-CssClass="gvheader">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="gvheader"></HeaderStyle>
                                        <ItemStyle CssClass="gvitem"></ItemStyle>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Process Date" ItemStyle-CssClass="gvitem" HeaderStyle-CssClass="gvheader">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("ProcessDate") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="gvheader"></HeaderStyle>
                                        <ItemStyle CssClass="gvitem"></ItemStyle>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Bank" ItemStyle-CssClass="gvitem" HeaderStyle-CssClass="gvheader">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("Bank") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="gvheader"></HeaderStyle>
                                        <ItemStyle CssClass="gvitem"></ItemStyle>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Account" ItemStyle-CssClass="gvitem" HeaderStyle-CssClass="gvheader">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("Account") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="gvheader"></HeaderStyle>
                                        <ItemStyle CssClass="gvitem"></ItemStyle>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="IFSC" ItemStyle-CssClass="gvitem" HeaderStyle-CssClass="gvheader">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("IFSC") %>'></asp:Label>
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
                                </Columns>
                                <EmptyDataTemplate>
                                    <div class="error">
                                        <asp:Label ID="lblError" runat="server" Text="No data found"></asp:Label>
                                    </div>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
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
