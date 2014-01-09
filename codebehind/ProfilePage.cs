using edu.neu.ccis.rasala;
using System;
using System.Security.Cryptography;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;

namespace edu.neu.ccis.ajt
{
    public partial class ProfilePage : edu.neu.ccis.ajt.BasePage
    {

        protected int profileId;
        protected bool editing = false;

        protected override void OnInit(EventArgs e)
        {
            // Relationship Status
            TextBox relationshipEditingBox = new TextBox();
            relationshipEditingBox.Text = profileRelationshipStatusText.Text;
            relationshipEditingBox.Width = 230;
            relationshipEditingBox.Wrap = true;
            relationshipEditingBox.ID = "relationshipEditingBox";
            profileRelationshipStatusPanel.Controls.Add(relationshipEditingBox);

            // About Me
            TextBox aboutMeEditingBox = new TextBox();
            aboutMeEditingBox.Text = profileAboutMeText.Text;
            aboutMeEditingBox.Width = 230;
            aboutMeEditingBox.Wrap = true;
            aboutMeEditingBox.Height = 200;
            aboutMeEditingBox.TextMode = TextBoxMode.MultiLine;
            aboutMeEditingBox.ID = "aboutMeEditingBox";
            aboutMeEditingBox.Attributes["runat"] = "server";
            profileAboutMePanel.Controls.Add(aboutMeEditingBox);

            // Interests
            TextBox interestsEditingBox = new TextBox();
            interestsEditingBox.Text = profileInterestsText.Text;
            interestsEditingBox.Width = 230;
            interestsEditingBox.Wrap = true;
            interestsEditingBox.Height = 200;
            interestsEditingBox.TextMode = TextBoxMode.MultiLine;
            interestsEditingBox.ID = "interestsEditingBox";
            interestsEditingBox.Attributes["runat"] = "server";
            profileInterestsPanel.Controls.Add(interestsEditingBox);

            base.OnInit(e);
        }

        public void Page_Load(object sender, EventArgs e)
        {
            validateCookie();
            checkForQueryStringId();
            displayProfileInformation();

            editingControls();

            MyProfileLink.PostBackUrl = "profile.aspx?profileId=" + userId + "&editing=false";
            MyFriendsLink.PostBackUrl = "friends.aspx?profileId=" + userId + "&editing=true";
            MyPhotosLink.PostBackUrl = "photos.aspx?profileId=" + userId + "&editing=true";
            ViewPhotosLink.PostBackUrl = "photos.aspx?profileId=" + profileId + "&editing=false";
        }

        public void checkForQueryStringId()
        {
            if (Request.QueryString["profileId"] != null)
            {
                profileId = Convert.ToInt32(Request.QueryString["profileId"]);
            }
            else
                Response.Redirect("error.aspx");

            if (Request.QueryString["editing"] != null)
            {
                editing = Convert.ToBoolean(Request.QueryString["editing"]);
            }
        }

        public void displayProfileInformation()
        {
            displayFirstLastName(userId, loggedInAsNameText);
            displayFirstLastName(profileId, profileNameBoxText);
            displayUserAccountImage(userId, loggedInPhotoThumb, 38, 38, false);
            displayUserAccountImage(profileId, profilePhoto, 300, 500, true);
            displayBirthday(profileId, profileBirthdayText);
            displayRelationshipStatus(profileId, profileRelationshipStatusText);
            displayAboutMe(profileId, profileAboutMeText);
            displayInterests(profileId, profileInterestsText);
        }

