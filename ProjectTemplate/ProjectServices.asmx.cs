using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace ProjectTemplate
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]

    public class ProjectServices : System.Web.Services.WebService
    {
        ////////////////////////////////////////////////////////////////////////
        ///replace the values of these variables with your database credentials
        ////////////////////////////////////////////////////////////////////////
        private string dbID = "summer2020group1";
        private string dbPass = "!!Group1";
        private string dbName = "summer2020group1";
        ////////////////////////////////////////////////////////////////////////

        ////////////////////////////////////////////////////////////////////////
        ///call this method anywhere that you need the connection string!
        ////////////////////////////////////////////////////////////////////////
        private string getConString()
        {
            return "SERVER=107.180.1.16; PORT=3306; DATABASE=" + dbName + "; UID=" + dbID + "; PASSWORD=" + dbPass;

        }
        ////////////////////////////////////////////////////////////////////////



        /////////////////////////////////////////////////////////////////////////
        //don't forget to include this decoration above each method that you want
        //to be exposed as a web service!

        string[][] data;
        [WebMethod(EnableSession = true)]
        /////////////////////////////////////////////////////////////////////////
        ///

        public string[][] TestRetrieval(string query)
        {

            try
            {
                query = query.Replace("%20", " ");
                string testQuery = query;
                ////////////////////////////////////////////////////////////////////////
                ///here's an example of using the getConString method!
                ////////////////////////////////////////////////////////////////////////
                MySqlConnection con = new MySqlConnection(getConString());
                ////////////////////////////////////////////////////////////////////////
                con.Open();
                MySqlCommand cmd = new MySqlCommand(testQuery, con);
                MySqlDataReader rdr = cmd.ExecuteReader();
                int count = 0;
                while (rdr.Read())
                {
                    count++;
                }
                rdr.Close();
                rdr = cmd.ExecuteReader();
                data = new string[count][];
                int fieldAmount = rdr.FieldCount;
                for (int j = 0; j < count; j++)
                {
                    rdr.Read();
                    data[j] = new string[fieldAmount];
                    for (int i = 0; i < fieldAmount; i++)
                    {
                        data[j][i] = Convert.ToString(rdr.GetValue(i));
                    }
                }
                rdr.Close();
                con.Close();

                return data;

            }
            catch (Exception e)
            {
                string[][] jaggedArray = new string[1][];
                jaggedArray[0] = new string[] { e.Message };
                return jaggedArray;
            }
        }
        [WebMethod(EnableSession = true)]
        /////////////////////////////////////////////////////////////////////////
        public string TestEditing(string query)
        {
            
            try
            {
                query = query.Replace("%20", " ");
                query = query.Replace("%60", "`");
                string testQuery = query;
                MySqlConnection con = new MySqlConnection(getConString());

                con.Open();
                MySqlCommand cmd = new MySqlCommand(testQuery, con);
                cmd.ExecuteNonQuery();

                return "Edit Successful";
            }
            catch (Exception e)
            {

                return e.Message;
            }
        }

        [WebMethod(EnableSession = true)]
        public string[][] Test(string logOnQuery)
        {

            try
            {
                logOnQuery = logOnQuery.Replace("%20", " ");
                string testQuery = logOnQuery;
                ////////////////////////////////////////////////////////////////////////
                ///here's an example of using the getConString method!
                ////////////////////////////////////////////////////////////////////////
                MySqlConnection con = new MySqlConnection(getConString());
                ////////////////////////////////////////////////////////////////////////
                con.Open();
                MySqlCommand cmd = new MySqlCommand(testQuery, con);
                MySqlDataReader rdr = cmd.ExecuteReader();
                int count = 0;
                while (rdr.Read())
                {
                    count++;
                }
                rdr.Close();
                rdr = cmd.ExecuteReader();
                data = new string[count][];
                int fieldAmount = rdr.FieldCount;
                for (int j = 0; j < count; j++)
                {
                    rdr.Read();
                    data[j] = new string[fieldAmount];
                    for (int i = 0; i < fieldAmount; i++)
                    {
                        data[j][i] = Convert.ToString(rdr.GetValue(i));
                    }
                }
                rdr.Close();
                con.Close();
                if (data.Length > 0)
                {
                    return data;
                }
                else
                {
                    string[][] jaggedArray = new string[1][];
                    jaggedArray[0] = new string[] {"false"};
                    return jaggedArray;
                }

            }
            catch (Exception e)
            {
                string[][] jaggedArray = new string[1][];
                jaggedArray[0] = new string[] { e.Message };
                return jaggedArray;
            }
        }
        /*
        //EXAMPLE OF AN INSERT QUERY WITH PARAMS PASSED IN.  BONUS GETTING THE INSERTED ID FROM THE DB!
        [WebMethod]
        public void CreateAccount(string FirstName, string LastName, string Email, string Username, string Password, string Department)
        {
            //string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["myDB"].ConnectionString;
            //the only thing fancy about this query is SELECT LAST_INSERT_ID() at the end.  All that
            //does is tell mySql server to return the primary key of the last inserted row.
            string sqlQuery = "INSERT INTO `summer2020group1`.`UserAccounts` (`FirstName`, `LastName`, `Email`, `Username`, `Password`, `Department`) " +
                "VALUES (@FirstName, @LastName, @Email, @Username, @Password, @Department);";


            // "INSERT into UserAccounts (FirstName, LastName, Email, Username, Password, Department) " +
            // "values(@idValue, @passValue, @fnameValue, @lnameValue, @emailValue); SELECT LAST_INSERT_ID();";

            MySqlConnection sqlConnection = new MySqlConnection(getConString());

            sqlConnection.Open();
            MySqlCommand sqlCommand = new MySqlCommand(sqlQuery, sqlConnection);

            sqlCommand.Parameters.AddWithValue("@FirstName", HttpUtility.UrlDecode(FirstName));
            sqlCommand.Parameters.AddWithValue("@LastName", HttpUtility.UrlDecode(LastName));
            sqlCommand.Parameters.AddWithValue("@Email", HttpUtility.UrlDecode(Email));
            sqlCommand.Parameters.AddWithValue("@Username", HttpUtility.UrlDecode(Username));
            sqlCommand.Parameters.AddWithValue("@Password", HttpUtility.UrlDecode(Password));
            sqlCommand.Parameters.AddWithValue("@Department", HttpUtility.UrlDecode(Department));


            //this time, we're not using a data adapter to fill a data table.  We're just
            //opening the connection, telling our command to "executescalar" which says basically
            //execute the query and just hand me back the number the query returns (the ID, remember?).
            //don't forget to close the connection!

            //we're using a try/catch so that if the query errors out we can handle it gracefully
            //by closing the connection and moving on
            try
            {
                int accountID = Convert.ToInt32(sqlCommand.ExecuteScalar());
                //here, you could use this accountID for additional queries regarding
                //the requested account.  Really this is just an example to show you
                //a query where you get the primary key of the inserted row back from
                //the database!
            }
            catch (Exception e)
            {
            }
            sqlConnection.Close();
        }

        [WebMethod]
        public bool AccountValidate(string Username)
        {

            bool repeat = false;


            string sqlQuery = "SELECT COUNT(Username) FROM `summer2020group1`.`UserAccounts` WHERE Username=@Username";

            MySqlConnection sqlConnection = new MySqlConnection(getConString());

            sqlConnection.Open();
            MySqlCommand sqlCommand = new MySqlCommand(sqlQuery, sqlConnection);

            sqlCommand.Parameters.AddWithValue("@Username", HttpUtility.UrlDecode(Username));

            MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);

            DataTable sqlDt = new DataTable();

            sqlDa.Fill(sqlDt);

            if (sqlDt.Rows.Count > 0)
            {
                repeat = false;
            }

            return repeat;

        }
        [WebMethod]
        public bool LogOn(string[] userPass)
        {   

            string Username = userPass[0];
            string Password = userPass[1];
            return true;
            
            //LOGIC: pass the parameters into the database to see if an account
            //with these credentials exist.  If it does, then return true.  If
            //it doesn't, then return false

            //we return this flag to tell them if they logged in or not
            bool success = false;

            //our connection string comes from our web.config file like we talked about earlier
            MySqlConnection sqlConnection = new MySqlConnection(getConString());
            //here's our query.  A basic select with nothing fancy.  Note the parameters that begin with @
            string sqlSelect = "SELECT id FROM accounts WHERE userid=@Username and pass=@Password";


            //set up our command object to use our connection, and our query
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);

            //tell our command to replace the @parameters with real values
            //we decode them because they came to us via the web so they were encoded
            //for transmission (funky characters escaped, mostly)
            sqlCommand.Parameters.AddWithValue("@idValue", HttpUtility.UrlDecode(Username));
            sqlCommand.Parameters.AddWithValue("@passValue", HttpUtility.UrlDecode(Password));

            //a data adapter acts like a bridge between our command object and 
            //the data we are trying to get back and put in a table object
            MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);
            //here's the table we want to fill with the results from our query
            DataTable sqlDt = new DataTable();
            //here we go filling it!
            sqlDa.Fill(sqlDt);
            //check to see if any rows were returned.  If they were, it means it's 
            //a legit account
            if (sqlDt.Rows.Count > 0)
            {
                //flip our flag to true so we return a value that lets them know they're logged in
                success = true;
            }
            //return the result!
            return success;
           
        }

        //EXAMPLE OF AN UPDATE QUERY
        [WebMethod]
        public void ChangePassword(string Password, string ID)
        {
            MySqlConnection sqlConnection = new MySqlConnection(getConString());
            //this is a simple update, with parameters to pass in values
            string sqlSelect = "UPDATE UserAccounts SET Password=@Password WHERE id=@ID";

            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);

            sqlCommand.Parameters.AddWithValue("@Password", HttpUtility.UrlDecode(Password));
            sqlCommand.Parameters.AddWithValue("@ID", HttpUtility.UrlDecode(ID));

            sqlConnection.Open();
            try
            {
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
            }
            sqlConnection.Close();
        }
        */
    }
}







