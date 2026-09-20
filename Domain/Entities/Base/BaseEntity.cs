namespace Domain.Entities.Base
{
    public class BaseEntity<TId> : DomainEntity where TId : notnull
    {
        public TId Id { get; set; } = default!;
    }
}
