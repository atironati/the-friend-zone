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
    public partial class ViewPhoto : edu.neu.ccis.ajt.BasePage
    {

        protected int profileId;
        protected int photoId;
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
                editPhotoCaptionInput.Attributes["style"] = "display: none;";
                editCaptionSubmitButton.Attributes["style"] = "display: none;";
            }
            displayProfileInformation();
            MyProfileLink.PostBackUrl = "profile.aspx?profileId=" + userId + "&editing=false";
            MyFriendsLink.PostBackUrl = "friends.aspx?profileId=" + userId + "&editing=true";
            MyPhotosLink.PostBackUrl = "photos.aspx?profileId=" + userId + "&editing=true";
            displayPhoto();
        }

        public void checkForQueryStringId()
        {
            if (Request.QueryString["profileId"] != null)
                profileId = Convert.ToInt32(Request.QueryString["profileId"]);
            else
                Response.Redirect("error.aspx");

            if (Request.QueryString["photoId"] != null)
                photoId = Convert.ToInt32(Request.QueryString["photoId"]);
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
            displayPhotoComments();
        }

        public void displayPhotoComments()
        {
            bool setToOpen = false;
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
                setToOpen = true;
            }
            SqlCommand cmd = new SqlCommand("SELECT * FROM ajt.photo_comments WHERE photo_id = @photo_id ORDER BY date_time ASC", connection);
            cmd.Parameters.AddWithValue("@photo_id", photoId);
            SqlDataReader reader = cmd.ExecuteReader();

            photoCommentsGrid.Controls.Clear();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    String photo_id = reader["photo_id"].ToString();
                    String commenter_id = reader["commenter_id"].ToString();
                    String comment = reader["comment"].ToString();
                    DateTime date_time = (DateTime)reader["date_time"]; 
                    String first_name = "";
                    String last_name = "";
                    String main_photo = "";

                    using (SqlConnection cn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["ajt"].ConnectionString))
                    {
                        cn2.Open();
                        SqlCommand cmd2 = new SqlCommand("SELECT first_name,last_name,main_photo FROM ajt.profile_info WHERE user_id = @commenter_id", cn2);
                        cmd2.Parameters.AddWithValue("@commenter_id", commenter_id);
                        SqlDataReader reader2 = cmd2.ExecuteReader();
                        if (reader2.HasRows)
                        {
                            reader2.Read();
                            first_name = reader2["first_name"].ToString();
                            last_name = reader2["last_name"].ToString();
                            main_photo = reader2["main_photo"].ToString();
                        }
                        cn2.Close();
                    }

                    // photoCommentContainer
                    HtmlGenericControl photoCommentContainer = new HtmlGenericControl("div");
                    photoCommentContainer.Attributes["class"] = "photoCommentContainer";

                    // photoCommentTitleContainer
                    HtmlGenericControl photoCommentTitleContainer = new HtmlGenericControl("div");
                    photoCommentTitleContainer.Attributes["class"] = "photoCommentTitleContainer";

                    // photoCommentTitleTextContainer
                    HtmlGenericControl photoCommentTitleTextContainer = new HtmlGenericControl("div");
                    photoCommentTitleTextContainer.Attributes["class"] = "photoCommentTitleTextContainer";
                    // photoCommentProfileLink
                    LinkButton photoCommentProfileLink = new LinkButton();
                    photoCommentProfileLink.Attributes["class"] = "photoCommentProfileLink";
                    photoCommentProfileLink.PostBackUrl = "profile.aspx?profileId=" + commenter_id;
                    photoCommentProfileLink.Text = first_name + " " + last_name;
                    /* Add */ photoCommentTitleTextContainer.Controls.Add(photoCommentProfileLink);

                    // photoCommentTitleDateContainer
                    HtmlGenericControl photoCommentTitleDateContainer = new HtmlGenericControl("div");
                    photoCommentTitleDateContainer.Attributes["class"] = "photoCommentTitleDateContainer";
                    // photoCommentTitleDateText
                    HtmlGenericControl photoCommentTitleDateText = new HtmlGenericControl("span");
                    photoCommentTitleDateText.Attributes["class"] = "photoCommentTitleDateText";
                    photoCommentTitleDateText.InnerText = date_time.ToString("MMMM dd, yyyy") + " at " + date_time.ToString("h:mm tt").ToLower();
                    /* Add */ photoCommentTitleDateContainer.Controls.Add(photoCommentTitleDateText);

                    // clear div
                    HtmlGenericControl clearDiv = new HtmlGenericControl("div");
                    clearDiv.Attributes["class"] = "clear";

                    // photoCommentTextContainer
                    HtmlGenericControl photoCommentTextContainer = new HtmlGenericControl("div");
                    photoCommentTextContainer.Attributes["class"] = "photoCommentTextContainer";
                    // photoCommentText
                    HtmlGenericControl photoCommentText = new HtmlGenericControl("span");
                    photoCommentText.Attributes["class"] = "photoCommentText";
                    photoCommentText.InnerText = comment;
                    /* Add */ photoCommentTextContainer.Controls.Add(photoCommentText);

                    // commenterImage
                    Image cmmtrImage = new Image();
                    cmmtrImage.Attributes["class"] = "commenterImage";
                    cmmtrImage.ImageUrl = "ImageHandler2.ashx?id=" + commenter_id + "&width=38&height=38";

                    /* Add The Rest */
                    photoCommentTitleContainer.Controls.Add(photoCommentTitleTextContainer);
                    photoCommentTitleContainer.Controls.Add(photoCommentTitleDateContainer);
                    
                    photoCommentContainer.Controls.Add(photoCommentTitleContainer);
                    photoCommentContainer.Controls.Add(clearDiv);
                    photoCommentContainer.Controls.Add(cmmtrImage);
                    photoCommentContainer.Controls.Add(photoCommentTextContainer);
                    photoCommentsGrid.Controls.Add(photoCommentContainer);
                }
            }

            if (setToOpen)
                connection.Close();
        }

        public void displayPhoto()
        {
            bool setToOpen = false;
            if (connection.State != ConnectionState.Open) {
                connection.Open();
                setToOpen = true;
            }
            SqlCommand cmd = new SqlCommand("SELECT photo FROM ajt.photos WHERE photo_id = @photo_id", connection);
            cmd.Parameters.AddWithValue("@photo_id", photoId);
            String imgpath = (String)cmd.ExecuteScalar();

            viewPhotoImage.ImageUrl = "ImageHandler2.ashx?id=" + userId + "&photoId=" + photoId + "&maxWidth=700&maxHeight=500";
            viewPhotoCaption.Text = getImageCaption(photoId);

            if(setToOpen)
                connection.Close();
        }

        public void submitCommentButton_Click(object sender, EventArgs e)
        {
            if (photoCommentInput.Text.Length > 420) {
                photoCommentsError.Text = "Sorry, your comment is too long. Comments are limited to 420 characters";
            } else if(photoCommentInput.Text.Length == 0) {
                photoCommentsError.Text = "Sorry, your comment cannot be empty";
            } else {
                connection.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO ajt.photo_comments (photo_id,commenter_id,comment,date_time) VALUES (@photo_id,@commenter_id,@comment,@date_time)", connection);
                cmd.Parameters.AddWithValue("@photo_id", photoId);
                cmd.Parameters.AddWithValue("@commenter_id", userId);
                cmd.Parameters.AddWithValue("@comment", photoCommentInput.Text);
                cmd.Parameters.AddWithValue("@date_time", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
                connection.Close();
                Response.Redirect(Request.Url.ToString(), true);
            }
        }

        public void editCaptionButton_Click(object sender, EventArgs e)
        {
            if (photoCommentInput.Text.Length > 200)
            {
                photoCommentsError.Text = "Sorry, your caption is too long. Captions are limited to 200 characters";
            }
            else
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("UPDATE ajt.photos SET caption = @caption WHERE photo_id = @photo_id", connection);
                cmd.Parameters.AddWithValue("@photo_id", photoId);
                cmd.Parameters.AddWithValue("@caption", editPhotoCaptionInput.Text);
                cmd.ExecuteNonQuery();
                connection.Close();
                Response.Redirect(Request.Url.ToString(), true);
            }
        }

        public String getImageCaption(int photo_id)
        {
            bool setToOpen = false;
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
                setToOpen = true;
            }
            string sql = "SELECT caption FROM ajt.photos WHERE photo_id = @photo_id";
            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@photo_id", photo_id);
            String caption = (String)cmd.ExecuteScalar();
            if (setToOpen)
                connection.Close();
            return caption;
        }

        public void SearchForFriends_Click(object sender, EventArgs e)
        {
            string[] stringArray = searchForFriendsInput.Text.Split(' ');
            Response.Redirect("findfriends.aspx?profileId=" + userId + "&firstName=" + stringArray[0] + "&lastName=" + stringArray[1]);
        }

    }
}