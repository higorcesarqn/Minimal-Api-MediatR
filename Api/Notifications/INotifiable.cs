using Api.Types;

namespace Api.Notifications;

public interface INotifiable
{
    Task Notify(string key, string value)
    {
        return Notify(new DomainNotification(key, value));
    }

    Task Notify(Error erro)
    {
        return Notify(new DomainNotification(erro.Key, erro.Message));
    }

    async Task Notify(Errors errors)
    {
        foreach (var erro in errors)
        {
            await Notify(erro).ConfigureAwait(false);
        }
    }

    Task Notify(DomainNotification notification);

    public bool IsValid();
}