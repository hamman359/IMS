using HappyPlate.Domain.Primatives;

namespace HappyPlate.Domain.DomainEvents;

public abstract record DomainEvent(Guid Id) : IDomainEvent;