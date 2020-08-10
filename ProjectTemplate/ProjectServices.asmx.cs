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

        /*Method to access a table in database and return records as jagged array*/
        public string[][] TestRetrieval(string query)
        {
            
            try
            {
                query = query.Replace("%20", " ");
                
                string testQuery = query; 
               
                
                MySqlConnection con = new MySqlConnection(getConString());
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
 

        /*Method to do any Update, insert, delete query*/
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
                //when setting up these queries just take the syntax made from the database and copy it to your javascript
            }
            catch (Exception e)
            {

                return e.Message;
            }
        }

        /*Method similar to test editing but used only for a query to get logon credentials*/
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
    }
}







