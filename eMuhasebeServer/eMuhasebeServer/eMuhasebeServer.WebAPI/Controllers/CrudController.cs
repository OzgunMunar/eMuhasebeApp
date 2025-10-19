using eMuhasebeServer.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TS.Result;

namespace eMuhasebeServer.WebAPI.Controllers
{
    public abstract class CrudController<
    TGetAll, TCreate, TUpdate, TDelete,
    TGetAllResult, TCreateResult, TUpdateResult, TDeleteResult
    >(IMediator mediator)
    : ApiController(mediator)
    where TGetAll : IRequest<Result<TGetAllResult>>
    where TCreate : IRequest<Result<TCreateResult>>
    where TUpdate : IRequest<Result<TUpdateResult>>
    where TDelete : IRequest<Result<TDeleteResult>>
    {
        [HttpPost]
        public Task<IActionResult> GetAll(TGetAll request, CancellationToken ct) => SendRequest(request, ct);

        [HttpPost]
        public Task<IActionResult> Create(TCreate request, CancellationToken ct) => SendRequest(request, ct);

        [HttpPost]
        public Task<IActionResult> Update(TUpdate request, CancellationToken ct) => SendRequest(request, ct);

        [HttpPost]
        public Task<IActionResult> Delete(TDelete request, CancellationToken ct) => SendRequest(request, ct);
    }

}
