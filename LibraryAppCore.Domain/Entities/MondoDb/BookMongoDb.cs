using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LibraryAppCore.Domain.Entities.MondoDb
{
    public class BookMongoDb
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string AuthorId { get; set; }
        public AuthorMongoDb Author { get; set; }
    }
}
