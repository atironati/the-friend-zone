<%@ Page Language="C#" AutoEventWireup="true" CodeFileBaseClass="edu.neu.ccis.ajt.BasePage" CodeFile="codebehind/FindFriends.cs" Inherits="edu.neu.ccis.ajt.FindFriends" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>The Friend Zone - a zone for friends!</title>
    <link rel="Shortcut Icon" href="images/friend_zone.ico" type="image/x-icon">
    <link type="text/css" rel="stylesheet" href="css/reset.css" />
    <link type="text/css" rel="stylesheet" href="css/main.css" />
    <link type="text/css" rel="stylesheet" href="css/profile.css" />
    <link type="text/css" rel="stylesheet" href="css/findfriends.css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.3/jquery.min.js"></script>
    <script type="text/javascript" src="javascript/resizeProfile.js"></script>
    <script type="text/javascript" src="javascript/searchForFriends.js"></script>
</head>
<body>
<form id="form1" runat="server">
<!--#include file="includes/header.inc"-->
<!--#include file="includes/profileInfoBox.inc"-->

    <div id="profileRightSideContainer"> 
        <div id="photosContainer">
        <div class="largeTop">
            <img src="images/header_find_friends.png" alt="Photos" />
        </div>
        <div class="largeMiddle">

            <asp:Label ID="findFriendsErrors" runat="server" />
            <br /><br />
            <asp:Panel ID="findFriendsGrid" runat="server" />

            <br /><br />
            

        </div>
        <div class="largeBottom">
    
        </div>
        </div>
    </div>



    </form>
</body>
</html>