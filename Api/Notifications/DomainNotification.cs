using System;

namespace Api.Notifications
{
    public class DomainNotification : MediatR.INotification
    {
        public DateTime Timestamp { get; private set; }
        public Guid DomainNotificationId { get; private set; }
        public string Key { get; private set; }
        public string Value { get; private set; }
        public Guid AggregateId { get; set; }

        public DomainNotification(string key, string value)
        {
            DomainNotificationId = Guid.NewGuid();
            Key = key;
            Value = value;
            Timestamp = DateTime.Now;
        }
    }
}