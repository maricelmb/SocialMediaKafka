using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using Post.Cmd.Domain.Aggregates;
using Post.Cmd.Infrastructure.Stores;

namespace Post.Cmd.Infrastructure.Handlers
{
    public class EventSourcingHandler : IEventSourcingHandler<PostAggregate>
    {
        private readonly EventStore _eventStore;

        public EventSourcingHandler(EventStore eventStore)
        {
            _eventStore = eventStore;
        }
        public async Task<PostAggregate> GetByIdAsync(Guid id)
        {
            var aggregate = new PostAggregate();
            var events = await _eventStore.GetEventsAsync(id);

            if (events == null || !events.Any()) return aggregate;
            
            aggregate.ReplayEvents(events);
            aggregate.Version = events.Max(e => e.Version);

            return aggregate;
        }

        public async Task SaveAsync(AggregateRoot aggregate)
        {
            await _eventStore.SaveEventsAsync(aggregate.Id, aggregate.GetUncommittedChanges().ToList(), aggregate.Version);
            aggregate.MarkChangesAsCommitted();
        }
    }
}
