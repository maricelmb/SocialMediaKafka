﻿using cqrs_core.Commands;

namespace Post.Cmd.Api.Commands
{
    public class DeletePostCommand: BaseCommand
    {
        public string Username { get; set; }
    }
}
