namespace Brickweave.EventStore.Tests.Models
{
    public class ChildCommentSet : IChildEvent
    {
        public ChildCommentSet(int id, string comment)
        {
            Id = id;
            Comment = comment;
        }

        public int Id { get; }
        public string Comment { get; }

        public object GetEntityId() => Id;
    }
}
