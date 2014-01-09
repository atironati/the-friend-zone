<%@ Page Language="C#" AutoEventWireup="true" CodeFileBaseClass="edu.neu.ccis.ajt.BasePage" CodeFile="codebehind/Photos.cs" Inherits="edu.neu.ccis.ajt.Photos" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>The Friend Zone - a zone for friends!</title>
    <link rel="Shortcut Icon" href="images/friend_zone.ico" type="image/x-icon">
    <link type="text/css" rel="stylesheet" href="css/reset.css" />
    <link type="text/css" rel="stylesheet" href="css/main.css" />
    <link type="text/css" rel="stylesheet" href="css/profile.css" />
    <link type="text/css" rel="stylesheet" href="css/photos.css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.3/jquery.min.js"></script>
    <script type="text/javascript" src="javascript/resizeProfile.js"></script>
    <script type="text/javascript" src="javascript/photos.js"></script>
    <script type="text/javascript" src="javascript/searchForFriends.js"></script>
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
            <img src="images/header_photos.png" alt="Photos" />
        </div>
        <div class="largeMiddle">

            <asp:Panel ID="imgUploadContainer" runat="server">
            <asp:FileUpload ID="imgUpload" runat="server" />
            <asp:Button ID="btnSubmit" runat="server" onclick="uploadPhoto_Click" Text="Submit" />
            <asp:TextBox ID="imgCaptionInput" runat="server" />
            <asp:Label ID="lblResult" runat="server" ForeColor="#0066FF"></asp:Label>
            </asp:Panel>

            <br /><br />
            <asp:Panel ID="imagesGrid" runat="server" />

            <br /><br />
            

        </div>
        <div class="largeBottom">
    
        </div>
        </div>
    </div>



    </form>
</body>
</html>