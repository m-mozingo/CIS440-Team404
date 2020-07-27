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

        [WebMethod(EnableSession = true)]
        /////////////////////////////////////////////////////////////////////////
        public string[][] TestRetrieval()
        { 
            string[][] data;
            try
			{
				string testQuery = "select * from Scenarios where scenarioID='S0002'";

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
                for(int j =0; j<count; j++)
                {
                    rdr.Read();
                    data[j] = new string[fieldAmount];
                    for(int i = 0; i < fieldAmount; i++)
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
                string[][] jaggedArray = new string[3][];
                jaggedArray[0] = new string[] { e.Message };
                return jaggedArray;
            }
            
        }
        [WebMethod(EnableSession = true)]
        /////////////////////////////////////////////////////////////////////////
        public string TestEditing()
        {
            
            try
            {
                string testQuery = "UPDATE `summer2020group1`.`Scenarios` SET `scenarioText` = '1st test', `scenarioHeading` = '1st heading' WHERE (`scenarioID` = 's0001');";
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
        public void accountValidate(string Username, string Password)
        {

            bool repeat = false;

            string sqlQuery = "SELECT * FROM `summer2020group1`.`UserAccounts`";

            MySqlConnection sqlConnection = new MySqlConnection(getConString());

            sqlConnection.Open();
            MySqlCommand sqlCommand = new MySqlCommand(sqlQuery, sqlConnection);
        }

    }
}
