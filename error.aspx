<%@ Page Language="C#" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>The Friend Zone - a zone for friends!</title>
    <link rel="Shortcut Icon" href="images/friend_zone.ico" type="image/x-icon">
    <link type="text/css" rel="stylesheet" href="css/reset.css" />
    <link type="text/css" rel="stylesheet" href="css/login.css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.3/jquery.min.js"></script>
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
    ERROR
    </div>
    <div id="bottom">
    <div id="createAccountErrorBox" class="errorBox"><asp:Label ID="createAccountErrorBoxText" Text="" runat="server"></asp:Label></div>
    </div>
    </div>

</body>
</html>