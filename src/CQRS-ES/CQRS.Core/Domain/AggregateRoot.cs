using CQRS.Core.Events;

namespace CQRS.Core.Domain
{  
    // An aggregate root is an entity that serves as the entry point for accessing and
    // manipulating a group of related entities (the aggregate)
    // Tracks the changes made to the aggregate by storing the events that represent those changes
    public abstract class AggregateRoot
    {
        protected Guid _id;
        private readonly List<BaseEvent> _changes = new();

        public Guid Id 
        { 
            get 
            { 
                return _id; 
            } 
        }

        public int Version { get; set; } = -1;

        public IEnumerable<BaseEvent> GetUncommittedChanges()
        {
            return _changes;
        }

        public void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }

        private void ApplyChange(BaseEvent @event, bool isNew)
        {
            var method = this.GetType().GetMethod("Apply", new Type[] { @event.GetType() });

            if (method == null)
            { 
                throw new ArgumentNullException(nameof(method), 
                    $"The Apply method was not found in the {this.GetType().Name} aggregate for {@event.GetType().Name} event");
            }

            method.Invoke(this, new object[] { @event });

            if(isNew)
            {
                _changes.Add(@event);
            }
        }

        protected void RaiseEvent(BaseEvent @event)
        {
            ApplyChange(@event, true);
        }

        public void ReplayEvents(IEnumerable<BaseEvent> events)
        {
            foreach (var @event in events)
            {
                ApplyChange(@event, false);
            }
        }
    }
}
