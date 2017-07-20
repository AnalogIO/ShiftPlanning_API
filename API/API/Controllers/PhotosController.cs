using API.Authorization;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Headers;
using API.Logic;
using Data.Models;
using Data.Services;

namespace API.Controllers
{
    [RoutePrefix(RoutePrefix)]
    public class PhotosController : ApiController
    {
        public const string RoutePrefix = "api/photos";
        private readonly IPhotoService _photoService;
        private readonly IAuthManager _authManager;
        private readonly PhotoMapper _photoMapper;

        public PhotosController(IAuthManager authManager, IPhotoService photoService, PhotoMapper photoMapper)
        {
            _authManager = authManager;
            _photoService = photoService;
            _photoMapper = photoMapper;
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

            var message = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(photo.Data)
            };

            message.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(photo.Type);

            return ResponseMessage(message);
        }

        [HttpPost, AdminFilter]
        public IHttpActionResult Post([FromBody] string base64EncodedPhoto)
        {
            var manager = _authManager.GetManagerByHeader(Request.Headers);

            var photo = _photoMapper.ParseBase64Photo(base64EncodedPhoto, manager.Organization);

            photo = _photoService.CreatePhoto(photo, manager);

            return Created($"{Request.RequestUri}/{photo.Id}/{manager.Organization.Id}", photo.Data);
        }
    }
}