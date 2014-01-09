<div id="header_left">
  <a href="dashboard.aspx"><img src="images/logo.png" id="logo" alt="The Friend Zone - a zone for friends!" /></a>
</div>
<div id="header_right">
  <div id="mainLinksBar">
    <ul class="master_navigation">
      <li>
        <asp:LinkButton id="MyProfileLink" runat="server">
          <div class="link_left"></div>
          <div class="link_middle">
            My Profile
          </div>
          <div class="link_right"></div>
        </asp:LinkButton>
      </li>

      <li>
        <asp:LinkButton id="MyFriendsLink" runat="server">
          <div class="link_left"></div>
          <div class="link_middle">
            My Friends
          </div>
          <div class="link_right"></div>
        </asp:LinkButton>
      </li>

      <li>
        <asp:LinkButton id="MyPhotosLink" runat="server">
          <div class="link_left"></div>
          <div class="link_middle">
            My Photos
          </div>
          <div class="link_right"></div>
        </asp:LinkButton>
      </li>
    </ul>
  </div>

  <div id="searchForFriendsBox">
    <asp:TextBox ID="searchForFriendsInput" Text="Find Friends!" OnSubmit="SearchForFriends" runat="server" />
  </div>

  <div id="searchForFriendsButtonBox">
    <asp:LinkButton id="SearchButtonLink" OnClick="SearchForFriends_Click" runat="server">
    
    </asp:LinkButton>
  </div>

  <div id="userAccountBox">
    <div id="userAccountLinksBox">
      <a href="" style="color: #f69100;">Edit Account</a>
      <br />
      <!-- hover ffcc00 -->
      <asp:LinkButton id="signOutButton" OnClick="signOutClick" style="color: #f69100;" runat="server">Sign Out</asp:LinkButton>
    </div>

    <div id="userAccountBoxLeft"></div>
    <div id="userAccountBoxMiddle">
      <table id="loggedInAsTable">
        <tr>
          <td>
            <span id="loggedInAsText">Logged in as:</span>
            <br />
            <div id="loggedInAsNameTextContainer">
              <asp:Label ID="loggedInAsNameText" runat="server"></asp:Label>
            </div>
          </td>
          <td>
            <!--<img src="images/no_main_photo_thumb.png" alt="You!" id="loggedInPhotoThumb" />-->
            <asp:Image ID="loggedInPhotoThumb" ImageUrl="images/no_main_photo_thumb.png" Runat="server" />
          </td>
        </tr>
      </table>
    </div>
    <div id="userAccountBoxRight"></div>
  </div>

</div>