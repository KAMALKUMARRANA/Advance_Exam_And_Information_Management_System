<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Users.aspx.cs" Inherits="Admin_Users" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Control Panel | Users</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="CSS/Users.css" rel="stylesheet" type="text/css" />
    <%
        string path = HttpContext.Current.Server.MapPath("Link.txt");
        string content = System.IO.File.ReadAllText(path);
        Response.Write(content);
    %>
</head>
<body>
    <form id="users" runat="server">
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
                    <div class="status top">
                        <i class="fas fa-user-shield"></i>
                        <asp:Label class="lbl" runat="server" Text="Admin"></asp:Label>
                        <span class="info">
                            <asp:Label ID="lblId" class="lbl" runat="server" Text="Id"></asp:Label> - <asp:Label ID="lblName" class="lbl" runat="server" Text="Name"></asp:Label>
                        </span>
                    </div>
                    <div class="status bottom">
                        <i class="fas fa-angle-double-right"></i>
                        <asp:Label runat="server" Text="Control Panel / Find Candidates"></asp:Label>
                    </div>
                    <div id="content">
                        <div class="intro">
                            <ul>
                                <li>Manage all candidates here.</li>
                            </ul>
                        </div>

                        <div class="row">
                            <div class="input-box ddl">
                                <asp:DropDownList ID="ddlCategory" CssClass="input ddl" runat="server" required
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                    <asp:ListItem Value="Select a option">Select a option</asp:ListItem>
                                    <asp:ListItem Value="UserId">User Id</asp:ListItem>
                                    <asp:ListItem Value="Name">Name</asp:ListItem>
                                    <asp:ListItem Value="Mobile">Mobile</asp:ListItem>
                                    <asp:ListItem Value="Email">Email</asp:ListItem>
                                    <asp:ListItem Value="RefId">Authentication Code</asp:ListItem>
                                </asp:DropDownList>
                                <label for="ddlRequestCategory" class="label-name">
                                    <div class="icon"><i class="fas fa-list-ul"></i></div>
                                    <span class="content-name">Search user by</span>
                                </label>
                            </div>

                            <div class="input-box txt">
                                <asp:TextBox ID="txtKey" CssClass="input" type="text" autocomplete="off"
                                    runat="server" required MaxLength="50"></asp:TextBox>
                                <label for="txtKey" class="label-name">
                                    <div class="icon"><i class="fas fa-key"></i></div>
                                    <span class="content-name" id="lblKey" runat="server">Enter keyword</span>
                                    <asp:Button ID="btnSearch" CssClass="btn" runat="server" Text="Search" OnClick="btnSearch_Click"/>
                                </label>
                            </div>
                        </div>
                        <div class="gvContainer">
                            <asp:GridView ID="gvUser" CssClass="gv" runat="server" AutoGenerateColumns="False" DataKeyNames="UserId" OnRowCancelingEdit="gvUser_RowCancelingEdit" OnRowEditing="gvUser_RowEditing" OnRowUpdating="gvUser_RowUpdating">
                                <Columns>
                                    <asp:TemplateField HeaderText="Authentication Code" ItemStyle-CssClass="gvitem" HeaderStyle-CssClass="gvheader">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("RefId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("RefId") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="gvheader"></HeaderStyle>
                                        <ItemStyle CssClass="gvitem"></ItemStyle>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="User Id" ItemStyle-CssClass="gvitem" HeaderStyle-CssClass="gvheader">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("UserId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("UserId") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="gvheader"></HeaderStyle>
                                        <ItemStyle CssClass="gvitem"></ItemStyle>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Password" ItemStyle-CssClass="gvitem" HeaderStyle-CssClass="gvheader">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("Password") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("Password") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="gvheader"></HeaderStyle>
                                        <ItemStyle CssClass="gvitem"></ItemStyle>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Status" ItemStyle-CssClass="gvitem" HeaderStyle-CssClass="gvheader">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="gvheader"></HeaderStyle>
                                        <ItemStyle CssClass="gvitem"></ItemStyle>
                                    </asp:TemplateField>

                                    

                                    <asp:TemplateField HeaderText="Name" ItemStyle-CssClass="gvitem" HeaderStyle-CssClass="gvheader">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditName" CssClass="gvTextbox" runat="server" Text='<%# Bind("Name") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="gvheader"></HeaderStyle>
                                        <ItemStyle CssClass="gvitem"></ItemStyle>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Mobile" ItemStyle-CssClass="gvitem" HeaderStyle-CssClass="gvheader">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("Mobile") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditMobile" CssClass="gvTextbox" runat="server" Text='<%# Bind("Mobile") %>'
                                                onkeypress='validate(event)' MaxLength="10"></asp:TextBox>
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="gvheader"></HeaderStyle>
                                        <ItemStyle CssClass="gvitem"></ItemStyle>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Email" ItemStyle-CssClass="gvitem" HeaderStyle-CssClass="gvheader">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditEmail" CssClass="gvTextbox" runat="server" Text='<%# Bind("Email") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="gvheader"></HeaderStyle>
                                        <ItemStyle CssClass="gvitem"></ItemStyle>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="gvaction" HeaderStyle-CssClass="gvheader">
                                        <ItemTemplate>
                                            <asp:LinkButton CssClass="gvbtn" runat="server" CommandName="Edit" OnClientClick="return confirm('Do you want to edit the selected row?')"
                                                ToolTip="Edit"><i class="fas fa-user-edit"></i>Edit</asp:LinkButton>
                                            <asp:LinkButton CssClass="gvbtn delete" runat="server" CommandName="Delete"
                                                OnClientClick="return confirm('Do you want to delete the selected user? It can not be undone!')"
                                                ToolTip="Delete the entire row"><i class="fas fa-trash-alt delete"></i>Delete</asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton CssClass="gvbtn" runat="server" CommandName="Update" OnClientClick="return confirm('Do you want to update?')"
                                                ToolTip="Update row"><i class="fas fa-save"></i>Update</asp:LinkButton>
                                            <asp:LinkButton CssClass="gvbtn" runat="server" CommandName="Cancel" OnClientClick="return confirm('All unsaved data will be lost!')"
                                                ToolTip="Cancel editing"><i class="fas fa-window-close"></i>Cancel</asp:LinkButton>
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="gvheader"></HeaderStyle>
                                        <ItemStyle CssClass="gvaction"></ItemStyle>
                                    </asp:TemplateField>

                                </Columns>
                                <EmptyDataTemplate>
                                    <div class="error">
                                        <asp:Label ID="lblError" runat="server" Text="No data found."></asp:Label>
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
