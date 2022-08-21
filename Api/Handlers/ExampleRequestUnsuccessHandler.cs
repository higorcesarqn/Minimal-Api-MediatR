using Api.Notifications;
using Api.Requests;

namespace Api.Handlers;

public sealed record ExampleUnsuccessRequest(string Name, int Age) : IHttpRequest;

public sealed class ExampleRequestUnsuccessHandler : RequestHandler<ExampleUnsuccessRequest>
{
    private readonly INotifiable _notifiable;

    public ExampleRequestUnsuccessHandler(ILoggerFactory loggerFactory, INotifiable notifiable, INotifications notification) : base(loggerFactory, notification)
    {
        _notifiable = notifiable;
    }

    protected override async Task<IResult> Process(ExampleUnsuccessRequest request, CancellationToken cancellationToken)
    {
        await _notifiable.Notify(("error", "error message"));
        await Task.Delay(200, cancellationToken);
        return Response(new { request.Name, request.Age });
    }
}