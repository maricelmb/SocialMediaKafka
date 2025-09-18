using CQRS.Core.Commands;
using CQRS.Core.Infrastructure;

namespace Post.Cmd.Infrastructure.Dispatchers
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly Dictionary<Type, Func<BaseCommand, Task>> _handlers = new();

        public void RegisterHandler<T>(Func<T, Task> handler) where T : BaseCommand
        {
            if (_handlers.ContainsKey(typeof(T)))
            {
                throw new IndexOutOfRangeException($"Handler for command {typeof(T).Name} is already registered");
            }

            _handlers.Add(typeof(T), x => handler((T)x));
        }

        public async Task SendAsync<T>(BaseCommand command)
        {
            if (_handlers.TryGetValue(typeof(T), out Func<BaseCommand, Task>? handler)) // Add nullable annotation
            {
                if (handler != null) // Explicit null check
                {
                    await handler(command);
                }
                else
                {
                    throw new ArgumentNullException(nameof(handler), "No command handler is registered");
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(handler), "No command handler is registered");
            }
        }
    }
}
