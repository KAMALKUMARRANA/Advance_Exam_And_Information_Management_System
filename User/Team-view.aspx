<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Team-view.aspx.cs" Inherits="User_Team_view" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Touch & View | Team View</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="CSS/Team-view.css" rel="stylesheet" type="text/css" />
    <%
        string path = HttpContext.Current.Server.MapPath("Link.txt");
        string content = System.IO.File.ReadAllText(path);
        Response.Write(content);
    %>
</head>
<body>
    <form id="teamview" runat="server">
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
                        <i class="fas fa-user-shield"></i>
                        <asp:Label ID="status" runat="server" Text="My Team"></asp:Label>
                        <asp:Label ID="lblInfo" runat="server" Text="name (id)"></asp:Label>
                    </div>
                    <div id="content">
                        <div class="intro">
                            <ul>
                                <li>Direct Team members.</li>
                            </ul>
                        </div>
                        <div class="row">
                            <div class="list" id="listUser" runat="server">
                                <i id="icon" class="fas fa-user" runat="server"></i>
                                <label id="tagStatus" class="tag" runat="server">Inactive</label>
                                <asp:Label ID="lblId" CssClass="id" runat="server" Text="N/A"></asp:Label>
                                <asp:Label ID="lblName" class="name" runat="server" Text="N/A"></asp:Label>
                                <asp:Label ID="lblBoost" class="boost" runat="server" Text="Not Boosted"></asp:Label>
                            </div>
                        </div>

                        <div class="gvContainer">
                            <asp:GridView ID="gvTeam" CssClass="gv" runat="server" AutoGenerateColumns="False"
                                DataKeyNames="UserId" OnRowUpdating="gvTeam_RowUpdating" OnSelectedIndexChanged="gvTeam_SelectedIndexChanged">
                                <Columns>
                                    <asp:TemplateField HeaderText="User Id" ItemStyle-CssClass="gvitem" HeaderStyle-CssClass="gvheader">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("UserId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="gvheader"></HeaderStyle>
                                        <ItemStyle CssClass="gvitem"></ItemStyle>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Name" ItemStyle-CssClass="gvitem" HeaderStyle-CssClass="gvheader">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("Name") %>'></asp:Label>
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

                                    <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="gvitem" HeaderStyle-CssClass="gvheader">
                                        <ItemTemplate>
                                            <asp:LinkButton CssClass="gvbtn" runat="server" CommandName="Update"
                                                ToolTip="View this members team"><i class="fas fa-eye"></i>View</asp:LinkButton>
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
