<%@ Page Language="C#" AutoEventWireup="true" CodeFileBaseClass="edu.neu.ccis.ajt.BasePage" CodeFile="codebehind/ProfilePage.cs" Inherits="edu.neu.ccis.ajt.ProfilePage" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>The Friend Zone - a zone for friends!</title>
    <link rel="Shortcut Icon" href="images/friend_zone.ico" type="image/x-icon">
    <link type="text/css" rel="stylesheet" href="css/reset.css" />
    <link type="text/css" rel="stylesheet" href="css/main.css" />
    <link type="text/css" rel="stylesheet" href="css/profile.css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.3/jquery.min.js"></script>
    <script type="text/javascript" src="javascript/resizeProfile.js"></script>
    <script type="text/javascript" src="javascript/searchForFriends.js"></script>
    <script type="text/javascript" src="javascript/profile.js"></script>
</head>
<body>
<form id="form1" runat="server">
<!--#include file="includes/header.inc"-->
<!--#include file="includes/profileInfoBox.inc"-->

    <div id="profileRightSideContainer"> 
        <div id="topPanel">
            <div class="largeTop">
                <asp:LinkButton ID="SendFriendRequestLink" runat="server" OnClick="SendFriendRequest"><img src="images/send_friend_request.png" id="sendFriendRequestImg" /></asp:LinkButton>
                <asp:LinkButton ID="ViewPhotosLink" runat="server"><img src="images/view_photos.png" id="viewPhotosImg" /></asp:LinkButton>
                <asp:Label ID="profileMessageLabel" runat="server" />
            </div>
            <div class="largeMiddle">
            - Add to friends
            - Recent photos
            </div>
            <div class="largeBottom">
    
            </div>
        </div>

        <div id="Div1">
        <div class="largeTop">
    
        </div>
        <div class="largeMiddle">
        
        </div>
        <div class="largeBottom">
    
        </div>
        </div>
    </div>



    </form>
</body>
</html>