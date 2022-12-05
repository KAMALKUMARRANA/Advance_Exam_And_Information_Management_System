<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Helpdesk.aspx.cs" Inherits="Admin_Helpdesk" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Control Panel | Helpdesk</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="CSS/Helpdesk.css" rel="stylesheet" type="text/css" />
    <%
        string path = HttpContext.Current.Server.MapPath("Link.txt");
        string content = System.IO.File.ReadAllText(path);
        Response.Write(content);
    %>
</head>
<body>
    <form id="helpdesk" runat="server">
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
                    <div class="status top">
                        <i class="fas fa-user-shield"></i>
                        <asp:Label class="lbl" runat="server" Text="Admin"></asp:Label>
                        <span class="info">
                            <asp:Label ID="lblId" class="lbl" runat="server" Text="Id"></asp:Label> - <asp:Label ID="lblName" class="lbl" runat="server" Text="Name"></asp:Label>
                        </span>
                    </div>
                    <div class="status bottom">
                        <i class="fas fa-angle-double-right"></i>
                        <asp:Label runat="server" Text="Control Panel / Helpdesk"></asp:Label>
                    </div>
                    <div id="content">
                        <div class="intro">
                            <ul>
                                <li>Manage all complains here.</li>
                            </ul>
                        </div>
                        <asp:Panel ID="panelView" runat="server">
                            <div class="row">
                                <div class="input-box ddl">
                                    <asp:DropDownList ID="ddlCategory" CssClass="input ddl" runat="server" required
                                        AutoPostBack="True" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                        <asp:ListItem Value="Select a option">Select a option</asp:ListItem>
                                        <asp:ListItem Value="RefNo">Reference No</asp:ListItem>
                                        <asp:ListItem Value="UserId">User Id</asp:ListItem>
                                    </asp:DropDownList>
                                    <label for="ddlCategory" class="label-name">
                                        <div class="icon"><i class="fas fa-list-ul"></i></div>
                                        <span class="content-name">Search new request by</span>
                                    </label>
                                </div>

                                <div class="input-box txt" id="txtbox" runat="server">
                                    <asp:TextBox ID="txtKey" CssClass="input" type="text" autocomplete="off" runat="server" required
                                        MaxLength="50"></asp:TextBox>
                                    <label for="txtKey" class="label-name">
                                        <div class="icon"><i class="fas fa-key"></i></div>
                                        <span class="content-name" id="lblRequestKey" runat="server">Enter keyword</span>
                                        <asp:Button ID="btnRequestSearch" CssClass="btn" runat="server" Text="Search" OnClick="btnRequestSearch_Click"/>
                                    </label>
                                </div>
                            </div>

                            <div class="gvContainer">
                                <asp:GridView ID="gvRequest" CssClass="gv" runat="server" AutoGenerateColumns="False"
                                    DataKeyNames="RefNo" OnSelectedIndexChanged="gvRequest_SelectedIndexChanged">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Reference No" ItemStyle-CssClass="gvitem" HeaderStyle-CssClass="gvheader">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("RefNo") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="gvheader"></HeaderStyle>
                                            <ItemStyle CssClass="gvitem"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Raise Date" ItemStyle-CssClass="gvitem" HeaderStyle-CssClass="gvheader">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("RaiseDate") %>'></asp:Label>
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

                                        <asp:TemplateField HeaderText="User Id" ItemStyle-CssClass="gvitem" HeaderStyle-CssClass="gvheader">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("UserId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="gvheader"></HeaderStyle>
                                            <ItemStyle CssClass="gvitem"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Subject" ItemStyle-CssClass="gvitem" HeaderStyle-CssClass="gvheader">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("Subject") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="gvheader"></HeaderStyle>
                                            <ItemStyle CssClass="gvitem"></ItemStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="gvitem" HeaderStyle-CssClass="gvheader">
                                            <ItemTemplate>
                                                <asp:LinkButton CssClass="gvbtn" runat="server" CommandName="Select" OnClientClick="return confirm('Do you want to view details?')"
                                                    ToolTip="View the complain detals."><i class="far fa-eye"></i>View</asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="gvheader"></HeaderStyle>
                                            <ItemStyle CssClass="gvitem"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div class="error">
                                            <asp:Label ID="lblError" runat="server" Text="No data found."></asp:Label>
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
