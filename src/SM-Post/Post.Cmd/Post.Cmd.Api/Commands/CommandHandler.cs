
using CQRS.Core.Handlers;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Api.Commands
{
    public class CommandHandler : ICommandHandler
    {
        private readonly IEventSourcingHandler<PostAggregate> _eventSourcingHandler;

        public CommandHandler(IEventSourcingHandler<PostAggregate> eventSourcingHandler)
        {
            _eventSourcingHandler = eventSourcingHandler;
        }
        public async Task HandleAsync(NewPostCommand command)
        {
            var aggregate = new PostAggregate(command.Id, command.Author, command.Message);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(EditMessageCommand command)
        {
            var aggregate = _eventSourcingHandler.GetByIdAsync(command.Id).Result;
            aggregate.EditMessage(command.Message);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(LikePostCommand command)
        {
            var aggregate = _eventSourcingHandler.GetByIdAsync(command.Id).Result;
            aggregate.LikePost();

            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(AddCommentCommand command)
        {
            var aggregate = _eventSourcingHandler.GetByIdAsync(command.Id).Result;
            aggregate.AddComment(command.Comment, command.Username);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(RemoveCommentCommand command)
        {
            var aggregate = _eventSourcingHandler.GetByIdAsync(command.Id).Result;
            aggregate.RemoveComment(command.CommentId, command.Username);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(EditCommentCommand command)
        {
            var aggregate = _eventSourcingHandler.GetByIdAsync(command.Id).Result;
            aggregate.EditComment(command.CommentId, command.Comment, command.Username);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(DeletePostCommand command)
        {
            var aggregate = _eventSourcingHandler.GetByIdAsync(command.Id).Result;
            aggregate.DeletePost(command.Username);

            await _eventSourcingHandler.SaveAsync(aggregate);
        }
    }
}
