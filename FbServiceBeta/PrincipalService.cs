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
            int NUM_OF_PAGES = 5;

            ServiceHelper service = new ServiceHelper();
            SM_Facebook sm = new SM_Facebook();
            Console.WriteLine("Iniciando...");
            var client = new WebClient();
            string accessToken = service.requestingToken(client);
            var facebook_client = new FacebookClient(accessToken);
            var PageList = sm.getAll();
            if (PageList.Count > 0)
            {
                List<ServiceTest.Data.Tables.Object> pages = (List <ServiceTest.Data.Tables.Object>)PageList;
                DataManagement dm = new DataManagement();
                foreach (ServiceTest.Data.Tables.Object page in pages)
                {
                    dynamic posts = service.requestingFacebook(facebook_client, page.name);                    
                    List<Post> posts_formated = dm.buildListOfCoincidences(posts);                
                    foreach (Post p in posts_formated)
                    {
                        sm.AddPost(p);
                    }
                    Console.WriteLine("Insert succesfull");
                }
               
                for (int i = 0; i  < NUM_OF_PAGES ; i ++)
                {
                    Console.WriteLine("node "+ i );
                
                    dm.asingValues();
                    dm.clearAuxInfo();
                    foreach (String nextPage in dm.PostChilds )
                    {
                        dynamic posts = service.requestingFacebook(facebook_client, null, nextPage);
                        List<Post> posts_formated = dm.buildListOfCoincidences(posts);
                        foreach (Post p in posts_formated)
                        {
                            sm.AddPost(p);
                            Console.WriteLine("node post ");
                        }
                    }
                    foreach (String nextPage in dm.CommentsChilds)
                    {
                        dynamic posts = service.requestingFacebook(facebook_client, null, nextPage);
                        List<Post> posts_formated = dm.buildListOfCoincidences(posts);
                        foreach (Post p in posts_formated)
                        {
                            sm.AddPost(p);
                            Console.WriteLine("node comments ");
                        }
                    }
                    
                }

            }
            Console.WriteLine("Insertion finished");
            Console.ReadKey();

        }
    }
}
