using Api.Notifications;
using Api.Requests;
using MediatR;

#nullable disable

namespace Api;

public static class MinimalMediatRExtensions
{
    public static IServiceCollection AddNotificatons(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

        services.AddScoped((provider) =>
        {
            var notification = provider.GetService<INotificationHandler<DomainNotification>>();
            return (INotifications)notification;
        });

        services.AddScoped<INotifiable, Notifiable>();

        return services;
    }

    public static IServiceCollection AddRequest<TRequest, TRequestHandler>(this IServiceCollection services)
        where TRequest : IHttpRequest
        where TRequestHandler : Requests.RequestHandler<TRequest>
    {
        services.AddScoped<IRequestHandler<TRequest, IResult>, TRequestHandler>();

        return services;
    }

    public static WebApplication MediatGet<TRequest>(this WebApplication app, string template)
        where TRequest : IHttpRequest
    {
        app.MapGet(template, async (IMediator mediator, [AsParameters] TRequest request) => await mediator.Send(request));
        return app;
    }

    public static WebApplication MediatPost<TRequest>(this WebApplication app, string template)
      where TRequest : IHttpRequest
    {
        app.MapPost(template, async (IMediator mediator, [AsParameters] TRequest request) => await mediator.Send(request));
        return app;
    }

    public static WebApplication MediatDelete<TRequest>(this WebApplication app, string template)
        where TRequest : IHttpRequest
    {
        app.MapDelete(template, async (IMediator mediator, [AsParameters] TRequest request) => await mediator.Send(request));
        return app;
    }

    public static WebApplication MediatPut<TRequest>(this WebApplication app, string template)
    where TRequest : IHttpRequest
    {
        app.MapPut(template, async (IMediator mediator, [AsParameters] TRequest request) => await mediator.Send(request));
        return app;
    }
}