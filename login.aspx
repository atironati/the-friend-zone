<%@ Page Language="C#" AutoEventWireup="true" CodeFileBaseClass="edu.neu.ccis.ajt.BasePage" CodeFile="codebehind/Login.cs" Inherits="edu.neu.ccis.ajt.Login" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>The Friend Zone - a zone for friends!</title>
    <link rel="Shortcut Icon" href="images/friend_zone.ico" type="image/x-icon">
    <link type="text/css" rel="stylesheet" href="css/reset.css" />
    <link type="text/css" rel="stylesheet" href="css/login.css" />
    <link type="text/css" rel="stylesheet" href="css/main.css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.3/jquery.min.js"></script>
    <script type="text/javascript" src="javascript/login.js"></script>
    <script type="text/javascript" src="javascript/resize.js"></script>
</head>
<body>

<div id="header_left">
<img src="images/logo.png" id="logo" alt="The Friend Zone - a zone for friends!" />
</div>
<div id="header_right">
</div>

    <div id="loginBox">
    <div id="top">
    <div id="loginErrorBox" class="errorBox"><asp:Label ID="loginErrorBoxText" Text="" runat="server"></asp:Label></div>
    </div>
    <div id="middle">
    <form id="form1" runat="server">
        <table class="signInTable">
            <tr><td class="left_td"><asp:Label ID="usernameLabel" class="label" Text="Username:" runat="server" /></td>
                <td><asp:TextBox ID="usernameInput" runat="server" /></td></tr>
            <tr><td class="left_td"><asp:Label ID="passwordLabel" class="label" Text="Password:" runat="server" /></td>
                <td><asp:TextBox ID="passwordInput" TextMode="Password" runat="server" /></td></tr>
        </table><br />
        <asp:Button ID="signInButton" Text="" runat="server" OnClick="signInClick" />

        <div class="clear"></div>
        <hr />

        <table class="signInTable">
            <tr><td class="left_td"><asp:Label ID="firstNameLabel" class="label" Text="First Name:" runat="server" /></td>
                <td><asp:TextBox ID="firstNameInput" runat="server" /></td></tr>
            <tr><td class="left_td"><asp:Label ID="lastNameLabel" class="label" Text="Last Name:" runat="server" /></td>
                <td><asp:TextBox ID="lastNameInput" runat="server" /></td></tr>
            <tr><td class="left_td"><asp:Label ID="newUsernameLabel" class="label" Text="New Username:" runat="server" /></td>
                <td><asp:TextBox ID="newUsernameInput" runat="server" /></td></tr>
            <tr><td class="left_td"><asp:Label ID="newPasswordLabel" class="label" Text="New Password:" runat="server" /></td>
                <td><asp:TextBox ID="newPasswordInput" runat="server" /></td></tr>
            <tr><td class="left_td"><asp:Label ID="birthdayLabel" class="label" Text="Birthday:" runat="server" /></td>
                <td>
                <asp:DropDownList id="birthmonthDropdown" style="width: 90px;" runat="server"></asp:DropDownList>
                <asp:DropDownList id="birthdayDropdown" style="width: 41px;" runat="server"></asp:DropDownList>
                <asp:DropDownList id="birthyearDropdown" style="width: 55px;" runat="server"></asp:DropDownList>
                </td></tr>
        </table><br />  

        <asp:Button ID="createAccountButton" Text="" runat="server" OnClick="createAccountClick" />

        <span style="font-weight: bold;"><asp:Label ID="label5" runat="server" /></span>
    </form>
    </div>
    <div id="bottom">
    <div id="createAccountErrorBox" class="errorBox"><asp:Label ID="createAccountErrorBoxText" Text="" runat="server"></asp:Label></div>
    </div>
    </div>

</body>
</html>