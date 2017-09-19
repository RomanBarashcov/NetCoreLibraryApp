using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryAppCore.Domain.Entities.MondoDb
{
    public class AuthorMongoDb
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public ICollection<BookMongoDb> books { get; set; }
        public AuthorMongoDb()
        {
            books = new List<BookMongoDb>();
        }
    }
}
