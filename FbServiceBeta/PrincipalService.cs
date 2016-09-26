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
            dynamic posts = service.requestingFacebook(facebook_client, "MarvelUniverseExtended");
            DataManagement db = new DataManagement();        
            if (db.insert(posts) !=-1)
            {
               Console.WriteLine("Insercion exitosa!");
            }
            
            
            Console.ReadKey();

        }
    }
}
