<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="User_Dashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User | Dashboard</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="CSS/Dashboard.css" rel="stylesheet" type="text/css" />
    <%
        string path = HttpContext.Current.Server.MapPath("Link.txt");
        string content = System.IO.File.ReadAllText(path);
        Response.Write(content);
    %>
</head>
<body>
    <form id="dashboard" runat="server">
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
                <div id="body">
                    <div class="status">
                        <i class="fas fa-angle-double-right"></i>
                        <asp:Label ID="status" runat="server" Text="Dashboard"></asp:Label>
                        <asp:Label ID="lblInfo" runat="server" Text="Welcome User"></asp:Label>
                    </div>
                    <div id="content">
                       
                        <div id="update" class="window" runat="server">
                            <p>New Update</p>
                        </div>
                        <asp:Panel ID="task" class="window" runat="server" Visible="false">
                            <p>Daily Task</p>
                            <div class="lbl">
                                <asp:Label ID="lblDailyTask" runat="server" Text="Daily task Pending."></asp:Label>
                            </div>
                        </asp:Panel>
                        <div id="info" class="window">
                            <div class="tab">
                                <p>Account Status</p>
                                <div class="content">
                                    <i class="fas fa-user"></i>
                                    <asp:Label ID="lblStatus" runat="server" Text="Active"></asp:Label>
                                </div>
                            </div>
                            <div class="tab">
                                <p>Apear in Exam</p>
                                <div class="content">
                                    <i class="fas fa-tachometer-alt"></i>
                                    <asp:Label ID="lblBoost" runat="server" Text="Yes"></asp:Label>
                                </div>
                            </div>
                            <div class="tab">
                                <p>Previous Exam</p>
                                <div class="content">
                                    
                                    <asp:Label ID="lblBalance" runat="server" Text="3"></asp:Label>
                                </div>
                            </div>
                            <div class="tab">
                                <p>Latest Exam Marks</p>
                                <div class="content">
                                    
                                    <asp:Label ID="lblFund" runat="server" Text="90"></asp:Label>
                                </div>
                            </div>
                            <div id="DaiyIncomeTab" class="tab" runat="server">
                                <p>Daily Task</p>
                                <div class="content">
                                    
                                    <asp:Label ID="lblIncome" runat="server" Text="2"></asp:Label>
                                </div>
                            </div>
                            <div class="tab">
                                <p>Class Details</p>
                                <div class="content">
                                    <i class="fas fa-users"></i>
                                    <asp:Label ID="lblTeam" runat="server" Text="BCA-5th Sem"></asp:Label>
                                </div>
                            </div>
                            <div class="tab">
                                <p>College</p>
                                <div class="content">
                                    
                                    <asp:Label ID="lblValidity" runat="server" Text="Midnapore College"></asp:Label>
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
