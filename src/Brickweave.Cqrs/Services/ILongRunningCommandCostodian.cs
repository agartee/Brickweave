﻿using System.Threading.Tasks;

namespace Brickweave.Cqrs.Services
{
    public interface ILongRunningCommandCostodian
    {
        Task AttendAsync();
    }
}
