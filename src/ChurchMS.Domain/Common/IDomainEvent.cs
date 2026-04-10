using MediatR;

namespace ChurchMS.Domain.Common;

/// <summary>
/// Marker interface for domain events dispatched via MediatR.
/// </summary>
public interface IDomainEvent : INotification;
