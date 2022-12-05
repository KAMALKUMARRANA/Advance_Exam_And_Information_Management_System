<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Account.aspx.cs" Inherits="User_Account" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Candidate Panel | Account Details</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="CSS/Account.css" rel="stylesheet" type="text/css" />
    <%
        string path = HttpContext.Current.Server.MapPath("Link.txt");
        string content = System.IO.File.ReadAllText(path);
        Response.Write(content);
    %>
</head>
<body>
    <form id="account" runat="server">
        <asp:ScriptManager ID="scriptPage" runat="server">
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
        <%
            string headerpath = HttpContext.Current.Server.MapPath("Html/Header.htm");
            string headercontent = System.IO.File.ReadAllText(headerpath);
            Response.Write(headercontent);
        %>

        <asp:UpdatePanel ID="updatePage" runat="server">
            <ContentTemplate>
                <asp:Panel ID="verifyPanel" class="overlay" runat="server" Visible="false">
                    <div class="content">
                        <div class="header">OTP Verification</div>
                        <div class="lblContainer">
                            <asp:Label class="lbl" runat="server" Text="Enter OTP sent to your "></asp:Label>
                            <asp:Label class="lbl" ID="lblMessage" runat="server" Text="Email"></asp:Label>
                        </div>
                        <div class="input-box">
                            <asp:TextBox ID="txtOtp" class="input" type="text" MaxLength="6" autocomplete="off" runat="server" required></asp:TextBox>
                            <label for="txtOtp" class="label-name">
                                <span class="content-name">OTP</span>
                            </label>
                        </div>
                        <div class="linkContainer">
                            <asp:LinkButton ID="linkRsend" runat="server" Visible="false" OnClick="linkRsend_Click">Resend</asp:LinkButton>
                            <asp:Label ID="lblTime" runat="server" Text="Re-send OTP in 60 secs."></asp:Label>
                        </div>
                        <div class="btnContainer">
                            <asp:Button ID="btnVerify" CssClass="btn" runat="server" Text="Verify" OnClick="btnVerify_Click"/>
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="passwordUpdatePanel" class="overlay" runat="server" Visible="false">
                    <div class="content">
                        <div class="header">
                            Update your password
                            <asp:LinkButton ID="linkClose" runat="server" OnClick="linkClose_Click"><i class="fas fa-times"></i></asp:LinkButton>
                        </div>
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
                    <div class="status">
                        <i class="fas fa-angle-double-right"></i>
                        <asp:Label ID="status" runat="server" Text="My Account"></asp:Label>
                        <asp:Label ID="lblInfo" runat="server" Text="name (id)"></asp:Label>
                    </div>
                    <div id="content">
                        <div class="window">
                            <input type="checkbox" id="overview" class="checkbox" runat="server" />
                            <label for="overview" id="btnOverview">
                                <i class="icon fas fa-eye"></i>Account Status
                                <i class="close fas fa-chevron-right"></i>
                                <i class="open fas fa-chevron-down"></i>
                            </label>
                            <div id="overviewPanel">
                                <div class="row">
                                    <div class="list" id="listStatus" runat="server">
                                        <i class="fas fa-trophy"></i>
                                        <label id="tagStatus" class="tag" runat="server">Inactive</label>
                                        <p>Authentication Status</p>
                                        
                                        <asp:Label ID="lblBoost" class="link" runat="server" Text="Current Student"></asp:Label>
                                    </div>

                                    <div class="list refer" id="listReferrer" runat="server">
                                        <i class="fas fa-arrow-circle-up"></i>
                                        <label id="tagReferrerId" class="tag" runat="server">N/A</label>
                                        <p>Activation Details</p>
                                        <asp:Label ID="lblReferrerName" class="txt" runat="server" Text="N/A"></asp:Label>
                                        <asp:LinkButton ID="linkContact" class="link" runat="server" OnClick="linkContact_Click">Contact</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="window">
                            <input type="checkbox" id="details" class="checkbox" runat="server" />
                            <label for="details" id="btnDetail">
                                <i class="icon fas fa-user-circle"></i>Account Details
                                <i class="close fas fa-chevron-right"></i>
                                <i class="open fas fa-chevron-down"></i>
                            </label>
                            <div id="detailsPanel">
                                <div class="row">
                                    <div class="list" id="listEmail" runat="server">
                                        <i class="fas fa-envelope"></i>
                                        <label id="tagEmail" class="tag" runat="server"> Verified</label>
                                        <p>Email ID</p>
                                        <asp:Label ID="lblEmail" class="txt" runat="server" Text="N/A"></asp:Label>
                                        <asp:LinkButton ID="linkEmail" class="link" runat="server" >Verify</asp:LinkButton>
                                    </div>

                                    <div class="list" id="listMobile" runat="server">
                                        <i class="fas fa-mobile-alt"></i>
                                        <label id="tagMobile" class="tag" runat="server">Not Verified</label>
                                        <p>Mobile</p>
                                        <asp:Label ID="lblMobile" class="txt" runat="server" Text="N/A"></asp:Label>
                                        <asp:LinkButton ID="linkMobile" class="link" runat="server" >Verify</asp:LinkButton>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="list" id="listKyc" runat="server">
                                        <i class="fas fa-passport"></i>
                                        <label id="tagKyc" class="tag" runat="server"> Verified</label>
                                        <p>Authentication Verify</p>
                                        <asp:Label ID="lblKyc" class="txt" runat="server" Text="N/A"></asp:Label>
                                        <asp:LinkButton ID="linkKyc" class="link" runat="server" OnClick="linkKyc_Click">Upload</asp:LinkButton>
                                    </div>

                                    <div class="list username">
                                        <i class="fas fa-key"></i>
                                        <p>User Id</p>
                                        <asp:Label ID="lblUserId" class="txt" runat="server" Text="N/A"></asp:Label>
                                        <asp:LinkButton ID="linkChangePassword" class="link" runat="server" OnClick="linkChangePassword_Click">Change password</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="window">
                            <input type="checkbox" id="info" class="checkbox" runat="server" />
                            <label for="info" id="btnInfo">
                                <i class="icon fas fa-info-circle"></i>Personal Information
                                <i class="close fas fa-chevron-right"></i>
                                <i class="open fas fa-chevron-down"></i>
                            </label>
                            <div id="infoPanel" class="infoPanel">
                                <div class="row">
                                    <div>Name</div>
                                    <div>:</div>
                                    <span id="lblName" runat="server">N/A</span>
                                </div>
                                <div class="row">
                                    <div>Date of Birth</div>
                                    <div>:</div>
                                    <span id="lblDob" runat="server">N/A</span>
                                </div>
                                <div class="row">
                                    <div>Gender</div>
                                    <div>:</div>
                                    <span id="lblGender" runat="server">N/A</span>
                                </div>
                                <div class="row">
                                    <div>Address</div>
                                    <div>:</div>
                                    <span id="lblAddress" runat="server">N/A</span>
                                </div>
                                <div class="row">
                                    <asp:LinkButton ID="linkAddress" class="link" runat="server" Visible="false" OnClick="linkAddress_Click">Provide Address</asp:LinkButton>
                                </div>
                                <hr />
                               
                                <div class="row">
                                    <div>Class Roll</div>
                                    <div>:</div>
                                    <span id="lblRoll" runat="server">8</span>
                                </div>
                               
                                <div class="row">
                                    <asp:LinkButton ID="linkBank" class="link" runat="server" Visible="false" OnClick="linkBank_Click">Provide Bank Details</asp:LinkButton>
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
