<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Candidates.aspx.cs" Inherits="Admin_Leaders" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin Panel | Candidates</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    
    <link href="CSS/Candidates.css" rel="stylesheet" type="text/css" />
    <%
        string path = HttpContext.Current.Server.MapPath("Link.txt");
        string content = System.IO.File.ReadAllText(path);
        Response.Write(content);
    %>
    <script src="JS/Candidates.js" type="text/javascript"></script>
</head>
<body>
    <form id="leaders" runat="server">
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
                        <asp:Label runat="server" Text="Control Panel / Candidates"></asp:Label>
                    </div>
                    <div id="content">
                        <asp:Panel ID="panelForm" runat="server" Visible="false">
                            <div class="intro">
                                <ul>
                                    <li>Add new candidate here.</li>
                                </ul>
                            </div>
                            <div class="input-box">
                                <asp:TextBox ID="txtName" class="input" type="text" autocomplete="off" runat="server" required MaxLength="30"></asp:TextBox>
                                <label for="txtName" class="label-name">
                                    <span class="content-name">Name</span>
                                </label>
                            </div>

                            <div class="input-box">
                                <asp:TextBox ID="txtMobile" class="input" type="text" autocomplete="off" runat="server" required
                                    onkeypress='validate(event)' MaxLength="10"></asp:TextBox>
                                <label for="txtMobile" class="label-name">
                                    <span class="content-name">Mobile number</span>
                                </label>
                            </div>

                            <div class="input-box">
                                <asp:TextBox ID="txtEmail" class="input" type="text" autocomplete="off" runat="server" required MaxLength="50"></asp:TextBox>
                                <label for="txtEmail" class="label-name">
                                    <span class="content-name">Email</span>
                                </label>
                            </div>

                            <div class="input-box">
                                <asp:DropDownList ID="ddlGender" class="input" runat="server" required>
                                    <asp:ListItem Value="Gender" disabled Selected hidden>Gender</asp:ListItem>
                                    <asp:ListItem Value="Male">Male</asp:ListItem>
                                    <asp:ListItem Value="Female">Female</asp:ListItem>
                                </asp:DropDownList>
                                <label for="ddlGender" class="label-name">
                                    <span class="content-name">Gender</span>
                                </label>
                            </div>

                            <div class="input-box">
                                <asp:TextBox ID="txtDOB" class="input" TextMode="Date" autocomplete="off" runat="server"
                                    MaxLength="10"></asp:TextBox>
                                <label for="txtDOB" class="label-name">
                                    <span class="content-name">Date of Birth</span>
                                </label>
                            </div>

                            <div class="input-box">
                                <asp:TextBox ID="txtPassword" class="input" type="text" autocomplete="off" runat="server" required MaxLength="20"></asp:TextBox>
                                <label for="txtPassword" class="label-name">
                                    <span class="content-name">Password</span>
                                </label>
                            </div>

                            <div class="btnContainer">
                                <asp:LinkButton ID="btnRegister" CssClass="btn" runat="server" OnClientClick="return confirm('Do you want to register new leader?')" OnClick="btnRegister_Click"
                                    ><i class="fas fa-user-plus"></i>Register</asp:LinkButton>
                                <asp:LinkButton ID="linkCancel" CssClass="btn" runat="server" OnClientClick="return confirm('All unsaved data will be lost.')" OnClick="linkCancel_Click"
                                    ><i class="fas fa-times-circle"></i>Cancel</asp:LinkButton>
                            </div>
                        </asp:Panel>

                        <asp:Panel ID="panelView" runat="server">
                            <div class="intro">
                                <ul>
                                    <li>Manage all candidates here.</li>
                                    <li>Add new Candidate <asp:LinkButton ID="linkAdd" runat="server" OnClick="linkAdd_Click">here</asp:LinkButton></li>
                                </ul>
                            </div>
                            <div class="gvContainer">
                                <asp:GridView ID="gvLeader" CssClass="gv" runat="server"
                                    AutoGenerateColumns="False" DataKeyNames="UserId" OnRowCancelingEdit="gvLeader_RowCancelingEdit" OnRowEditing="gvLeader_RowEditing" OnRowUpdating="gvLeader_RowUpdating">
                                    <Columns>
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
