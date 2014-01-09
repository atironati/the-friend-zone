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
    public partial class Friends : edu.neu.ccis.ajt.BasePage
    {

        protected int profileId;
        protected bool editing = false;

        public void Page_Load(object sender, EventArgs e)
        {
            validateCookie();
            checkForQueryStringId();
            if (editing)
            {
                photosEditingSecurityCheck(profileId);
            }
            else
            {
                
            }
            displayProfileInformation();
            displayFriends();
            MyProfileLink.PostBackUrl = "profile.aspx?profileId=" + userId + "&editing=false";
            MyFriendsLink.PostBackUrl = "friends.aspx?profileId=" + userId + "&editing=true";
            MyPhotosLink.PostBackUrl = "photos.aspx?profileId=" + userId + "&editing=true";
        }

        public void checkForQueryStringId()
        {
            if (Request.QueryString["profileId"] != null)
                profileId = Convert.ToInt32(Request.QueryString["profileId"]);
            else
                Response.Redirect("error.aspx");

            if (Request.QueryString["editing"] != null)
                editing = Convert.ToBoolean(Request.QueryString["editing"]);
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

        public void displayFriends()
        {
            bool setToOpen = false;
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
                setToOpen = true;
            }
            SqlCommand cmd = new SqlCommand("SELECT friend_id FROM ajt.friends WHERE user_id = @user_id", connection);
            cmd.Parameters.AddWithValue("@user_id", profileId);
            SqlDataReader reader = cmd.ExecuteReader();

            friendsGrid.Controls.Clear();
            if (reader.HasRows)
            {
                int rowCounter = 0;
                while (reader.Read())
                {
                    String friend_id = reader["friend_id"].ToString();
                    String found_main_photo = "";
                    HtmlGenericControl brTag = new HtmlGenericControl("br");
                    String first_name = "";
                    String last_name = "";

                    using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["ajt"].ConnectionString))
                    {
                        cn2.Open();
                        SqlCommand cmd2 = new SqlCommand("SELECT first_name,last_name,main_photo FROM ajt.profile_info WHERE user_id = @friend_id", cn2);
                        cmd2.Parameters.AddWithValue("@friend_id", friend_id);
                        SqlDataReader reader2 = cmd2.ExecuteReader();
                        if (reader2.HasRows)
                        {
                            reader2.Read();
                            first_name = reader2["first_name"].ToString();
                            last_name = reader2["last_name"].ToString();
                            found_main_photo = reader2["main_photo"].ToString();
                        }
                        cn2.Close();
                    }

                    // Add img
                    Image img = new Image();
                    if (found_main_photo.Length > 0)
                        img.ImageUrl = "ImageHandler2.ashx?id=" + friend_id + "&maxWidth=150&maxHeight=150";
                    else
                    {
                        img.ImageUrl = "images/no_main_photo.png";
                        img.Height = 150;
                        img.Width = 150;
                    }

                    // Add the button for handling img clicks
                    LinkButton imgButton = new LinkButton();
                    imgButton.Attributes["class"] = "resultsProfileButtonImage";
                    imgButton.PostBackUrl = "profile.aspx?profileId=" + friend_id + "&editing=false";
                    imgButton.Controls.Add(img);

                    // Add the button for handling title name clicks
                    LinkButton nameButton = new LinkButton();
                    nameButton.Attributes["class"] = "resultsProfileButton";
                    nameButton.PostBackUrl = "profile.aspx?profileId=" + friend_id + "&editing=false";
                    nameButton.Text = first_name + " " + last_name;

                    // Add the div container for the image and image button
                    HtmlGenericControl imgDiv = new HtmlGenericControl("div");
                    imgDiv.Attributes["class"] = "resultsProfileContainer";
                    imgDiv.Controls.Add(nameButton);
                    imgDiv.Controls.Add(brTag);
                    imgDiv.Controls.Add(imgButton);

                    friendsGrid.Controls.Add(imgDiv);
                    if (rowCounter == 2)
                    {
                        friendsGrid.Controls.Add(brTag);
                    }
                    rowCounter = (rowCounter + 1) % 3;
                }
            }
            else
            {
                friendsErrors.Text = "You don't seem to have any friends :(";
            }
            if (setToOpen)
                connection.Close();
        }

        /*public void displayPhotos()
        {
            bool setToOpen = false;
            if (connection.State != ConnectionState.Open) {
                connection.Open();
                setToOpen = true;
            }
            SqlCommand cmd2 = new SqlCommand("SELECT photo_id,caption FROM ajt.photos WHERE user_id = @user_id ORDER BY date_time DESC", connection);
            cmd2.Parameters.AddWithValue("@user_id", userId);
            SqlDataReader reader = cmd2.ExecuteReader();

            imagesGrid.Controls.Clear();
            if (reader.HasRows)
            {
                int rowCounter = 0;
                while (reader.Read())
                {
                    String photo_id = reader["photo_id"].ToString();
                    HtmlGenericControl brTag = new HtmlGenericControl("br");

                    // Add img
                    Image img = new Image();
                    img.ImageUrl = "ImageHandler2.ashx?id=" + userId + "&photoId=" + photo_id + "&maxWidth=150&maxHeight=150";
                    img.Attributes["class"] = "photo";
                    img.Attributes["alt"] = reader["caption"].ToString();

                    // Add the button for handling img clicks
                    LinkButton imgButton = new LinkButton();
                    imgButton.Attributes["id"] = photo_id;
                    imgButton.Attributes["class"] = "photoButton";
                    if (editing)
                        imgButton.PostBackUrl = "viewphoto.aspx?profileId=" + userId + "&photoId=" + photo_id + "&editing=true";
                    else
                        imgButton.PostBackUrl = "viewphoto.aspx?profileId=" + userId + "&photoId=" + photo_id + "&editing=false";
                    imgButton.Controls.Add(img);

                    // Add the div container for the image and image button
                    HtmlGenericControl imgDiv = new HtmlGenericControl("div");
                    imgDiv.Attributes["class"] = "photoContainer";
                    imgDiv.Controls.Add(imgButton);

                    imgDiv.Controls.Add(brTag);

                    if (editing)
                    {
                        // Add button to delete image
                        Image img2 = new Image();
                        img2.ImageUrl = "images/delete.png";

                        LinkButton delButton = new LinkButton();
                        delButton.Attributes["class"] = "photoDeleteButton";
                        CommandEventHandler handler = new CommandEventHandler(delButton_Click);
                        delButton.Command += handler;
                        delButton.CommandArgument = photo_id;
                        delButton.Controls.Add(img2);
                        imgDiv.Controls.Add(delButton);
                    }

                    imagesGrid.Controls.Add(imgDiv);
                    if(rowCounter == 3){
                        imagesGrid.Controls.Add(brTag);
                    }
                    rowCounter = (rowCounter + 1) % 4;
                }
            }
            if(setToOpen)
                connection.Close();
        }*/

        /*public void delButton_Click(object sender, CommandEventArgs e)
        {
            connection.Open();
            int photo_id = Convert.ToInt32(e.CommandArgument);
            SqlCommand cmd1 = new SqlCommand("SELECT photo FROM ajt.photos WHERE photo_id = @photo_id", connection);
            cmd1.Parameters.AddWithValue("@photo_id", photo_id);
            String filepath = (String)cmd1.ExecuteScalar();
            System.IO.File.Delete(Server.MapPath(filepath));

            SqlCommand cmd2 = new SqlCommand("DELETE FROM ajt.photos WHERE photo_id = @photo_id", connection);
            cmd2.Parameters.AddWithValue("@photo_id", photo_id);
            cmd2.ExecuteNonQuery();
            connection.Close();
            Response.Redirect(Request.Url.ToString(), true);
        }*/

        //public void imgButton_Click(object sender, CommandEventArgs e)
       // {
        //    photoViewerText.Text = "yo";
        //    photoViewerImage.ImageUrl = "ImageHandler.ashx?id=" + userId + "&photoId=" + e.CommandArgument + "&maxWidth=" + 500 + "&maxHeight=" + 500;
        //    photoViewerCaption.Text = getImageCaption(Convert.ToInt32(e.CommandArgument));
        //}

        public void SearchForFriends_Click(object sender, EventArgs e)
        {
            string[] stringArray = searchForFriendsInput.Text.Split(' ');
            Response.Redirect("findfriends.aspx?profileId=" + userId + "&firstName=" + stringArray[0] + "&lastName=" + stringArray[1]);
        }

    }
}