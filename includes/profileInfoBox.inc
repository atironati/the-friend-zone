<div id="infoBox">
  <div class="top">
    <div id="profileNameBox">
      <asp:Label ID="profileNameBoxText" Text="My Name" runat="server"></asp:Label>
    </div>
  </div>
  <div class="middle">

    <asp:Image ID="profilePhoto" ImageUrl="images/no_main_photo.png" Runat="server" />

    <table id="profileInfoTable">
      <tr>
        <td class="table_left">
          Birthday:
        </td>
        <td class="table_right">
          <asp:Panel ID="profileBirthdayPanel" runat="server">
            <asp:Label ID="profileBirthdayText" runat="server"></asp:Label>
          </asp:Panel>
        </td>
      </tr>
      <tr>
        <td class="table_left">
          Relationship Status:
        </td>
        <td class="table_right">
          <asp:Panel ID="profileRelationshipStatusPanel" runat="server">
            <asp:Label ID="profileRelationshipStatusText" runat="server"></asp:Label>
          </asp:Panel>
        </td>
      </tr>
      <tr>
        <td class="table_left">
          About Me:
        </td>
        <td class="table_right">
          <asp:Panel ID="profileAboutMePanel" runat="server">
            <asp:Label ID="profileAboutMeText" runat="server"></asp:Label>
          </asp:Panel>
        </td>
      </tr>
      <tr>
        <td class="table_left">
          Interests:
        </td>
        <td class="table_right">
          <asp:Panel ID="profileInterestsPanel" runat="server">
            <asp:Label ID="profileInterestsText" runat="server"></asp:Label>
          </asp:Panel>
        </td>
      </tr>
    </table>
    
  </div>
  <div class="bottom">
    <asp:Panel ID="ProfileEditingPanel" runat="server" />
  </div>
</div>