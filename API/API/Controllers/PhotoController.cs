using API.Authorization;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Headers;
using Data.Services;

namespace API.Controllers
{
    [RoutePrefix(RoutePrefix)]
    public class PhotoController : ApiController
    {
        private const string RoutePrefix = "api/photos";
        private readonly IPhotoService _photoService;
        private readonly IAuthManager _authManager;

        public PhotoController(IAuthManager authManager, IPhotoService photoService)
        {
            _authManager = authManager;
            _photoService = photoService;
        }

        /// <summary>
        /// Retrieves a photo from an organization.
        /// </summary>
        /// <param name="photoId">Id of the photo.</param>
        /// <returns>
        /// A response containing the image if found. 
        /// If the provided authorization token is invalid: Http 400 (Bad Request) is returned.
        /// If the photo is not found: Http 404 (Not Found) is returned.</returns>
        [AdminFilter]
        [HttpGet, Route("{photoId}")]
        public IHttpActionResult Get(int photoId)
        {
            var manager = _authManager.GetManagerByHeader(Request.Headers);
            if (manager == null) return BadRequest("Provided token is invalid!");

            var photo = _photoService.Read(photoId, manager.Organization.Id);

            if (photo == null) return NotFound();

            var message = new HttpResponseMessage(HttpStatusCode.OK);

            message.Content = new ByteArrayContent(photo.Data);
            message.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(photo.Type);

            return ResponseMessage(message);
        }

    }
}