        public void editingControls()
        {
            if (editing)
            {
                if (userId == profileId)
                {
                    // Relationship Status
                    profileRelationshipStatusText.Attributes["style"] = "display: none;";
                    TextBox blah = (TextBox)profileRelationshipStatusPanel.FindControl("relationshipEditingBox");
                    blah.Visible = true;

                    // About Me
                    profileAboutMeText.Attributes["style"] = "display: none;";
                    TextBox blah2 = (TextBox)profileAboutMePanel.FindControl("aboutMeEditingBox");
                    blah2.Visible = true;

                    // Interests
                    profileInterestsText.Attributes["style"] = "display: none;";
                    TextBox blah3 = (TextBox)profileInterestsPanel.FindControl("interestsEditingBox");
                    blah3.Visible = true;

                    TextBox interestsEditingBox = new TextBox();
                    interestsEditingBox.Visible = false;

                    //profileRelationshipStatusPanel.Controls.Clear();
                    //profileAboutMePanel.Controls.Clear();
                    //profileInterestsPanel.Controls.Clear();
                    //profileRelationshipStatusPanel.Controls.Add(relationshipEditingBox);
                    //profileAboutMePanel.Controls.Add(aboutMeEditingBox);
                    profileInterestsPanel.Controls.Add(interestsEditingBox);

                    // Submit Button
                    LinkButton editingSubmitLink = new LinkButton();
                    editingSubmitLink.Command += new CommandEventHandler(this.UpdateProfileInfo);
                    editingSubmitLink.Attributes["runat"] = "server";
                    editingSubmitLink.Attributes["id"] = "submitLink";

                    ProfileEditingPanel.Controls.Clear();
                    ProfileEditingPanel.Controls.Add(editingSubmitLink);
                }
                else
                    Response.Redirect("error.aspx");
            }
            else
            {
                if (userId == profileId)
                {
                    LinkButton editingLink = new LinkButton();
                    String currentUrl = Request.Url.ToString();
                    string[] urlArray = currentUrl.Split('&');
                    if (urlArray.Length >= 2)
                    {
                        editingLink.PostBackUrl = urlArray[0] + "&editing=true";
                    }
                    else
                    {
                        editingLink.PostBackUrl = currentUrl + "&editing=true";
                    }
                    editingLink.Attributes["id"] = "editMyInfoLink";

                    ProfileEditingPanel.Controls.Clear();
                    ProfileEditingPanel.Controls.Add(editingLink);
                }
                profileRelationshipStatusText.Attributes["style"] = "display: inline;";
                TextBox blah = (TextBox)profileRelationshipStatusPanel.FindControl("relationshipEditingBox");
                blah.Visible = false;
                profileAboutMeText.Attributes["style"] = "display: inline;";
                TextBox blah2 = (TextBox)profileAboutMePanel.FindControl("aboutMeEditingBox");
                blah2.Visible = false;
                profileInterestsText.Attributes["style"] = "display: inline;";
                TextBox blah3 = (TextBox)profileInterestsPanel.FindControl("interestsEditingBox");
                blah3.Visible = false;
            }
        }

        public void UpdateProfileInfo(Object sender, EventArgs e)
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand("UPDATE ajt.profile_info SET relationship_status = @relationship_status, about_me = @about_me, interests = @interests WHERE user_id = @user_id", connection);
            cmd.Parameters.AddWithValue("@user_id", userId);

            TextBox objBox = new TextBox();
            objBox = (TextBox)profileRelationshipStatusPanel.FindControl("relationshipEditingBox");
            TextBox objBox2 = new TextBox();
            objBox2 = (TextBox)profileRelationshipStatusPanel.FindControl("aboutMeEditingBox");
            TextBox objBox3 = new TextBox();
            objBox3 = (TextBox)profileInterestsPanel.FindControl("interestsEditingBox");

            cmd.Parameters.AddWithValue("@relationship_status", objBox.Text);
            cmd.Parameters.AddWithValue("@about_me", objBox2.Text);
            cmd.Parameters.AddWithValue("@interests", objBox3.Text);
            cmd.ExecuteNonQuery();
            connection.Close();

            String currentUrl = Request.Url.ToString();
            string[] urlArray = currentUrl.Split('&');
            currentUrl = urlArray[0] + "&editing=false";
            Response.Redirect(currentUrl);
        }

        public void SendFriendRequest(Object sender, EventArgs e)
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO ajt.friend_requests (user_id,friend_id,date_time) VALUES (@user_id,@friend_id,@date_time)", connection);
            cmd.Parameters.AddWithValue("@user_id", profileId);
            cmd.Parameters.AddWithValue("@friend_id", userId);
            cmd.Parameters.AddWithValue("@date_time", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.ExecuteNonQuery();
            connection.Close();
            profileMessageLabel.Text = "Friend Request Sent!";
        }

        public void SearchForFriends_Click(object sender, EventArgs e)
        {
            string[] stringArray = searchForFriendsInput.Text.Split(' ');
            Response.Redirect("findfriends.aspx?profileId=" + userId + "&firstName=" + stringArray[0] + "&lastName=" + stringArray[1]);
        }
    }
}