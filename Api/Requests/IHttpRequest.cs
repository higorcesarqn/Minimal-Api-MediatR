using MediatR;

namespace Api.Requests;

public interface IHttpRequest : IRequest<IResult>
{
}