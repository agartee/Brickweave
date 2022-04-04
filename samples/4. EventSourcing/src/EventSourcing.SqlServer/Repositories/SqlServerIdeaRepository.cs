using System.Threading.Tasks;
using Brickweave.EventStore.Factories;
using Brickweave.EventStore.SqlServer;
using Brickweave.Messaging.SqlServer.Extensions;
using Brickweave.Serialization;
using EventSourcing.Domain.Common.Exceptions;
using EventSourcing.Domain.Ideas.Models;
using EventSourcing.Domain.Ideas.Services;
using EventSourcing.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventSourcing.SqlServer.Repositories
{
    public class SqlServerIdeaRepository : AggregateRepository<Idea>, IIdeaRepository
    {
        private EventSourcingDbContext _dbContext;
        private readonly IDocumentSerializer _serializer;

        public SqlServerIdeaRepository(EventSourcingDbContext dbContext, IDocumentSerializer serializer,
            IAggregateFactory aggregateFactory)
            : base(serializer, aggregateFactory)
        {
            _dbContext = dbContext;
            _serializer = serializer;
        }

        public async Task<Idea> DemandIdeaAsync(IdeaId id)
        {
            var idea = await CreateFromEventsAsync(_dbContext.Events, id.Value);

            if (idea == null)
                throw new EntityNotFoundException(id, nameof(Idea));

            return idea;
        }

        public async Task SaveIdeaAsync(Idea idea)
        {
            AddUncommittedEvents(_dbContext.Events, idea, idea.Id.Value);

            idea.GetDomainEvents()
                .Enqueue(_dbContext.MessageOutbox, _serializer);

            await HandleIdeaProjection(idea);

            await _dbContext.SaveChangesAsync();

            idea.ClearUncommittedEvents();
            idea.ClearDomainEvents();
        }

        private async Task HandleIdeaProjection(Idea idea)
        {
            var existingData = await _dbContext.Ideas
                .FirstOrDefaultAsync(a => a.Id == idea.Id.Value);

            if (existingData == null)
                CreateIdeaProjection(idea);
            else if (!idea.IsActive)
                DeleteIdeaProjection(existingData);
            else
                UpdateIdeaProjection(existingData, idea);
        }

        private void CreateIdeaProjection(Idea idea)
        {
            _dbContext.Ideas.Add(new IdeaData
            {
                Id = idea.Id.Value,
                Name = idea.Name.Value
            });
        }

        private void DeleteIdeaProjection(IdeaData data)
        {
            _dbContext.Ideas.Remove(data);
        }

        private void UpdateIdeaProjection(IdeaData existingData, Idea idea)
        {
            existingData.Name = idea.Name.Value;
        }
    }
}
