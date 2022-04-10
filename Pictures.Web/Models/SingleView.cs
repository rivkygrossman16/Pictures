using Pictures.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pictures.Web.Models
{
    public class SingleView
    {
        public Picture picture { get; set; }
        public bool AlreadyLiked { get; set; }
    }
}
