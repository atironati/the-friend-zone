<%@ Page Language="C#" AutoEventWireup="true" CodeFileBaseClass="edu.neu.ccis.ajt.BasePage" CodeFile="codebehind/Friends.cs" Inherits="edu.neu.ccis.ajt.Friends" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>The Friend Zone - a zone for friends!</title>
    <link rel="Shortcut Icon" href="images/friend_zone.ico" type="image/x-icon">
    <link type="text/css" rel="stylesheet" href="css/reset.css" />
    <link type="text/css" rel="stylesheet" href="css/main.css" />
    <link type="text/css" rel="stylesheet" href="css/profile.css" />
    <link type="text/css" rel="stylesheet" href="css/photos.css" />
    <link type="text/css" rel="stylesheet" href="css/friends.css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.3/jquery.min.js"></script>
    <script type="text/javascript" src="javascript/resizeProfile.js"></script>
    <script type="text/javascript" src="javascript/searchForFriends.js"></script>
    <script type="text/javascript" src="javascript/friends.js"></script>
</head>
<body>
<form id="form1" runat="server">
<!--#include file="includes/header.inc"-->
<!--#include file="includes/profileInfoBox.inc"-->

    <asp:Panel id="grayout" runat="server">
    <asp:Panel id="photoViewer" runat="server">
    <asp:Label ID="photoViewerText" runat="server"></asp:Label>
    <asp:Image ID="photoViewerImage" src="images/logo.png" runat="server" /><br />
    <asp:Label ID="photoViewerCaption" runat="server" />
    </asp:Panel>
    </asp:Panel>

    <div id="profileRightSideContainer"> 
        <div id="photosContainer">
        <div class="largeTop">
            <img src="images/header_friends.png" alt="Photos" />
        </div>
        <div class="largeMiddle">

            <br /><br />
            <asp:Panel ID="friendsGrid" runat="server" />
            <asp:Label ID="friendsErrors" runat="server" />
            <br /><br />
            <div class="clear"></div>

        </div>
        <div class="largeBottom">
    
        </div>
        </div>
    </div>



    </form>
</body>
</html>