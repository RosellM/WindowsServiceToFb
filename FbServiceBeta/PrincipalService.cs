using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facebook;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using ServiceTest.Data.Tables;
using ServiceTest.Data;
namespace FbServiceBeta
{
    class PrincipalService
    {
        static void Main(string[] args)
        {

            ServiceHelper service = new ServiceHelper();
            SM_MSSQL sm = new SM_MSSQL();
            Console.WriteLine("Iniciando...");
            var client = new WebClient();
            string accessToken = service.requestingToken(client);
            var facebook_client = new FacebookClient(accessToken);
            dynamic posts = service.requestingFacebook(facebook_client, "AristeguiOnline");
            //dynamic coincidendes = service.requestingCoincidence(facebook_client,"chiapas");
            DataManagement dm = new DataManagement();
            List<Post> posts_formated = dm.buildListOfCoincidences(posts, "cnte test arigtegi");
            foreach (Post p in posts_formated)
            {
                sm.AddPost(p);
            }
            Console.WriteLine("Insert succesfull");
            Console.ReadKey();

        }
    }
}
