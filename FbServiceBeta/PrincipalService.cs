using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facebook;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
namespace FbServiceBeta
{
    class PrincipalService
    {
        static void Main(string[] args)
        {

            ServiceHelper service = new ServiceHelper();
            var client = new WebClient();
            string accessToken = service.requestingToken(client);
            var facebook_client = new FacebookClient(accessToken);
            dynamic posts = service.requestingFacebook(facebook_client, "BorrachosVIP");
            int PostLenght = posts.data.Count;
            String message = "its empty";
            for (int post_iterator = 0; post_iterator < PostLenght; post_iterator++)
            {
                String comment = "its empty";

                message = posts.data[post_iterator].message;
                Console.WriteLine(" # publicacion " + message);

                int num_comments = posts.data[post_iterator].comments.data.Count;
                for (int comment_iterador = 0; comment_iterador < num_comments; comment_iterador++) 
                {
                    comment = posts.data[post_iterator].comments.data[comment_iterador].message;
                    Console.WriteLine(" - Comentario "  + comment);
                }
              
            }
            Console.ReadKey();

        }
    }
}
