using cqrs_core.Commands;

namespace Post.Cmd.Api.Commands
{
    public class EditBaseCommand : BaseCommand
    {
        public string Message { get; set; }
    }
}
