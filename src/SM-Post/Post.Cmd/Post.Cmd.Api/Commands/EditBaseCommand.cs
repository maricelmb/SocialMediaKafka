using CQRS_Core.Commands;

namespace Post.Cmd.Api.Commands
{
    public class EditBaseCommand : BaseCommand
    {
        public string Message { get; set; }
    }
}
