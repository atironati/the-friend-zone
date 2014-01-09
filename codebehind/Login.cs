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
    public partial class Login : edu.neu.ccis.ajt.BasePage
    {

        public void Page_Load(object sender, EventArgs e)
        {
            for (int i = 2012; i > 1900; i--)
            {
                birthyearDropdown.Items.Add(new ListItem(i.ToString()));
            }
            //birthyearDropdown.SelectedIndex = birthyearDropdown.Items.IndexOf(new ListItem("2012"));
            for (int i = 1; i < 32; i++)
            {
                birthdayDropdown.Items.Add(new ListItem(i.ToString()));
            }
            //birthdayDropdown.SelectedIndex = birthdayDropdown.Items.IndexOf(new ListItem("1"));
            for (int i = 1; i < 13; i++)
            {
                birthmonthDropdown.Items.Add(new ListItem(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i).ToString()));
            }
            //birthmonthDropdown.SelectedIndex = birthmonthDropdown.Items.IndexOf(new ListItem("January"));
        }

        public void signInClick(object sender, EventArgs e)
        {
            if (usernameInput.Text == "" || passwordInput.Text == "")
            {
                createAccountErrorBoxText.Text = "";
                loginErrorBoxText.Text = "It seems that you forgot to enter your username or password!";
            }
            else
            {
                connection.Open();
                string selectSql = "SELECT username, password, salt FROM ajt.account_info WHERE username COLLATE Latin1_General_CS_AS = @username";
                SqlCommand selectCmd = new SqlCommand(selectSql, connection);
                selectCmd.Parameters.Add("@username", SqlDbType.VarChar, 50, "username");
                selectCmd.Parameters["@username"].Value = usernameInput.Text;

                SqlDataReader reader = selectCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    String username = reader["username"].ToString();
                    String password = reader["password"].ToString();
                    int _salt = Int32.Parse(reader["salt"].ToString());

                    if (usernameInput.Text == username && ComputeSaltedHash(passwordInput.Text, _salt) == password)
                    {
                        // Account is valid
                        placeCookie(username, password, _salt);
                        // Redirect to the desired page
                        Response.Redirect("dashboard.aspx");
                    }
                    else
                    {
                        createAccountErrorBoxText.Text = "";
                        loginErrorBoxText.Text = "Sorry, the password you entered doesn't match our records";
                    }
                }
                else
                {
                    createAccountErrorBoxText.Text = "";
                    loginErrorBoxText.Text = "Sorry, no records were found for that username";
                }
                reader.Close();
                connection.Close();
            }
        }

        public void placeCookie(String username, String password, int _salt)
        {
            //Check if the browser support cookies 
            if ((Request.Browser.Cookies))
            {
                //Check if the cookie exists on user's machine 
                if ((Request.Cookies["FRIENDZONELOGIN"] == null))
                {
                    //Create a cookie with that will expire in 30 days 
                    HttpCookie fzl = new HttpCookie("FRIENDZONELOGIN");
                    fzl.Expires = DateTime.Now.AddDays(30);
                    fzl["USERNAME"] = username;
                    fzl["PASSWORD"] = password;
                    fzl["SALT_THE_SNAIL"] = _salt.ToString();
                    Response.Cookies.Add(fzl);
                }
                //If the cookie already exists then write the username and password on the cookie 
                else
                {
                    Response.Cookies["FRIENDZONELOGIN"]["USERNAME"] = username;
                    Response.Cookies["FRIENDZONELOGIN"]["PASSWORD"] = password;
                    Response.Cookies["FRIENDZONELOGIN"]["SALT_THE_SNAIL"] = _salt.ToString();
                }
            }
        }

        public void createAccountClick(object sender, EventArgs e)
        {
            if (firstNameInput.Text == "" || lastNameInput.Text == "" || newUsernameInput.Text == "" || newPasswordInput.Text == "")
            {
                loginErrorBoxText.Text = "";
                createAccountErrorBoxText.Text = "Sorry, but all fields are required to create an account!";
            }
            else
            {
                connection.Open();
                string selectSql = "SELECT username, password, salt FROM ajt.account_info WHERE username COLLATE Latin1_General_CS_AS = @username";
                SqlCommand selectCmd = new SqlCommand(selectSql, connection);
                selectCmd.Parameters.Add("@username", SqlDbType.VarChar, 50, "username");
                selectCmd.Parameters["@username"].Value = newUsernameInput.Text;

                SqlDataReader reader = selectCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Close();
                    loginErrorBoxText.Text = "";
                    createAccountErrorBoxText.Text = "Sorry, that username is already taken";
                }
                else
                {
                    reader.Close();
                    insertNewAccount();

                    // Redirect to the desired page
                    Response.Redirect("dashboard.aspx");
                }
                connection.Close();
            }
        }

        public void insertNewAccount()
        {
            // Insert into account_info table
            string insertSql = "INSERT INTO ajt.account_info (username,password,salt) VALUES (@username,@password,@salt)";
            SqlCommand insertCmd = new SqlCommand(insertSql, connection);

            insertCmd.Parameters.Add("@username", SqlDbType.VarChar, 50, "username");
            String newUsername = newUsernameInput.Text;
            insertCmd.Parameters["@username"].Value = newUsername;

            int _salt = CreateRandomSalt();

            insertCmd.Parameters.Add("@password", SqlDbType.VarChar, 50, "username");
            String newPassword = ComputeSaltedHash(newPasswordInput.Text, _salt);
            insertCmd.Parameters["@password"].Value = newPassword;

            insertCmd.Parameters.Add("@salt", SqlDbType.VarChar, 25, "username");
            insertCmd.Parameters["@salt"].Value = _salt;

            insertCmd.ExecuteNonQuery();

            // Retrieve id of recently inserted record
            string findIdSql = "SELECT user_id FROM ajt.account_info WHERE username COLLATE Latin1_General_CS_AS = @username";
            SqlCommand findIdCmd = new SqlCommand(findIdSql, connection);
            findIdCmd.Parameters.Add("@username", SqlDbType.VarChar, 50, "username");
            findIdCmd.Parameters["@username"].Value = newUsernameInput.Text;
            SqlDataReader idReader = findIdCmd.ExecuteReader();
            idReader.Read();
            int insertedID = Int32.Parse(idReader["user_id"].ToString());
            idReader.Close();

            // Insert into profile_info table
            string insertProfileSql = @"INSERT INTO ajt.profile_info (user_id,first_name,last_name,birth_month,birth_day,birth_year) 
                                    VALUES (@user_id,@first_name,@last_name,@birth_month,@birth_day,@birth_year)";
            SqlCommand insertProfileCmd = new SqlCommand(insertProfileSql, connection);

            insertProfileCmd.Parameters.Add("@user_id", SqlDbType.Int, 4, "user_id");
            insertProfileCmd.Parameters["@user_id"].Value = insertedID;
            insertProfileCmd.Parameters.Add("@first_name", SqlDbType.VarChar, 50, "first_name");
            insertProfileCmd.Parameters["@first_name"].Value = firstNameInput.Text;
            insertProfileCmd.Parameters.Add("@last_name", SqlDbType.VarChar, 50, "last_name");
            insertProfileCmd.Parameters["@last_name"].Value = lastNameInput.Text;
            insertProfileCmd.Parameters.Add("@birth_month", SqlDbType.VarChar, 25, "birth_month");
            insertProfileCmd.Parameters["@birth_month"].Value = birthmonthDropdown.SelectedValue;
            insertProfileCmd.Parameters.Add("@birth_day", SqlDbType.Int, 4, "birth_day");
            insertProfileCmd.Parameters["@birth_day"].Value = birthdayDropdown.SelectedValue;
            insertProfileCmd.Parameters.Add("@birth_year", SqlDbType.Int, 4, "birth_year");
            insertProfileCmd.Parameters["@birth_year"].Value = birthyearDropdown.SelectedValue;

            insertProfileCmd.ExecuteNonQuery();

            placeCookie(newUsername, newPassword, _salt);
        }

        public static int CreateRandomSalt()
        {
            Byte[] _saltBytes = new Byte[4];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(_saltBytes);

            return ((((int)_saltBytes[0]) << 24) + (((int)_saltBytes[1]) << 16) +
              (((int)_saltBytes[2]) << 8) + ((int)_saltBytes[3]));
        }

        public string ComputeSaltedHash(String password, int _salt)
        {
            // Create Byte array of password string
            ASCIIEncoding encoder = new ASCIIEncoding();
            Byte[] _secretBytes = encoder.GetBytes(password);

            // Create a new salt
            Byte[] _saltBytes = new Byte[4];
            _saltBytes[0] = (byte)(_salt >> 24);
            _saltBytes[1] = (byte)(_salt >> 16);
            _saltBytes[2] = (byte)(_salt >> 8);
            _saltBytes[3] = (byte)(_salt);

            // append the two arrays
            Byte[] toHash = new Byte[_secretBytes.Length + _saltBytes.Length];
            Array.Copy(_secretBytes, 0, toHash, 0, _secretBytes.Length);
            Array.Copy(_saltBytes, 0, toHash, _secretBytes.Length, _saltBytes.Length);

            SHA1 sha1 = SHA1.Create();
            Byte[] computedHash = sha1.ComputeHash(toHash);

            return encoder.GetString(computedHash);
        }

    }
}