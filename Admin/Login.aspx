<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Admin_Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin Login</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="CSS/Login.css" rel="stylesheet" type="text/css" />
    <link href="CSS/Loader.css" rel="stylesheet" type="text/css" />
    <link href="../Global/Fontawesome-free-6.0.0-beta2-web/css/all.css" rel="stylesheet" type="text/css" />
    <script src="JS/Login.js" type="text/javascript"></script>
</head>
<body>
    <form id="login" runat="server">
    <asp:ScriptManager ID="pageScript" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="progressUpdate" AssociatedUpdatePanelID="pageUpdate" runat="server"><ProgressTemplate>
        <div id="loadingPanel">
            <div id="loader">
                <svg class="circular" viewbox="25 25 50 50">
                    <circle class="path" cx="50" cy="50" r="20" fill="none" stroke-width="3"></circle>
                </svg>
            </div>
        </div>
    </ProgressTemplate></asp:UpdateProgress>

    <asp:UpdatePanel ID="pageUpdate" runat="server"><ContentTemplate>
        <div id="logo" runat="server">Online Exam Management System</div>
        <div class="login-panel">
            <h1><i class="fas fa-user-shield"></i> ADMIN LOGIN</h1>

            <div class="input-box">
                <asp:TextBox ID="txtUsername" class="input" type="text" autocomplete="off" runat="server" required MaxLength="10"></asp:TextBox>
                <label for="txtUsername" class="label-name">
                    <div class="icon"><i class="far fa-envelope"></i></div>
                    <span class="content-name">Username</span>
                </label>
            </div>

            <div class="input-box">
                <asp:TextBox ID="txtPassword" class="input password" type="password" autocomplete="off" runat="server" required MaxLength="20"></asp:TextBox>
                <label for="txtPassword" class="label-name">
                    <div class="icon"><i class="fas fa-key"></i></div>
                    <span class="content-name">Password</span>
                    <span class="eye" onclick="showhide()">
                        <i id="show" class="far fa-eye"></i>
                        <i id="hide" class="far fa-eye-slash"></i>
                    </span>
                </label>
            </div>
            <asp:Button ID="btnLogin" CssClass="login-btn" runat="server" Text="LOGIN" OnClientClick="check()" OnClick="btnLogin_Click"/>
            <div class="row">
                <asp:LinkButton ID="linkForgotPassword" CssClass="link" runat="server">Password Recovery</asp:LinkButton>
            </div>
        </div>
    </ContentTemplate></asp:UpdatePanel>
    </form>
</body>
</html>
