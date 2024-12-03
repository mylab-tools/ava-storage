using System.ComponentModel.DataAnnotations;
using System.Net;
using AvaStorage.Application.UseCases.PutAvatar;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyLab.WebErrors;

namespace AvaStorage.Controllers
{
    [ApiController]
    [Route("v1/ava")]
    public class AvaController(IMediator mediator) : ControllerBase
    {

        [HttpPut]
        [Consumes("application/octet-stream")]
        [ErrorToResponse(typeof(ValidationException), HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PutAsync
            (
                [FromQuery(Name = "id")][Required(AllowEmptyStrings = false)] string id,
                [FromQuery(Name = "st")] string? subjectType,
                [FromBody][Required] byte[] picture,
                CancellationToken cancellationToken
            )
        {
            await mediator.Send(new PutAvatarCommand(id, subjectType, picture), cancellationToken);

            return Ok();
        }

        [HttpGet]
        public Task<IActionResult> GetAsync
            (
                [FromQuery]string id, 
                [FromQuery]int? size, 
                CancellationToken cancellationToken
            )
        {
            throw new NotImplementedException();
        }
    }
}
