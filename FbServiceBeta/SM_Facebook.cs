using Dapper;
using ServiceTest.Data.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;
using System.Data.SqlClient;
using ServiceTest.Data.Tables;


namespace ServiceTest.Data
{

    public class SM_Facebook
        : SocialMediaDapper
    {
        private IDbConnection conn;
        private Boolean production;
        public String error = "";
        int FACEBOOK = 1;
        public SM_Facebook()
        {
            String conf_str_conn = "local_master";
            try
            {
                String str_conn = ConfigurationManager.ConnectionStrings[conf_str_conn].ConnectionString;
                this.conn = OpenConnection(str_conn);
            }
            catch (Exception e)
            {

            }
        }

        public IDbConnection OpenConnection(String str_conn)
        {
            SqlConnection temp_conn = new SqlConnection(str_conn);
            if (temp_conn != null && temp_conn.State == ConnectionState.Closed)
            {
                try
                {
                    temp_conn.Open();
                }
                catch (Exception ex)
                {
                    temp_conn = null;
                    error += ex.ToString();
                }
            }
            else
            {
                error += "Connection.State";
            }
            return temp_conn;
        }

        public void CloseConnection()
        {
            if (this.conn != null && this.conn.State == ConnectionState.Open)
            {
                this.conn.Dispose();
            }
            this.conn = null;
        }

        public int AddPost(Post post)
        {
            int id = -1;
            if (this.conn != null && this.conn.State == ConnectionState.Open)
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@IdPostCatalog", post.IdPostCatalog);
                param.Add("@text", post.text);            
                param.Add("@object", post.@object);
                param.Add("@date", post.date);
                param.Add("@IdCatalog", FACEBOOK);
                param.Add("@sentiment", post.sentiment);
                param.Add("@useraccount", post.useraccount);
                param.Add("@usernamecomplete", post.usernamecomplete);
                param.Add("@location", post.location);
                param.Add("@latitude", post.latitude);
                param.Add("@longitude", post.longitude);
                param.Add("@IdPost", dbType: DbType.Int32, direction: ParameterDirection.Output);
                try
                {
                    this.conn.Execute("addPostProc", param, commandType: CommandType.StoredProcedure);
                    id = param.Get<Int32>("IdPost");
                    if (id <= 0)
                    {
                        id = -1;
                        
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return id;
        }

        public int AddFollowers(Followers followers)
        {
            int id = -1;
            if (this.conn != null && this.conn.State == ConnectionState.Open)
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@object", followers.@object);
                param.Add("@source", followers.source);
                param.Add("@total", followers.total);
                param.Add("@date", followers.date);
                param.Add("@id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                try
                {
                    this.conn.Execute("addFollowersCountProc", param, commandType: CommandType.StoredProcedure);
                    id = param.Get<Int32>("id");
                    if (id <= 0)
                    {
                        id = -1;
                    }
                }
                catch (Exception ex)
                {
                    error += ex.ToString();
                }
            }
            return id;
        }

        public dynamic getSingle(int id)
        {
            throw new NotImplementedException();
        }

        public dynamic getAll()
        {
           
            try {
                if (this.conn != null && this.conn.State == ConnectionState.Open)
                {
                    return conn.Query<ServiceTest.Data.Tables.Object>("Select * From filter where IdCatalog=" + FACEBOOK).ToList();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public dynamic getAllTopics()
        {


            try
            {
                if (this.conn != null && this.conn.State == ConnectionState.Open)
                {
                    return conn.Query<ServiceTest.Data.Tables.Topic>("Select * From topic").ToList();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }
    }
}
