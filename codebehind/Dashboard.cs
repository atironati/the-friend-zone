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
using edu.neu.ccis.ajt;
using System.IO;

namespace edu.neu.ccis.ajt
{
    public partial class Dashboard : edu.neu.ccis.ajt.BasePage
    {

        public void Page_Load(object sender, EventArgs e)
        {
            validateCookie();
            displayFirstLastName(userId, loggedInAsNameText);
            displayUserAccountImage(userId, loggedInPhotoThumb, 38, 38, false);
            displayFriendRequests();
            MyProfileLink.PostBackUrl = "profile.aspx?profileId=" + userId + "&editing=false";
            MyFriendsLink.PostBackUrl = "friends.aspx?profileId=" + userId + "&editing=true";
            MyPhotosLink.PostBackUrl = "photos.aspx?profileId=" + userId + "&editing=true";
        }

        /// <summary>
        /// btnUpload_Click event is used to upload images into database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
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
                    IsExists = System.IO.Directory.Exists(Server.MapPath("user_data/" + userId + "/main_photo/"));
                    if (!IsExists)
                        System.IO.Directory.CreateDirectory(Server.MapPath("user_data/" + userId + "/main_photo/"));

                    savePath = "~/final_project/user_data/" + userId + "/main_photo/main" + Path.GetExtension(img.FileName);
                    File.SaveAs(Server.MapPath(savePath));
                    File.InputStream.Close();
                    img.Dispose();
                } else {
                    throw new Exception();
                }
                connection.Open();
                SqlCommand cmd = new SqlCommand("UPDATE ajt.profile_info SET main_photo = @main_photo WHERE user_id = @user_id", connection);
                cmd.Parameters.AddWithValue("@main_photo", savePath);
                cmd.Parameters.Add("@user_id", SqlDbType.Int, 4).Value = userId;
                int newid = Convert.ToInt32(cmd.ExecuteScalar());

