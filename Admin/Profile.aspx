<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Profile.aspx.cs" Inherits="Admin_Profile" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Control Panel | Profile</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="CSS/Profile.css" rel="stylesheet" type="text/css" />
    <%
        string path = HttpContext.Current.Server.MapPath("Link.txt");
        string content = System.IO.File.ReadAllText(path);
        Response.Write(content);
    %>
</head>
<body>
    <form id="profie" runat="server">
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
                <asp:Panel ID="passwordUpdatePanel" class="overlay" runat="server" Visible="false">
                    <div class="content">
                        <div class="header">Update your password<asp:LinkButton ID="linkClose" runat="server" OnClick="linkClose_Click"><i class="fas fa-times"></i></asp:LinkButton></div>
                        <div class="input-box">
                            <asp:TextBox ID="txtOldPassword" class="input" type="text" autocomplete="off" runat="server" required MaxLength="20"></asp:TextBox>
                            <label for="txtOldPassword" class="label-name">
                                <span class="content-name">Current password</span>
                            </label>
                        </div>
                        <div class="input-box">
                            <asp:TextBox ID="txtPassword" class="input" type="text" autocomplete="off" runat="server" required MaxLength="20"></asp:TextBox>
                            <label for="txtPassword" class="label-name">
                                <span class="content-name">New password</span>
                            </label>
                        </div>
                        <div class="input-box">
                            <asp:TextBox ID="txtCPassword" class="input" type="text" autocomplete="off" runat="server" required MaxLength="20"></asp:TextBox>
                            <label for="txtCPassword" class="label-name">
                                <span class="content-name">Confirm password</span>
                            </label>
                        </div>
                        <div class="btnContainer">
                            <asp:Button ID="btnUpdate" CssClass="btn" runat="server" Text="Update" OnClick="btnUpdate_Click"/>
                        </div>
                    </div>
                </asp:Panel>

                <div id="body">
                    <div class="status top">
                        <i class="fas fa-user-shield"></i>
                        <asp:Label class="lbl" runat="server" Text="Admin"></asp:Label>
                        <span class="info">
                            <asp:Label ID="lblAdminId" class="lbl" runat="server" Text="Id"></asp:Label> - <asp:Label ID="lblAdminName" class="lbl" runat="server" Text="Name"></asp:Label>
                        </span>
                    </div>
                    <div class="status bottom">
                        <i class="fas fa-angle-double-right"></i>
                        <asp:Label runat="server" Text="Control Panel / Admin Profile"></asp:Label>
                    </div>

                    <div id="content">
                        <div class="window">
                            <label for="info">
                                <i class="icon fas fa-info-circle"></i> Account Information <i class="open fas fa-chevron-down"></i>
                            </label>
                            <div class="infoPanel">
                                <div class="list">
                                    <i class="fas fa-key"></i>
                                    <p>Admin Id</p>
                                    <asp:Label ID="lblId" class="txt" runat="server" Text="N/A"></asp:Label>
                                    <asp:LinkButton ID="btnEditPassword" class="link" runat="server" OnClick="btnEditPassword_Click">Change password</asp:LinkButton>
                                </div>
                                <div class="row">
                                    <div>Name</div>
                                    <div>:</div>
                                    <span id="lblName" runat="server">N/A</span>
                                </div>
                                <div class="row">
                                    <div>Mobile</div>
                                    <div>:</div>
                                    <span id="lblMobile" runat="server">N/A</span>
                                </div>
                                <div class="row">
                                    <div>Email</div>
                                    <div>:</div>
                                    <span id="lblEmail" runat="server">N/A</span>
                                </div>
                                <div class="row">
                                    <div>Type</div>
                                    <div>:</div>
                                    <span id="lblType" runat="server">N/A</span>
                                </div>
                                <div class="row">
                                    <asp:LinkButton ID="linkEdit" class="link" runat="server" OnClick="linkEdit_Click"><i class="fas fa-edit"></i> Edit</asp:LinkButton>
                                </div>
                            </div>
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
