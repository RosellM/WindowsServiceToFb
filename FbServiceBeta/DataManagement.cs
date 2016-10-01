using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using ServiceTest.Data.Tables;
using ServiceTest.Data;
namespace FbServiceBeta
{

    class DataManagement
    {

        private List<Topic> filters;
        private String coincidence;

        public List<String> aux_PostChilds;
        public List<String> aux_CommentsChilds;

        public List<String> PostChilds;
        public List<String> CommentsChilds;

        public String nextNodePost { get; set; }
        public String nextNodeComent { get; set; }

        public DataManagement()
        {
            aux_PostChilds  = new List<string>();
            aux_CommentsChilds = new List<string>();
        }

        private String getFilter(String post)
        {
            if (filters == null)
            {
                loadFilters();
            }
            else {
                if (filters.Count() == 0)
                {
                    loadFilters();
                }
            }
            var words = post.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
            int wordIteratorSize = words.Count();
            int filterIteratorSize = filters.Count();
            for (int word_iterador = 0; word_iterador < wordIteratorSize; word_iterador++)
            {
                for (int filter_iterador = 0; filter_iterador < filterIteratorSize; filter_iterador++)
                {
                    if (words[word_iterador] == filters[filter_iterador].text)
                    {
                        coincidence = filters[filter_iterador].text;
                        return post;
                    }
                }
            }
            return "-c";
        }

        private void loadFilters()
        {
            SM_Facebook md = new SM_Facebook();
            this.filters = md.getAllTopics();

        }

        public void clearPostInfo()
        {
            PostChilds.Clear();
            CommentsChilds.Clear();
        }

        public void clearAuxInfo()
        {
            aux_PostChilds.Clear();
            aux_CommentsChilds.Clear();
        }

        public void asingValues()
        {
            PostChilds =  new List<string>(aux_PostChilds);
            CommentsChilds = new List<string>( aux_CommentsChilds);
        }

        public List<Post> buildListOfCoincidences(dynamic list) 
        {
            List<Post> posts = new List<Post>();     
            int listLenght = list.data.Count;
            string message = "";
            String comment = "";
            String user = "";           
            DateTime created_time;
            string id_post = "";
            try {
                for (int post_iterator = 0; post_iterator < listLenght; post_iterator++)
                {
                    //post
                    message = list.data[post_iterator].message;
                    id_post = list.data[post_iterator].id;
                    if (!string.IsNullOrEmpty( message ))
                    {
                        if (message == this.getFilter(message))
                        {
                            posts.Add(new Post
                            {
                                text = message,
                                IdPostCatalog = id_post,
                                @object = coincidence,
                                date = DateTime.Now,
                                sentiment = "Neutral",
                                useraccount = "no yet",
                                usernamecomplete = "Admim page",
                                location = "no yet",
                                latitude = "no yet",
                                longitude = "no yet",
                            });
                        }                                
                    }
                    nextNodePost = list.paging.next;
                  
                    //comentarios
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
                                id_post = list.data[post_iterator].comments.data[comment_iterador].id;
                                if (!string.IsNullOrEmpty(comment))
                                {
                                    posts.Add(new Post
                                    {
                                        text = comment,
                                        @object = coincidence,
                                        date = created_time,
                                        IdPostCatalog = id_post,
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
                        nextNodeComent = list.data[post_iterator].comments.paging.next;
                        if (nextNodeComent != null)
                        {
                            aux_CommentsChilds.Add(nextNodeComent);
                        }     
                    }
                  
                }
                if(nextNodePost != null)
                {
                    aux_PostChilds.Add(nextNodePost);
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
