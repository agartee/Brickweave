namespace BasicCqrs.Domain.People.Models
{
    public class PersonSearchCriteria
    {
        public PersonSearchCriteria(string nameLike)
        {
            NameLike = nameLike ?? string.Empty;
        }

        public string NameLike { get; }
    }
}
