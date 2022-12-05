<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Registration.aspx.cs" Inherits="User_Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>New User Registration</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="CSS/Registration.css" rel="stylesheet" type="text/css" />
    <link href="CSS/Loader.css" rel="stylesheet" type="text/css" />
    <link href="../Global/Fontawesome-free-6.0.0-beta2-web/css/all.css" rel="stylesheet" type="text/css" />
    <script src="JS/Registration.js" type="text/javascript"></script>
</head>
<body>
    <form id="registration" runat="server">
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
                    <div id="logo"><span>TOUCH &amp; VIEW</span></div>
                </div>
                <div id="body">
                    <div class="status">
                        <i class="fas fa-angle-double-right"></i>
                        <asp:Label ID="status" runat="server" Text="New Registration"></asp:Label>
                    </div>
                    <div id="content">
                        <div id="title">Authentication</div>

                        <div class="input-box">
                            <asp:TextBox ID="txtReferCode" CssClass="input" type="text" autocomplete="off" runat="server" required
                                MaxLength="10" AutoPostBack="True" OnTextChanged="txtReferCode_TextChanged"></asp:TextBox>
                            <label id="searchBox" for="txtReferCode" class="label-name" runat="server">
                                <span class="content-name">Enter the access code</span>
                                <span class="icon">
                                    <asp:Label ID="error" class="fas fa-exclamation-circle" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="success" class="fas fa-check-circle" runat="server" Visible="false"></asp:Label>
                                </span>
                                <asp:LinkButton ID="btnSearch" CssClass="btn" runat="server" OnClick="btnSearch_Click"><i class="fas fa-search"></i></asp:LinkButton>
                            </label>
                            <small id="lblError" runat="server"></small>
                        </div>

                        <asp:Panel ID="formRegistration" CssClass="form" runat="server" Visible="TRUE">
                            <div class="tag">
                                <p>Referrer details</p>
                            </div>
                            <div class="data">
                                <asp:Label runat="server" Text="Refer Code : "></asp:Label>
                                <asp:Label ID="lblId" runat="server" Text="N/A"></asp:Label>
                            </div>
                            <div class="data">
                                <asp:Label runat="server" Text="Name : "></asp:Label>
                                <asp:Label ID="lblName" runat="server" Text="N/A"></asp:Label>
                            </div>

                            <div class="row">
                                <div class="input-box half">
                                    <asp:TextBox ID="txtFirstName" class="input" type="text" autocomplete="off" runat="server" required MaxLength="15"></asp:TextBox>
                                    <label for="txtFirstName" class="label-name">
                                        <span class="content-name">First name</span>
                                    </label>
                                </div>
                                <div class="input-box half">
                                    <asp:TextBox ID="txtLastName" class="input" type="text" autocomplete="off" runat="server" required MaxLength="15"></asp:TextBox>
                                    <label for="txtLastName" class="label-name">
                                        <span class="content-name">Last name</span>
                                    </label>
                                </div>
                            </div>

                            <div class="input-box">
                                <asp:DropDownList ID="ddlGender" class="input" runat="server" required>
                                    <asp:ListItem Value="Select Gender" disabled Selected hidden>Select Gender</asp:ListItem>
                                    <asp:ListItem Value="Male">Male</asp:ListItem>
                                    <asp:ListItem Value="Female">Female</asp:ListItem>
                                    <asp:ListItem Value="Others">Others</asp:ListItem>
                                </asp:DropDownList>
                                <label for="ddlGender" class="label-name">
                                    <span class="content-name">Gender</span>
                                </label>
                            </div>

                            <div class="input-box">
                                <asp:TextBox ID="txtDob" class="input" TextMode="Date" autocomplete="off" runat="server" MaxLength="10"></asp:TextBox>
                                <label for="txtDob" class="label-name">
                                    <span class="content-name">Date of Birth</span>
                                </label>
                            </div>

                            <div class="input-box">
                                <asp:TextBox ID="txtMobile" class="input" type="text" autocomplete="off" runat="server" required onkeypress='validate(event)' MaxLength="10"></asp:TextBox>
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

                            <div class="msg">
                                <p>Your username and password will be sent to your mobile or email after successfull registration.</p>
                            </div>

                            <div class="check">
                                <label for="Check18">
                                    <asp:CheckBox ID="Check18" runat="server" />
                                    I am over 18 years old.
                                </label>
                            </div>

                            <div class="check">
                                <label for="checkPC">
                                    <asp:CheckBox ID="checkPC" runat="server" />
                                    I have read and accept all the <a>Privacy Policy</a>
                                </label>
                            </div>

                            <div class="check">
                                <label for="CheckTC">
                                    <asp:CheckBox ID="CheckTC" runat="server" />
                                    I have read and accept all the <a>Terms and Conditions</a>
                                </label>
                            </div>

                            <div class="btnContainer">
                                <asp:LinkButton ID="btnRegister" CssClass="btn" runat="server" OnClientClick="return confirm('Are you sure you want to register?')"
                                    OnClick="btnRegister_Click">Register</asp:LinkButton>
                            </div>
                        </asp:Panel>

                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
    <script src="JS/Registration.js" type="text/javascript"></script>
</body>
</html>
