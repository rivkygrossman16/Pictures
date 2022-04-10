using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pictures.Data
{
    public class Picture
    {
        public int Id { get; set; }
        public string FilePath { get; set; }
        public string ImageName { get; set; }
        public DateTime Date { get; set; }
        public int NumberOfLikes { get; set; }
    }
}
