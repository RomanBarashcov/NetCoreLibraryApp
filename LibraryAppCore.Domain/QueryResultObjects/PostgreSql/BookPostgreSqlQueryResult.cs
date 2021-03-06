﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryAppCore.Domain.QueryResultObjects
{
    public class BookPostgreSqlQueryResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
    }
}
