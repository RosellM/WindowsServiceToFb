using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using ServiceTest.Data.Tables;

namespace FbServiceBeta
{

    class DataManagement
    {


        private String getFilter(String post)
        {
           var words = post.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
           string[] filters = loadFilters();
           int wordIteratorSize= words.Count();
           int filterIteratorSize = filters.Count();
            for (int word_iterador = 0; word_iterador < wordIteratorSize; word_iterador++)
            {
                for (int filter_iterador = 0; filter_iterador < filterIteratorSize; filter_iterador++)
                {
                    if (words[word_iterador] == filters[ filter_iterador ])
                    {
                        return post;
                    }
                }
            }
            return "-c";
        }

        private String[] loadFilters()
        {
            string[] separators = { "," };
            string text = System.IO.File.ReadAllText(@"C:\Users\Ricardo\Documents\Visual Studio 2013\Projects\WindowsServiceToFb\FbServiceBeta\Filters.data");
            var words = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return words;
        }

        public List<Post> buildListOfCoincidences(dynamic list,string search) 
        {
            List<Post> posts = new List<Post>();         
            int listLenght = list.data.Count;
            string message = "";
            String comment = "";
            String user = "";
            DateTime created_time;
            try {
                for (int post_iterator = 0; post_iterator < listLenght; post_iterator++)
                {
                    message = list.data[post_iterator].message;
                    if (!string.IsNullOrEmpty( message ))
                    {
                        if (message == this.getFilter(message))
                        {
                            posts.Add(new Post
                            {
                                text = message,
                                @object = search,
                                date = DateTime.Now,
                                source = "facebook",
                                sentiment = "Neutral",
                                useraccount = "no yet",
                                usernamecomplete = "Admim page",
                                location = "no yet",
                                latitude = "no yet",
                                longitude = "no yet",
                            });
                        }                                
                    }
                    
                    dynamic validator = list.data[post_iterator].comments;
                    if (validator != null)
                    {
                        int num_comments = list.data[post_iterator].comments.data.Count;
                        for (int comment_iterador = 0; comment_iterador < num_comments; comment_iterador++)
                        {
                            comment = list.data[post_iterator].comments.data[comment_iterador].message;
                            if (comment == this.getFilter(comment))
                            {
                                user = list.data[post_iterator].comments.data[comment_iterador].from.name;
                                created_time = DateTime.Parse(list.data[post_iterator].comments.data[comment_iterador].created_time);
                                if (!string.IsNullOrEmpty(comment))
                                {
                                    posts.Add(new Post
                                    {
                                        text = comment,
                                        @object = search,
                                        date = created_time,
                                        source = "facebook",
                                        sentiment = "Neutral",
                                        useraccount = "no yet",
                                        usernamecomplete = user,
                                        location = "no yet",
                                        latitude = "no yet",
                                        longitude = "no yet",
                                    });
                                }

                            }



                        }
                    }
                  
                }

                return posts;               
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
