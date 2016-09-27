using Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FbServiceBeta
{
    class ServiceHelper
    {

        private string app_id = "306602469705321";
        private string app_secret = "c0895fd936fe83691d424f06b8c45581";

        public String requestingToken(WebClient client) 
        {
            Console.WriteLine("Autenticando...");
            string oauthUrl = string.Format("https://graph.facebook.com/oauth/access_token?type=client_cred&client_id={0}&client_secret={1}", app_id, app_secret);
            string accessToken = client.DownloadString(oauthUrl).Split('=')[1];
            Console.WriteLine("Token ready!");
            return accessToken;
      
        }

        public dynamic requestingFacebook(FacebookClient facebook_client,string search)
        {
            string search_final = String.Format("{0}/posts?fields=message,comments", search);
            dynamic posts = facebook_client.Get(search_final);
            return posts;
        }

        public dynamic requestingCoincidence(FacebookClient facebook_client, string search)
        {
            //search?type=topic&q=clinton&fields=id%2Cname%2Cpage
            string search_final = 
                String.Format("/search?type=topic&q={0}&fields=id",search);
           
            dynamic posts = facebook_client.Get(search_final);
            return posts;
        }


    }
}
