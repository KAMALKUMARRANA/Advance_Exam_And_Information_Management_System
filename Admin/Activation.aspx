<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Activation.aspx.cs" Inherits="Admin_Activation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Control Panel | User Activation</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="CSS/Activation.css" rel="stylesheet" type="text/css" />
    <%
        string path = HttpContext.Current.Server.MapPath("Link.txt");
        string content = System.IO.File.ReadAllText(path);
        Response.Write(content);
    %>
</head>
<body>
    <form id="activation" runat="server">
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
                            <asp:Label ID="lblId" class="lbl" runat="server" Text="Id"></asp:Label> - <asp:Label ID="lblAdminName" class="lbl" runat="server" Text="Name"></asp:Label>
                        </span>
                    </div>
                    <div class="status bottom">
                        <i class="fas fa-angle-double-right"></i>
                        <asp:Label runat="server" Text="Control Panel / Member Activation"></asp:Label>
                    </div>
                    <div id="content">
                        <div class="intro">
                            <ul>
                                <li>Activate new candidate.</li>
                            </ul>
                        </div>

                        <div class="input-box">
                            <asp:TextBox ID="txtUserId" CssClass="input" type="text" autocomplete="off" runat="server" required
                                MaxLength="10"></asp:TextBox>
                            <label for="txtUserId" class="label-name">
                                <div class="icon"><i class="fas fa-key"></i></div>
                                <span class="content-name" id="lblKey" runat="server">Enter User Id</span>
                                <asp:Button ID="btnSearch" CssClass="btn" runat="server" Text="Search" OnClick="btnSearch_Click" />
                            </label>
                        </div>

                        <asp:Panel ID="panelActivation" CssClass="tabPanel" runat="server" Visible="false">
                            <div class="infoPanel">
                                <div class="row">
                                    <div>User Id</div>
                                    <div>:</div>
                                    <asp:Label ID="lblUserId" CssClass="lbl" runat="server" Text="N/A"></asp:Label>
                                </div>
                                <div class="row">
                                    <div>Name</div>
                                    <div>:</div>
                                    <asp:Label ID="lblName" CssClass="lbl" runat="server" Text="N/A"></asp:Label>
                                </div>
                            </div>

                            <div class="input-box ddl">
                                <asp:DropDownList ID="ddlPackage" CssClass="input ddl" required runat="server"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlPackage_SelectedIndexChanged">
                                    <asp:ListItem Value="Select Package" disabled Selected hidden>Select Package</asp:ListItem>
                                </asp:DropDownList>
                                <label for="ddlType" class="label-name">
                                    <div class="icon"><i class="fas fa-list-ul"></i></div>
                                    <span class="content-name">Type</span>
                                </label>
                            </div>

                            <div class="infoPanel">
                                <div class="row">
                                    <div>Payable amount</div>
                                    <div>:</div>
                                    <asp:Label ID="lblAmount" CssClass="lbl" runat="server" Text="0"></asp:Label>
                                </div>
                            </div>

                            <div class="btnContainer">
                                <asp:LinkButton ID="btnActivate" CssClass="btn" runat="server" ToolTip="Activate member account"
                                    OnClientClick="return confirm('Do you want to Peroceed?')" OnClick="btnActivate_Click"><i class="fas fa-check-circle"></i>Activate</asp:LinkButton>
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
