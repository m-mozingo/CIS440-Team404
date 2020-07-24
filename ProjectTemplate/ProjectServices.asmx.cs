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
    }
}
