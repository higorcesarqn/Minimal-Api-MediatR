using MediatR;

namespace Api.Notifications
{
    public class Notifiable : INotifiable
    {
        private readonly IMediator _bus;
        private readonly INotifications _notifications;

        public Notifiable(IMediator bus, INotifications notifications)
        {
            _bus = bus;
            _notifications = notifications;
        }

        public Task Notify(DomainNotification notification)
        {
            return _bus.Publish(notification);
        }

        public bool IsValid()
        {
            return !_notifications.HasNotifications();
        }
    }
}