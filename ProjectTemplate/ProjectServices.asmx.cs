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
            /*method type is a jagged array or an array of arrays thus the two [][]
             this jagged array has one parameter which is of type string and name query you will see this used on the homepage when the parameters is set in the ajax part
             what the parameter thing in the ajax parts is simply saying take a javascript variable and send it as this methods string query parameter
             thats how you get stuff from javascript to the webmethod
             */
            try
            {
                query = query.Replace("%20", " ");
                //stupid thing that has to happen for whatever reason when a string with a space is sent from the javascript it is encoded as %20 
                //so 'hello world' would become 'hello%20world'
                //this fixes that so its a space again
                string testQuery = query; 
                //sets the testquery to query which I think was already declared in how the professor wrote it
                
                //here's an example of using the getConString method!
                
                MySqlConnection con = new MySqlConnection(getConString());
                con.Open();
                MySqlCommand cmd = new MySqlCommand(testQuery, con);
                MySqlDataReader rdr = cmd.ExecuteReader();
                //database crap saying open a connection, send test query to this connection and execute return results to cmd 
                //then take cmd and read whatever is in it and store that in rdr
                int count = 0;
                while (rdr.Read())
                {
                    count++;
                }
                //some you need to do to get the number of records, no in built function to do this so this counts how many records there are
                rdr.Close();
                //closes rdr to reset it for reasons
                rdr = cmd.ExecuteReader();
                //open it again to read the actual data
                data = new string[count][];
                //this is saying that the number of outer arrays is equal to the number of records found earlier
                int fieldAmount = rdr.FieldCount; 
                //this get total of column count foundin the records
                for (int j = 0; j < count; j++)
                {
                    rdr.Read();
                    data[j] = new string[fieldAmount];
                    for (int i = 0; i < fieldAmount; i++)
                    {
                        data[j][i] = Convert.ToString(rdr.GetValue(i));
                    }
                }
                //inner loop that set a record to an outer array and then fills the inner array with column data
                rdr.Close();
                con.Close();
                //close everything
                return data;
                //returns data which has mysql select data [0][userID, username, etc] [1][userID2, username2, etc] etc
            }
            catch (Exception e)
            {
                string[][] jaggedArray = new string[1][];
                jaggedArray[0] = new string[] { e.Message };
                return jaggedArray;
                //catch mandatory for try
            }
        }
        //this method can take and select statement and return its values as sql data is always a jagged array record and record data so to pull anything just use this


        [WebMethod(EnableSession = true)]
        /////////////////////////////////////////////////////////////////////////
        public string TestEditing(string query)
        {
        //method for sending update, delete, insert etc
        //of type string
            try
            {
                query = query.Replace("%20", " ");
                query = query.Replace("%60", "`");
                //similar to above has to fix certian encoding problems
                string testQuery = query;
                MySqlConnection con = new MySqlConnection(getConString());
                con.Open();
                MySqlCommand cmd = new MySqlCommand(testQuery, con);
                cmd.ExecuteNonQuery();
                //similar open connection and execute if the query passed as parameter from javscript is formatted properly it should work"
                return "Edit Successful";
                //return is simply for testing just console.log the data of the data = msg.d in the ajax part to make sure it went through;
                //you can also pull up the database and check as well
                //when setting up these queries just take the syntax made from the database and copy it to you javascript
            }
            catch (Exception e)
            {

                return e.Message;
            }
        }


        [WebMethod(EnableSession = true)]
        public string[][] Test(string logOnQuery)
        {
            //pretty much the same as testRetrieval 
            //used for the logon Page
            //few changes to check what came back from the database as you arent sure if the pass and user are correct 
            //if statement checks data after its proccessed into the array of arrays
            //if data outer arrays length is less than 1 then no record was returned and no record has that user and pass
            //dont confuse these data variables from the ones in the javascript
            //they both deal with the same information but on different ends
            //data in javascript recieves what is contained in the returned data from the web methods
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

        //I commented these out as I don't know how to set up multiple parameters from the javascript in the ajax call

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







