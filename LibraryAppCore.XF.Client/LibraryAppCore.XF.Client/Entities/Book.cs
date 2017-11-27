﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAppCore.XF.Client.Entities
{
    [Table("Book")]
    public class Book
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        public int Year { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AuthorName { get; set; }
        public int AuthorId { get; set; }
        public bool Synced { get; set; }
    }
}
