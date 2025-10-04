using CQRS.Core.Events;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Post.Common.Events
{
    public class CommentRemovedEvent: BaseEvent
    {
        public CommentRemovedEvent() : base(nameof(CommentRemovedEvent))
        {
            
        }

        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid CommentId { get; set; }
    }
}
