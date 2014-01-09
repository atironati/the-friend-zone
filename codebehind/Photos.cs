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
    public partial class Photos : edu.neu.ccis.ajt.BasePage
    {

        protected int profileId;
        protected bool editing = false;

        public void Page_Load(object sender, EventArgs e)
        {
            validateCookie();
            checkForQueryStringId();
            if (editing)
            {
                imgUploadContainer.Visible = true;
                photosEditingSecurityCheck(profileId);
            }
            else
            {
                imgUploadContainer.Visible = false;
            }
            displayProfileInformation();
            MyProfileLink.PostBackUrl = "profile.aspx?profileId=" + userId + "&editing=false";
            MyFriendsLink.PostBackUrl = "friends.aspx?profileId=" + userId + "&editing=true";
            MyPhotosLink.PostBackUrl = "photos.aspx?profileId=" + userId + "&editing=true";
            displayPhotos();
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

        protected void uploadPhoto_Click(object sender, EventArgs e)
        {
            try
            {
                FileUpload img = (FileUpload)imgUpload;
                Byte[] imgByte = null;
                String savePath = "";
                if (img.HasFile && img.PostedFile != null)
                {
                    //To create a PostedFile
                    HttpPostedFile File = imgUpload.PostedFile;
                    //Create byte Array with file len
                    imgByte = new Byte[File.ContentLength];
                    //force the control to load data in array
                    File.InputStream.Read(imgByte, 0, File.ContentLength);

                    bool IsExists = System.IO.Directory.Exists(Server.MapPath("user_data/" + userId + "/"));
                    if (!IsExists)
                        System.IO.Directory.CreateDirectory(Server.MapPath("user_data/" + userId + "/"));
                    IsExists = System.IO.Directory.Exists(Server.MapPath("user_data/" + userId + "/photos/"));
                    if (!IsExists)
                        System.IO.Directory.CreateDirectory(Server.MapPath("user_data/" + userId + "/photos/"));

                    savePath = "~/final_project/user_data/" + userId + "/photos/" + img.FileName;
                    File.SaveAs(Server.MapPath(savePath));
                    File.InputStream.Close();
                    img.Dispose();
                } else {
                    throw new Exception();
                }
                connection.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO ajt.photos (user_id,photo,date_time,caption) VALUES (@user_id,@photo,@date_time,@caption)", connection);
                cmd.Parameters.AddWithValue("@user_id", userId);
                cmd.Parameters.AddWithValue("@photo", savePath);
                cmd.Parameters.AddWithValue("@date_time", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@caption", imgCaptionInput.Text);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch(System.Data.SqlClient.SqlException ex)
                {
                    lblResult.Text += ex.Message;
                    lblResult.Text += ex.InnerException;
                    lblResult.Text += ex.StackTrace;
                    lblResult.Text += ex.Errors;
                }

                lblResult.Text = "Success!";
                Response.Redirect(Request.Url.ToString(), true);
            }
            catch(Exception ex)
            {
                lblResult.Text = "There was an error";
            }
            finally
            {
                connection.Close();
            }
        }

        public void displayPhotos()
        {
            bool setToOpen = false;
            if (connection.State != ConnectionState.Open) {
                connection.Open();
                setToOpen = true;
            }
            SqlCommand cmd2 = new SqlCommand("SELECT photo_id,caption FROM ajt.photos WHERE user_id = @user_id ORDER BY date_time DESC", connection);
            cmd2.Parameters.AddWithValue("@user_id", profileId);
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
                    img.ImageUrl = "ImageHandler2.ashx?id=" + profileId + "&photoId=" + photo_id + "&maxWidth=150&maxHeight=150";
                    img.Attributes["class"] = "photo";
                    img.Attributes["alt"] = reader["caption"].ToString();

                    // Add the button for handling img clicks
                    LinkButton imgButton = new LinkButton();
                    imgButton.Attributes["id"] = photo_id;
                    imgButton.Attributes["class"] = "photoButton";
                    if (editing)
                        imgButton.PostBackUrl = "viewphoto.aspx?profileId=" + profileId + "&photoId=" + photo_id + "&editing=true";
                    else
                        imgButton.PostBackUrl = "viewphoto.aspx?profileId=" + profileId + "&photoId=" + photo_id + "&editing=false";
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
        }

        public void delButton_Click(object sender, CommandEventArgs e)
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
        }

        //public void imgButton_Click(object sender, CommandEventArgs e)
       // {
        //    photoViewerText.Text = "yo";
        //    photoViewerImage.ImageUrl = "ImageHandler.ashx?id=" + userId + "&photoId=" + e.CommandArgument + "&maxWidth=" + 500 + "&maxHeight=" + 500;
        //    photoViewerCaption.Text = getImageCaption(Convert.ToInt32(e.CommandArgument));
        //}

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