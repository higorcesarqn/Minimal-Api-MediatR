using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Notifications
{
    public class DomainNotificationHandler : INotifications, INotificationHandler<DomainNotification>
    {
        private List<DomainNotification> _notifications;
        private readonly ILogger<DomainNotificationHandler> _logger;

        public DomainNotificationHandler(ILogger<DomainNotificationHandler> logger)
        {
            _notifications = new List<DomainNotification>();
            _logger = logger;
        }

        public virtual IReadOnlyList<DomainNotification> GetNotifications()
        {
            return _notifications;
        }

        public void Dispose()
        {
            _notifications = new List<DomainNotification>();
        }

        public virtual bool HasNotifications() => GetNotifications().Any();

        public Task Handle(DomainNotification notification, CancellationToken cancellationToken)
        {
            _logger.LogTrace($"Domain Notification: {notification.Key} - {notification.Value}");
            _notifications.Add(notification);
            return Task.CompletedTask;
        }
    }
}