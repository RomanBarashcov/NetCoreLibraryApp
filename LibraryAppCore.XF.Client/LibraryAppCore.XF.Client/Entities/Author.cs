using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAppCore.XF.Client.Entities
{
    [Table("Author")]
    public class Author
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public bool Synced { get; set; }

    }
}
