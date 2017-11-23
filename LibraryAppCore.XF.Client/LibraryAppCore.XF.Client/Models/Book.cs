using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAppCore.XF.Client.Models
{
    public class Book
    {
        public string Id { get; set; }
        public int Year { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AuthorName { get; set; }
        public string AuthorId { get; set; }
    }
}
