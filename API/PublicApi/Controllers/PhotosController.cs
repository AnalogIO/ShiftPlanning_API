using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Data.Services;

namespace PublicApi.Controllers
{
    [RoutePrefix(RoutePrefix)]
    public class PhotosController : ApiController
    {
        public const string RoutePrefix = "api/photos";
        private readonly IPhotoService _photoService;

        public PhotosController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpGet, Route("{shortKey}/{id:int}")]
        public IHttpActionResult Get(string shortKey, int id)
        {
            var photo = _photoService.Read(id, shortKey);

            if (photo == null) return NotFound();

            var message = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(photo.Data ?? new byte[0])
            };

            message.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(photo.Type);

            return ResponseMessage(message);
        }
    }
}