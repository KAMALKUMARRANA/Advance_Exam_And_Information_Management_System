<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="User_Register" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Registration</title>
    <link href="bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="container">
<div class="row">
<div class="col-md-8 mx-auto">
<div class="card">
<div class="card-body">


<div class="row">
<div class="col">
<center>
    <img src="../Images/generaluser.png" width="100px"/>
</center>
</div>
</div>


<div class="row">
<div class="col">
<center>
    <h4>User Registration</h4>
</center>
</div>
</div>


<div class="row">
<div class="col">
<hr />
</div>
</div>

<div class="row">
<div class="col-md-6">
<asp:Label ID="Label1" runat="server" Text="Label" >Full Name</asp:Label>
   <asp:TextBox CssClass="form-control" ID="txtMemid" runat="server" placeholder="Full Name"></asp:TextBox>

</div>
<div class="col-md-6">
<asp:Label ID="Label2" runat="server" Text="Label" >Date of Birth</asp:Label>
   <asp:TextBox CssClass="form-control" ID="txtDob" runat="server" 
        placeholder="Password" TextMode="Date">
</asp:TextBox>

</div>
</div>

<div class="row">
<div class="col-md-6">
<asp:Label ID="Label3" runat="server" Text="Label" >Contact Number</asp:Label>
   <asp:TextBox CssClass="form-control" ID="txtPhone" runat="server" placeholder="Contact Number" TextMode="Phone"></asp:TextBox>

</div>
<div class="col-md-6">
<asp:Label ID="Label4" runat="server" Text="Label" >Email</asp:Label>
   <asp:TextBox CssClass="form-control" ID="txtEmail" runat="server" 
        placeholder="Email" TextMode="Email">
</asp:TextBox>

</div>
</div>

<div class="row">
<div class="col-md-4">
<asp:Label ID="Label5" runat="server" Text="Label" >State</asp:Label>
   <div class="form-group">
       <asp:DropDownList  CssClass="form-control" ID="DropDownList1" runat="server">
       <asp:ListItem Text="Select" Value="select">  </asp:ListItem>
       </asp:DropDownList>
</div>
</div>

<div class="col-md-4">
<div class="form-group">
    <asp:Label ID="Label6" runat="server" Text="Label">City</asp:Label>
    <asp:TextBox CssClass="form-control" ID="TextBox4" runat="server" placeholder="City"></asp:TextBox>

</div>
</div>

<div class="col-md-4">
<asp:Label ID="Label7" runat="server" Text="Label" >Pin Code</asp:Label>
   <asp:TextBox CssClass="form-control" ID="TextBox6" runat="server" 
        placeholder="" TextMode="Number">
</asp:TextBox>

</div>
</div>

<div class="row">
<div class="col">
<asp:Label ID="Label8" runat="server" Text="Label" >Full Address</asp:Label>
   <asp:TextBox CssClass="form-control" ID="TextBox5" runat="server" placeholder="Full Address" TextMode="MultiLine" Rows="2"></asp:TextBox>

</div>
</div>
<br />

<center>
<div class="row">
<div class="col">
<span class="badge badge-pill badge-info">Login Credentials</span>
</div>
</div>
</center>

<div class="row">
<div class="col-md-6">
<div class="form-group">
    

<asp:Label ID="Label9" runat="server" Text="Label" >User ID</asp:Label>
   <asp:TextBox CssClass="form-control" ID="txtUserid" runat="server" placeholder="User ID"></asp:TextBox>

</div>
</div>
<div class="col-md-6">
<asp:Label ID="Label10" runat="server" Text="Label" >Password</asp:Label>
   <asp:TextBox CssClass="form-control" ID="txtPass" runat="server" 
        placeholder="Password" TextMode="Password">
</asp:TextBox>

</div>
</div>




<div class="row">
<div class="col">
<div class="form-group">
     <asp:Button class="btn btn-success btn-block btn-lg" ID="Button1" 
         runat="server" Text="Sign Up" />
</div>
</div>
</div>









</div>
</div>
<a href="../Default.aspx" target="_blank"><< Back to Home</a>
</div>
</div>
</div>
    </form>
    <a href="">../Default.aspx</a>
</body>
</html>
