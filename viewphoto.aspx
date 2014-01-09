<%@ Page Language="C#" AutoEventWireup="true" CodeFileBaseClass="edu.neu.ccis.ajt.BasePage" CodeFile="codebehind/ViewPhoto.cs" Inherits="edu.neu.ccis.ajt.ViewPhoto" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>The Friend Zone - a zone for friends!</title>
    <link rel="Shortcut Icon" href="images/friend_zone.ico" type="image/x-icon">
    <link type="text/css" rel="stylesheet" href="css/reset.css" />
    <link type="text/css" rel="stylesheet" href="css/main.css" />
    <link type="text/css" rel="stylesheet" href="css/profile.css" />
    <link type="text/css" rel="stylesheet" href="css/viewphoto.css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.3/jquery.min.js"></script>
    <script type="text/javascript" src="javascript/resizeViewPhotos.js"></script>
    <script type="text/javascript" src="javascript/searchForFriends.js"></script>
</head>
<body>
<form id="form1" runat="server">
<!--#include file="includes/header.inc"-->
<!--#include file="includes/profileInfoBox.inc"-->

    <div id="profileRightSideContainer"> 
        <div id="photosContainer">
        <div class="largeTop">
            <img src="images/header_photos.png" alt="Photos" />
        </div>
        <div class="largeMiddle">

            <asp:Panel id="viewPhotoContainer" runat="server">
                <asp:Image ID="viewPhotoImage" runat="server" /><br />
                <asp:Label ID="viewPhotoCaption" runat="server" /><br />
                <div style="margin-top: 5px;">
                <asp:TextBox ID="editPhotoCaptionInput" runat="server" />
                <asp:Button ID="editCaptionSubmitButton" OnClick="editCaptionButton_Click" Text="Submit Caption" runat="server" />
                </div>
            </asp:Panel>

            <br /><hr /><br />

            <table id="commentPostingTable">
            <tr>
            <td>
            <asp:TextBox ID="photoCommentInput" textmode="multiline" TextWrapping="Wrap" AcceptsReturn="True" VerticalScrollBarVisibility="Visible" runat="server" />
            </td>
            <td style="vertical-align: top;"><asp:Button ID="photoCommentSubmitButton" Text="" runat="server" OnClick="submitCommentButton_Click" /></td>
            </tr>
            </table>

            <asp:Label ID="photoCommentsError" runat="server" />

            <br /><br />
            <asp:Panel ID="photoCommentsGrid" runat="server" />

            <div class="clear"></div>

        </div>
        <div class="largeBottom">
    
        </div>
        </div>
    </div>



    </form>
</body>
</html>