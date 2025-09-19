using MongoDB.Bson.Serialization.Attributes;

namespace CQRS.Core.Events
{
    public class EventModel
    {
        [BsonId] // maps to the _id field in MongoDB
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        public DateTime Timestamp { get; set; }

        public Guid AggregateIdentifier { get; set; }

        public string AggregateType { get; set; }

        public int Version { get; set; }

        public string EventType { get; set; }

        public BaseEvent EventData { get; set; }
    }
}
