using FaceId.Web.Api.Services;
using FaceId.Web.Api.ViewModels;
using FaceId.Web.Core;
using FaceId.Web.Core.Abstract;
using FaceId.Web.Core.Models;
using FaceId.Web.Core.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceId.Web.Api.Controllers
{
    [Route("api/image-recognition")]
    public class ImageRecognitionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IFileService _fileService;
        public ImageRecognitionController(IMediator mediator, IFileService fileService)
        {
            _mediator = mediator;
            _fileService = fileService;
        }

        [HttpPost("gender-recognition")]
        public async Task<ActionResult<Dictionary<string, float>>> GenderRecogniton(FileViewModel model)
        {
            if (model.File == null)
                return BadRequest("File is null");

            var result = await _mediator.Send(new GetPrediction<GenderInMemoryImageData>() { File = model.File });
            var best = result.OrderByDescending(x => x.Value).FirstOrDefault();

            await _fileService.SaveImageAsync(model.File, best.Key, Constants.GenderContainer, new GuidFileNameService());
            return Ok(result);
        }

        [HttpPost("age-recognition")]
        public async Task<ActionResult<Dictionary<string, float>>> AgeRecognition(FileViewModel model)
        {
            if (model.File == null)
                return BadRequest("File is null");

            var result = await _mediator.Send(new GetPrediction<AgeInMemoryImageData>() { File = model.File });
            var best = result.OrderByDescending(x => x.Value).FirstOrDefault();

            await _fileService.SaveImageAsync(model.File, best.Key, Constants.AgeContainer, new GuidFileNameService());
            return Ok(result);
        }

    }
}
