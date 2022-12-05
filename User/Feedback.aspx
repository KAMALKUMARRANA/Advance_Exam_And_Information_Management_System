<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Feedback.aspx.cs" Inherits="User_Feedback" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Toch & View | Feedback</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="CSS/Feedback.css" rel="stylesheet" type="text/css" />
    <%
        string path = HttpContext.Current.Server.MapPath("Link.txt");
        string content = System.IO.File.ReadAllText(path);
        Response.Write(content);
    %>
</head>
<body>
    <form id="feedback" runat="server">
        <asp:ScriptManager ID="scriptPage" runat="server">
        </asp:ScriptManager>
        <asp:UpdateProgress ID="progressUpdate" AssociatedUpdatePanelID="pageUpdate" runat="server">
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
                    <div class="status">
                        <i class="fas fa-angle-double-right"></i>
                        <asp:Label ID="status" runat="server" Text="Feedback"></asp:Label>
                        <asp:Label ID="lblInfo" runat="server" Text="Welcome User"></asp:Label>
                    </div>
                    <div id="content">
                        <div id="intro">
                            <h>Your valueable feedback is importmant to us.</h>
                            <ul>
                                <li>Please give your valueable feedback regularly to improve our services.</li>
                                
                            </ul>
                        </div>
                        <div id="form">
                            <div class="input-box">
                                <asp:DropDownList ID="ddlCategory" class="input" runat="server" required>
                                    <asp:ListItem Value="Select category" disabled Selected hidden>Select category</asp:ListItem>
                                    <asp:ListItem>Development</asp:ListItem>
                                    <asp:ListItem>Interface Access</asp:ListItem>
                                    <asp:ListItem>Others</asp:ListItem>
                                </asp:DropDownList>
                                <label for="name" class="label-name">
                                    <span class="content-name">Category</span>
                                </label>
                            </div>

                            <div class="input-box">
                                <asp:TextBox ID="txtMessage" CssClass="input" type="text" autocomplete="off" runat="server" placeholder="Briefly describe your message"
                                    TextMode="MultiLine" Rows="10" MaxLength="500"></asp:TextBox>
                                <label for="txtMessage" class="label">
                                    <span class="content">Message</span>
                                </label>
                            </div>

                            <div class="btnContainer">
                                <asp:Button ID="btnSend" CssClass="btn" runat="server" Text="Send Feedback" OnClientClick="return confirm('Do you want to send feedback?')" OnClick="btnSend_Click" />
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
