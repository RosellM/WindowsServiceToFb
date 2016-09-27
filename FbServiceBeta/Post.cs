using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTest.Data.Tables
{
    public class Post
    {
        public String text { get; set; }
        public String @object { get; set; }
        public DateTime date { get; set; }
        public String source { get; set; }
        public String sentiment { get; set; }
        public String useraccount { get; set; }
        public String usernamecomplete { get; set; }
        public String location { get; set; }
        public String latitude { get; set; }
        public String longitude { get; set; }
    }
}
