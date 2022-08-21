namespace Api.Notifications
{
    public interface INotifications
    {
        IReadOnlyList<DomainNotification> GetNotifications();

        bool HasNotifications() => GetNotifications().Any();
    }
}