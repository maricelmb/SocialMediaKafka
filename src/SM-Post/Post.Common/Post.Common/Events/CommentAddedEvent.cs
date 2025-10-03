using CQRS.Core.Events;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Post.Common.Events
{
    public class CommentAddedEvent : BaseEvent
    {
        public CommentAddedEvent() : base(nameof(CommentAddedEvent))
        {
            
        }

        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid CommentId { get; set; }

        public string Comment { get; set; }

        public string Username { get; set; }
        public DateTime CommentDate { get; set; }
    }
}
