using Api.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Api.Requests;

public abstract class RequestHandler<THttpRequest> : IRequestHandler<THttpRequest, IResult>
    where THttpRequest : IHttpRequest
{
    private readonly INotifications _notifications;

    protected ILogger Logger { get; }

    protected RequestHandler(ILoggerFactory loggerFactory, INotifications notification)
    {
        Logger = loggerFactory.CreateLogger(GetType());
        _notifications = notification;
    }

    public async Task<IResult> Handle(THttpRequest request, CancellationToken cancellationToken)
    {
        Logger.LogTrace("Processing Request '{command}' ...", request);
        var watch = Stopwatch.StartNew();

        try
        {
            var response = await Process(request, cancellationToken).ConfigureAwait(false);

            return response;
        }
        finally
        {
            watch.Stop();
            Logger.LogTrace("Processed Command '{command}': {ElapsedMilliseconds} ms", request, watch.ElapsedMilliseconds);
        }
    }

    protected abstract Task<IResult> Process(THttpRequest request, CancellationToken cancellationToken);

    protected IResult Response(object result)
    {
        if (!IsValidOperation())
        {
            return ResponseBadRequest();
        }

        if (result == default)
        {
            return Results.NoContent();
        }

        return Results.Ok(result);
    }

    protected IResult Response<T>(IEnumerable<T> enumerable)
    {
        if (!IsValidOperation())
        {
            return ResponseBadRequest();
        }

        if (enumerable == default)
        {
            return ResponseNotFound();
        }

        if (!enumerable.Any())
        {
            return ResponseNoContent();
        }

        return Results.Ok(enumerable);
    }

    protected IResult ResponseNotFound()
    {
        if (!IsValidOperation())
        {
            return ResponseBadRequest();
        }

        return Results.NotFound();
    }

    protected IResult ResponseNoContent()
    {
        if (!IsValidOperation())
        {
            return ResponseBadRequest();
        }

        return Results.NoContent();
    }

    protected IResult ResponseCreated(string uri, object result)
    {
        if (result == default)
        {
            return ResponseNotFound();
        }

        if (!IsValidOperation())
        {
            return ResponseBadRequest();
        }

        return Results.Created(uri, result);
    }

    protected bool IsValidOperation()
    {
        return !_notifications.HasNotifications();
    }

    protected IResult ResponseBadRequest()
    {
        return Results.BadRequest(
            _notifications
            .GetNotifications()
            .GroupBy(x => x.Key.ToLower().Split(".")[0])
            .ToDictionary(k => k.Key, v => v.Select(s => s.Value))
        );
    }
}