using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
namespace FbServiceBeta
{
    
    class DataManagement
    {

        private SqlConnection connection;

        public DataManagement() 
        {
            this.createConectionString();
            this.openConnection();
        }

        private void createConectionString() 
        {

            String conectionString = "Data Source=ROSELL\\ROSEL_94;initial catalog=service_db;persist security info=True;user id=sa;password=rosel0194";
            connection = new SqlConnection(conectionString);
        
        }

        private void openConnection()
        {
            try
            {
                connection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public int insert(dynamic postsList) 
        {
            string insertStmt = "INSERT INTO dbo.facebook(comment, search, coincidence) " +
                    "VALUES(@comment, @search,@coincidence)";
            try {
               return this.insertRange(insertStmt, postsList);              
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
                return -1;
            }
        }

        private int insertRange(String sttnm, dynamic list) 
        {
            if (connection.State == ConnectionState.Closed) 
            {
                connection.Open();
            }        
            SqlCommand command = new SqlCommand(sttnm, connection);
            int listLenght = list.data.Count;
            string message = "";
            String comment = "";
            command.Parameters.Add("@comment", SqlDbType.VarChar);
            command.Parameters.Add("@search", SqlDbType.VarChar);
            command.Parameters.Add("@coincidence", SqlDbType.VarChar);
            try {
                for (int post_iterator = 0; post_iterator < listLenght; post_iterator++)
                {
                    message = list.data[post_iterator].message;

                    command.Parameters["@comment"].Value = message;
                    command.Parameters["@search"].Value = "Comics";
                    command.Parameters["@coincidence"].Value = "MarvelUniverseExtended";
                    command.ExecuteNonQuery();
                    int num_comments = list.data[post_iterator].comments.data.Count;
                    for (int comment_iterador = 0; comment_iterador < num_comments; comment_iterador++)
                    {
                        comment = list.data[post_iterator].comments.data[comment_iterador].message;
                        command.Parameters["@comment"].Value = comment;
                        command.Parameters["@search"].Value = "Comics";
                        command.Parameters["@coincidence"].Value = "MarvelUniverseExtended";
                        command.ExecuteNonQuery();
                    }
                }
                return 1;
            }
            catch(Exception e)
            {
                return -1;
            }
        }


        public void Dispose()
        {
            
        }
    }
}