                // Display the image from the database
                Image1.ImageUrl = "ImageHandler.ashx?id=" + userId;
            }
            catch
            {
                lblResult.Text = "There was an error";
            }
            finally
            {
                connection.Close();
            }
        }
        /// <summary>
        /// function is used to bind gridview
        /// </summary>
        private void BindGridData()
        {
            // Display the image from the database
            Image1.ImageUrl = "~/ImageHandler.ashx?id=" + userId;
        }

        public void displayFriendRequests()
        {
            bool setToOpen = false;
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
                setToOpen = true;
            }
            SqlCommand cmd = new SqlCommand("SELECT * FROM ajt.friend_requests WHERE user_id = @user_id ORDER BY date_time ASC", connection);
            cmd.Parameters.AddWithValue("@user_id", userId);
            SqlDataReader reader = cmd.ExecuteReader();

            FriendRequestsPanel.Controls.Clear();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    String friend_id = reader["friend_id"].ToString();
                    String first_name = "";
                    String last_name = "";
                    String main_photo = "";

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
                            main_photo = reader2["main_photo"].ToString();
                        }
                        cn2.Close();
                    }

                    // friendRequestContainer
                    HtmlGenericControl friendRequestContainer = new HtmlGenericControl("div");
                    friendRequestContainer.Attributes["class"] = "friendRequestContainer";

                    // friendRequestProfileLink
                    LinkButton friendRequestProfileLink = new LinkButton();
                    friendRequestProfileLink.Attributes["class"] = "friendRequestProfileLink";
                    friendRequestProfileLink.PostBackUrl = "profile.aspx?profileId=" + friend_id;
                    friendRequestProfileLink.Text = first_name + " " + last_name;

                    // clear div
                    HtmlGenericControl clearDiv = new HtmlGenericControl("div");
                    clearDiv.Attributes["class"] = "clear";

                    // friendRequestTextContainer
                    HtmlGenericControl friendRequestTextContainer = new HtmlGenericControl("div");
                    friendRequestTextContainer.Attributes["class"] = "friendRequestTextContainer";
                    // friendRequestText
                    HtmlGenericControl friendRequestText = new HtmlGenericControl("span");
                    friendRequestText.Attributes["class"] = "friendRequestText";
                    friendRequestText.InnerText = " wants to be friends with you";
                    /* Add */
                    friendRequestTextContainer.Controls.Add(friendRequestProfileLink);
                    friendRequestTextContainer.Controls.Add(friendRequestText);

                    // Accept Button
                    Button acceptButton = new Button();
                    acceptButton.Command += new CommandEventHandler(this.AcceptFriendRequest);
                    acceptButton.CommandArgument = friend_id;
                    acceptButton.Attributes["runat"] = "server";
                    acceptButton.Attributes["id"] = "acceptButton";
                    acceptButton.Text = "Accept";

                    // Reject Button
                    Button rejectButton = new Button();
                    rejectButton.Command += new CommandEventHandler(this.RejectFriendRequest);
                    rejectButton.CommandArgument = friend_id;
                    rejectButton.Attributes["runat"] = "server";
                    rejectButton.Attributes["id"] = "rejectButton";
                    rejectButton.Text = "Reject";

                    //friendRequestTextContainer.Controls.Add(new HtmlGenericControl("br"));
                    friendRequestTextContainer.Controls.Add(acceptButton);
                    friendRequestTextContainer.Controls.Add(rejectButton);

                    // commenterImage
                    Image cmmtrImage = new Image();
                    cmmtrImage.Attributes["class"] = "commenterImage";
                    cmmtrImage.ImageUrl = "ImageHandler2.ashx?id=" + friend_id + "&width=38&height=38";

                    /* Add The Rest */
                    friendRequestContainer.Controls.Add(clearDiv);
                    friendRequestContainer.Controls.Add(cmmtrImage);
                    friendRequestContainer.Controls.Add(friendRequestTextContainer);
                    FriendRequestsPanel.Controls.Add(friendRequestContainer);
                }
            }

            if (setToOpen)
                connection.Close();
        }

        public void AcceptFriendRequest(object sender, CommandEventArgs e)
        {
            int friend_id = Convert.ToInt32(e.CommandArgument);
            RemoveFriendRequest(friend_id);
            connection.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO ajt.friends (user_id,friend_id,date_time) VALUES (@user_id,@friend_id,@date_time)", connection);
            cmd.Parameters.AddWithValue("@user_id", userId);
            cmd.Parameters.AddWithValue("@friend_id", friend_id);
            cmd.Parameters.AddWithValue("@date_time", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            SqlCommand cmd2 = new SqlCommand("INSERT INTO ajt.friends (user_id,friend_id,date_time) VALUES (@user_id,@friend_id,@date_time)", connection);
            cmd2.Parameters.AddWithValue("@user_id", friend_id);
            cmd2.Parameters.AddWithValue("@friend_id", userId);
            cmd2.Parameters.AddWithValue("@date_time", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd2.ExecuteNonQuery();
            connection.Close();
            Response.Redirect(Request.Url.ToString(), true);
        }

        public void RejectFriendRequest(object sender, CommandEventArgs e)
        {
            RemoveFriendRequest(Convert.ToInt32(e.CommandArgument));
            Response.Redirect(Request.Url.ToString(), true);
        }

        public void RemoveFriendRequest(int friend_id)
        {
            connection.Open();
            SqlCommand cmd2 = new SqlCommand("DELETE FROM ajt.friend_requests WHERE user_id = @user_id AND friend_id = @friend_id", connection);
            cmd2.Parameters.AddWithValue("@user_id", userId);
            cmd2.Parameters.AddWithValue("@friend_id", friend_id);
            cmd2.ExecuteNonQuery();
            connection.Close();
        }

        public void SearchForFriends_Click(object sender, EventArgs e)
        {
            string[] stringArray = searchForFriendsInput.Text.Split(' ');
            Response.Redirect("findfriends.aspx?profileId=" + userId + "&firstName=" + stringArray[0] + "&lastName=" + stringArray[1]);
        }

    }
}