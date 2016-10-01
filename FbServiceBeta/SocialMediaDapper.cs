using ServiceTest.Data.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTest.Data.Interface
{
    public interface SocialMediaDapper
    {
        IDbConnection OpenConnection(String str_conn);
        void CloseConnection();
        int AddPost(Post post);
        int AddFollowers(Followers followers);
        dynamic getSingle(int id);
        dynamic getAll();
    }
}
