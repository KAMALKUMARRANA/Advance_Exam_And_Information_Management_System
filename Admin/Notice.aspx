<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Notice.aspx.cs" Inherits="Admin_Notice" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Control Panel | Notice Board</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="CSS/Notice.css" rel="stylesheet" type="text/css" />
    <%
        string path = HttpContext.Current.Server.MapPath("Link.txt");
        string content = System.IO.File.ReadAllText(path);
        Response.Write(content);
    %>
</head>
<body>
    <form id="notice" runat="server">
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
                        <asp:Label ID="status" runat="server" Text="Control Panel / Notice Board"></asp:Label>
                    </div>
                    <div id="content">
                        <div class="intro">
                            <ul>
                                <li>Manage all updates here.</li>
                            </ul>
                        </div>
                        <div class="gvContainer">
                            <asp:GridView ID="gvUpdates" CssClass="gv" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                DataKeyNames="Id" OnRowCancelingEdit="gvUpdates_RowCancelingEdit" OnRowCommand="gvUpdates_RowCommand"
                                OnRowDeleting="gvUpdates_RowDeleting" OnRowEditing="gvUpdates_RowEditing" OnRowUpdating="gvUpdates_RowUpdating">
                                <Columns>
                                    <asp:TemplateField HeaderText="Update" ItemStyle-CssClass="gvitem" HeaderStyle-CssClass="gvheader">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("Date") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("Date") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="gvheader"></HeaderStyle>
                                        <ItemStyle CssClass="gvitem"></ItemStyle>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Message" ItemStyle-CssClass="gvitem" HeaderStyle-CssClass="gvheader">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("Message") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtMessage" CssClass="gvTextbox" runat="server" Text='<%# Bind("Message") %>'
                                                TextMode="MultiLine" Rows="10" MaxLength="500"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtMessage" CssClass="gvTextbox" placeholder="Message" autocomplete="off" runat="server"
                                                TextMode="MultiLine" Rows="10" MaxLength="500"></asp:TextBox>
                                        </FooterTemplate>
                                        <HeaderStyle CssClass="gvheader"></HeaderStyle>
                                        <ItemStyle CssClass="gvitem"></ItemStyle>
                                        <FooterStyle CssClass="gvitem"></FooterStyle>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="gvaction" HeaderStyle-CssClass="gvheader">
                                        <ItemTemplate>
                                            <asp:LinkButton CssClass="gvbtn" runat="server" CommandName="Edit" OnClientClick="return confirm('Do you want to edit the selected row?')"
                                                ToolTip="Edit"><i class="fas fa-user-edit"></i>Edit</asp:LinkButton>
                                            <asp:LinkButton ID="LinkButton1" CssClass="gvbtn delete" runat="server" CommandName="Delete"
                                                OnClientClick="return confirm('Do you want to delete the selected notice? It cannot be undone!')"
                                                ToolTip="Delete the entire row"><i class="fas fa-trash-alt delete"></i>Delete</asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton CssClass="gvbtn" runat="server" CommandName="Update" OnClientClick="return confirm('Do you want to update?')"
                                                ToolTip="Update row"><i class="fas fa-save"></i>Update</asp:LinkButton>
                                            <asp:LinkButton CssClass="gvbtn" runat="server" CommandName="Cancel" OnClientClick="return confirm('All unsaved data will be lost!')"
                                                ToolTip="Cancel editing"><i class="fas fa-window-close"></i>Cancel</asp:LinkButton>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:LinkButton CssClass="gvbtn" runat="server" CommandName="New" ToolTip="Add notice"
                                                OnClientClick="return confirm('Do you want to publish new notice?')"><i class="fas fa-user-plus"></i>Publish</asp:LinkButton>
                                        </FooterTemplate>
                                        <HeaderStyle CssClass="gvheader"></HeaderStyle>
                                        <ItemStyle CssClass="gvaction"></ItemStyle>
                                        <FooterStyle CssClass="gvaction"></FooterStyle>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <div class="error">
                                        <asp:Label ID="lblError" runat="server" Text="No data found."></asp:Label>
                                    </div>
                                </EmptyDataTemplate>
                            </asp:GridView>
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
