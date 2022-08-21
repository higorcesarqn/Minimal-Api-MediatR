using Api.Notifications;
using Api.Requests;

namespace Api.Handlers;

public sealed record ExampleSuccessRequest(string Name, int Age) : IHttpRequest;

public sealed class ExampleRequestSuccessHandler : RequestHandler<ExampleSuccessRequest>
{
    public ExampleRequestSuccessHandler(ILoggerFactory loggerFactory, INotifications notification) : base(loggerFactory, notification)
    {
    }

    protected override async Task<IResult> Process(ExampleSuccessRequest request, CancellationToken cancellationToken)
    {
        await Task.Delay(1000, cancellationToken);
        return Response(new { request.Name, request.Age });
    }
}