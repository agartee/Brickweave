using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AdvancedCqrs.Domain.Things.Models;
using AdvancedCqrs.Domain.Things.Services;

namespace AdvancedCqrs.SqlServer.Repositories
{
    public class SqlServerThingRepository : IThingRepository
    {
        public async Task SaveThingAsync(Thing thing)
        {
            throw new NotImplementedException();
        }
    }
}
