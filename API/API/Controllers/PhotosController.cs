using API.Authorization;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Headers;
using Data.Services;

namespace API.Controllers
{
    [RoutePrefix(RoutePrefix)]
    public class PhotosController : ApiController
    {
        public const string RoutePrefix = "api/photos";
        private readonly IPhotoService _photoService;
        private readonly IAuthManager _authManager;

        public PhotosController(IAuthManager authManager, IPhotoService photoService)
        {
            _authManager = authManager;
            _photoService = photoService;
        }

        /// <summary>
        /// Retrieves a photo from an organization.
        /// </summary>
        /// <param name="photoId">Id of the photo.</param>
        /// <param name="organizationId">Id of the organization.</param>
        /// <returns>
        /// A response containing the image if found. 
        /// If the provided authorization token is invalid: Http 400 (Bad Request) is returned.
        /// If the photo is not found: Http 404 (Not Found) is returned.</returns>
        [HttpGet, Route("{photoId}/{organizationId}")]
        public IHttpActionResult Get(int photoId, int organizationId)
        {
            var photo = _photoService.Read(photoId, organizationId);

            if (photo == null) return NotFound();

            var message = new HttpResponseMessage(HttpStatusCode.OK);

            message.Content = new ByteArrayContent(photo.Data);
            message.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(photo.Type);

            return ResponseMessage(message);
        }

    }
}