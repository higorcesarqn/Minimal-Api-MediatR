using Api.Notifications;
using Api.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Api.Handlers;

public sealed record ExamplePostRequest([FromRoute]string Section,[FromBody] UserPostModel UserPostModel) : IHttpRequest;

public sealed record UserPostModel(string Name, int Age);

public sealed class ExampleRequestPostHandler : RequestHandler<ExamplePostRequest>
{
    public ExampleRequestPostHandler(ILoggerFactory loggerFactory, INotifications notification) : base(loggerFactory, notification)
    {
    }

    protected override async Task<IResult> Process(ExamplePostRequest request, CancellationToken cancellationToken)
    {
        await Task.Delay(1000, cancellationToken);
        return ResponseCreated($"sucess/{Guid.NewGuid()}", request.UserPostModel);
    }
}