﻿using System.Threading;
using System.Threading.Tasks;

namespace Brickweave.Cqrs.Services
{
    public interface ICommandProcessor
    {
        Task ProcessCommandsAsync(CancellationToken stoppingToken);
    }
}
