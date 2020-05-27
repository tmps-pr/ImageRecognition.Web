using FaceId.Web.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections;
using System.Collections.Generic;

namespace FaceId.Web.Core.Queries
{
    public class GetPrediction<T> : IRequest<Dictionary<string, float>> where T: InMemoryImageData
    {
        public IFormFile File { get; set; } 
    }
}
