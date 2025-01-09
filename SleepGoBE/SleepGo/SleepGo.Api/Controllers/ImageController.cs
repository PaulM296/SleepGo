using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SleepGo.App.DTOs.ImageDtos;
using SleepGo.App.Features.Images.Commands;
using SleepGo.App.Features.Images.Queries;

namespace SleepGo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ImageController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ImageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage([FromForm] UploadImageDto uploadImageDto)
        {
            var command = new UploadImageCommand(uploadImageDto);
            var response = await _mediator.Send(command);

            return Ok(response);
        }

        [HttpDelete("id")]
        public async Task<IActionResult> RemoveImage(Guid id)
        {
            var command = new RemoveImageCommand(id);
            var response = await _mediator.Send(command);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImageById(Guid id)
        {
            var query = new GetImageByIdQuery(id);
            var image = await _mediator.Send(query);

            if (image == null)
            {
                return NotFound();
            }

            var base64 = Convert.ToBase64String(image.Data);
            var imgSrc = $"data:{image.Type};base64,{base64}";

            return Ok(new { imageSrc = imgSrc });
        }
    }
}
