<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Forgot-password.aspx.cs" Inherits="User_Forgot_password" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Forgot Password</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="CSS/Forgot-password.css" rel="stylesheet" type="text/css" />
    <link href="CSS/Loader.css" rel="stylesheet" type="text/css" />
    <link href="../Global/Fontawesome-free-5.15.4-web/css/all.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="forgotpassword" runat="server">
        <asp:ScriptManager ID="scriptRegistration" runat="server">
        </asp:ScriptManager>
        <asp:UpdateProgress ID="progressUpdate" AssociatedUpdatePanelID="updatePage" runat="server">
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

        <asp:UpdatePanel ID="updatePage" runat="server">
            <ContentTemplate>
                <div id="header">
                    <div id="logo"><span>Touch &amp; View</span></div>
                </div>
                <div id="body">
                    <div class="status">
                        <i class="fas fa-angle-double-right"></i>
                        <asp:Label ID="status" runat="server" Text="Password Recovery"></asp:Label>
                    </div>

                    <div id="content">
                        <asp:Panel ID="formSearch" class="form" runat="server">
                            <div class="input-box search">
                                <asp:TextBox ID="txtUserId" CssClass="input" type="text" autocomplete="off" runat="server" required MaxLength="10"></asp:TextBox>
                                <label id="searchBox" for="txtUserId" class="label-name" runat="server">
                                    <span class="content-name">Enter User Id</span>
                                    <span class="icon">
                                        <asp:Label ID="error" class="fas fa-exclamation-circle" runat="server" Visible="false"></asp:Label>
                                    </span>
                                </label>
                                <small id="lblError" runat="server"></small>
                            </div>
                            <div class="btnContainer">
                                <asp:Button ID="btnSearch" CssClass="btn" runat="server" Text="Next" OnClick="btnSearch_Click"/>
                            </div>
                        </asp:Panel>

                        <asp:Panel ID="formSendOTP" class="form" runat="server" Visible="false">
                            <div class="lblContainer">
                                <asp:Label ID="lblMobile" class="lbl" runat="server" Text="An One Time Password will be sent to your registered mobile number: xxxxxxxxxx."></asp:Label>
                            </div>
                            <asp:Label ID="lblSendMessage" class="lblMessage" runat="server" Text=""></asp:Label>
                            <div class="btnContainer">
                                <asp:Button ID="btnSendOTP" CssClass="btn" runat="server" Text="Send OTP" OnClick="btnSendOTP_Click"/>
                            </div>
                        </asp:Panel>

                        <asp:Panel ID="formSetPassword" class="form" runat="server" Visible="false">
                            <div class="lblContainer">
                                <asp:Label ID="lblInfo" class="lbl" runat="server" Text="Password recovery for account:"></asp:Label>
                            </div>
                            <div class="input-box">
                                <asp:TextBox ID="txtOtp" CssClass="input" type="text" autocomplete="off" runat="server" required MaxLength="6"></asp:TextBox>
                                <label for="txtOtp" class="label-name">
                                    <span class="content-name">OTP</span>
                                    <span class="icon">
                                        <asp:Label ID="OtpError" class="fas fa-exclamation-circle" runat="server" Visible="false"></asp:Label>
                                    </span>
                                </label>
                            </div>
                            <div class="input-box">
                                <asp:TextBox ID="txtPassword" CssClass="input" type="password" autocomplete="off" runat="server" required MaxLength="20"></asp:TextBox>
                                <label for="txtPassword" class="label-name">
                                    <span class="content-name">Password</span>
                                </label>
                            </div>
                            <div class="input-box">
                                <asp:TextBox ID="txtCPassword" CssClass="input" type="text" autocomplete="off" runat="server" required MaxLength="20"></asp:TextBox>
                                <label for="txtCPassword" class="label-name">
                                    <span class="content-name">Confirm Password</span>
                                </label>
                            </div>
                            <small id="lblMessage" class="lblMessage" runat="server"></small>
                            <div class="btnContainer">
                                <asp:Button ID="btnReset" CssClass="btn" runat="server" Text="Reset Password" OnClick="btnReset_Click"/>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
