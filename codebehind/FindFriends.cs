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
    public partial class FindFriends : edu.neu.ccis.ajt.BasePage
    {

        protected int profileId;
        protected String firstName;
        protected String lastName;
        protected bool editing = false;

        public void Page_Load(object sender, EventArgs e)
        {
            validateCookie();
            checkForQueryStringId();
            displayProfileInformation();
            MyProfileLink.PostBackUrl = "profile.aspx?profileId=" + userId + "&editing=false";
            MyFriendsLink.PostBackUrl = "friends.aspx?profileId=" + userId + "&editing=true";
            MyPhotosLink.PostBackUrl = "photos.aspx?profileId=" + userId + "&editing=true";
            displayFriendResults();
        }

        public void checkForQueryStringId()
        {
            if (Request.QueryString["profileId"] != null)
                profileId = Convert.ToInt32(Request.QueryString["profileId"]);
            else
                Response.Redirect("error.aspx");

            if (Request.QueryString["firstName"] != null)
                firstName = Request.QueryString["firstName"].ToString();
            else
                Response.Redirect("error.aspx");

            if (Request.QueryString["lastName"] != null)
                lastName = Request.QueryString["lastName"].ToString();
            else
                Response.Redirect("error.aspx");
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

        public void displayFriendResults()
        {
            bool setToOpen = false;
            if (connection.State != ConnectionState.Open) {
                connection.Open();
                setToOpen = true;
            }
            SqlCommand cmd = new SqlCommand("SELECT user_id, main_photo FROM ajt.profile_info WHERE first_name = @first_name AND last_name = @last_name", connection);
            cmd.Parameters.AddWithValue("@first_name", firstName);
            cmd.Parameters.AddWithValue("@last_name", lastName);
            SqlDataReader reader = cmd.ExecuteReader();

            findFriendsGrid.Controls.Clear();
            if (reader.HasRows)
            {
                int rowCounter = 0;
                while (reader.Read())
                {
                    String found_id = reader["user_id"].ToString();
                    String found_main_photo = reader["main_photo"].ToString();
                    HtmlGenericControl brTag = new HtmlGenericControl("br");

                    // Add img
                    Image img = new Image();
                    if (found_main_photo.Length > 0)
                        img.ImageUrl = "ImageHandler2.ashx?id=" + found_id + "&maxWidth=150&maxHeight=150";
                    else
                    {
                        img.ImageUrl = "images/no_main_photo.png";
                        img.Height = 150;
                        img.Width = 150;
                    }
                    //img.Attributes["class"] = "photo";
                    //img.Attributes["alt"] = reader["caption"].ToString();

                    // Add the button for handling img clicks
                    LinkButton imgButton = new LinkButton();
                    imgButton.Attributes["class"] = "resultsProfileButtonImage";
                    imgButton.PostBackUrl = "profile.aspx?profileId=" + found_id + "&editing=false";
                    imgButton.Controls.Add(img);

                    // Add the button for handling title name clicks
                    LinkButton nameButton = new LinkButton();
                    nameButton.Attributes["class"] = "resultsProfileButton";
                    nameButton.PostBackUrl = "profile.aspx?profileId=" + found_id + "&editing=false";
                    nameButton.Text = firstName + " " + lastName;

                    // Add the div container for the image and image button
                    HtmlGenericControl imgDiv = new HtmlGenericControl("div");
                    imgDiv.Attributes["class"] = "resultsProfileContainer";
                    imgDiv.Controls.Add(nameButton);
                    imgDiv.Controls.Add(brTag);
                    imgDiv.Controls.Add(imgButton);

                    findFriendsGrid.Controls.Add(imgDiv);
                    if (rowCounter == 3)
                    {
                        findFriendsGrid.Controls.Add(brTag);
                    }
                    rowCounter = (rowCounter + 1) % 4;
                }
            }
            else
            {
                findFriendsErrors.Text = "No Records Found for" + firstName + " " + lastName;
            }
            if(setToOpen)
                connection.Close();
        }

        public void SearchForFriends_Click(object sender, EventArgs e)
        {
            string[] stringArray = searchForFriendsInput.Text.Split(' ');
            Response.Redirect("findfriends.aspx?profileId=" + userId + "&firstName=" + stringArray[0] + "&lastName=" + stringArray[1]);
        }

    }
}