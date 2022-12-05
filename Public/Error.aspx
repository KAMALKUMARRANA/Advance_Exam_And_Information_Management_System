<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="Public_Error" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>404 Page not found</title>
    <link href="CSS/Error.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="error" runat="server">
        <div class="container">
            <h2>Oops! You are lost.</h2>
            <h1>404</h1>
            <p>We can't find the page you're looking for.</p>
            <a href="../User/Dashboard.aspx">Go back home</a>
        </div>
    </form>
</body>
</html>
