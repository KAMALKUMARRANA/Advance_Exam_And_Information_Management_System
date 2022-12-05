<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Refer.aspx.cs" Inherits="User_Refer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Touh & View | Refer</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="CSS/Refer.css" rel="stylesheet" type="text/css" />
    <%
        string path = HttpContext.Current.Server.MapPath("Link.txt");
        string content = System.IO.File.ReadAllText(path);
        Response.Write(content);
    %>
</head>
<body>
    <form id="refer" runat="server">
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
                        <asp:Label ID="status" runat="server" Text="Bring a Friend"></asp:Label>
                        <asp:Label ID="lblInfo" runat="server" Text="name (id)"></asp:Label>
                    </div>
                    <div id="content">
                        <div id="intro">
                            <h>Join your friend with us.</h>
                            <ul>
                                <li>Tell your friends about us.</li>
                                <li>Share <asp:LinkButton ID="btnShare"  runat="server" OnClick="btnShare_Click"><i class="fas fa-share-alt-square"></i></asp:LinkButton>
                                    the registration link with them.</li>
                                <li>Share your referral code (<asp:Label ID="lblRefId" runat="server" Text="RefId"></asp:Label>) also with them.</li>
                                <li>Tell them to put your referral code while registration.</li>
                            </ul>
                        </div>
                        <div class="divider">
                            <h>OR</h>
                        </div>
                        <div id="link">
                            <h>You can directly join your friends</h>
                            <asp:LinkButton ID="linkJoin" runat="server" OnClick="linkJoin_Click">Join</asp:LinkButton>
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
