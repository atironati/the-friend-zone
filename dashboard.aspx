<%@ Page Language="C#" AutoEventWireup="true" CodeFileBaseClass="edu.neu.ccis.ajt.BasePage" CodeFile="codebehind/Dashboard.cs" Inherits="edu.neu.ccis.ajt.Dashboard" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>The Friend Zone - a zone for friends!</title>
    <link rel="Shortcut Icon" href="images/friend_zone.ico" type="image/x-icon">
    <link type="text/css" rel="stylesheet" href="css/reset.css" />
    <link type="text/css" rel="stylesheet" href="css/profile.css" />
    <link type="text/css" rel="stylesheet" href="css/main.css" />
    <link type="text/css" rel="stylesheet" href="css/dashboard.css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.3/jquery.min.js"></script>
    <script type="text/javascript" src="javascript/resizeProfile.js"></script>
    <script type="text/javascript" src="javascript/searchForFriends.js"></script>
</head>
<body>
<form id="form1" runat="server">
<!--#include file="includes/header.inc"-->
<!--#include file="includes/dashboardInfoBox.inc"-->

    <div id="profileRightSideContainer"> 
        <div id="photosContainer">
        <div class="largeTop">
            
        </div>
        <div class="largeMiddle">

            <asp:Label ID="lblImage" runat="server" Text="Upload an Image!"></asp:Label>
            <asp:FileUpload ID="imgUpload" runat="server" />
            <asp:Button ID="btnSubmit" runat="server" onclick="btnSubmit_Click" Text="Submit" />
   
            <asp:Label ID="lblResult" runat="server" ForeColor="#0066FF"></asp:Label>

            <div>
                <asp:Image ID="Image1" ImageUrl="images/no_main_photo_thumb.png" Runat="server" />
            </div>

        </div>
        <div class="largeBottom">
    
        </div>
        </div>
    </div>

    </form>
</body>
</html>