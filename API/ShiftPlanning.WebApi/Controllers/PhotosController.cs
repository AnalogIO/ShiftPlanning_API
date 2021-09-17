using System;
using Microsoft.AspNetCore.Mvc;
using ShiftPlanning.WebApi.Helpers.Authorization;
using ShiftPlanning.WebApi.Helpers.Mappers;
using ShiftPlanning.WebApi.Services;

namespace ShiftPlanning.WebApi.Controllers
{
    [ApiController]
    [Route(RoutePrefix)]
    public class PhotosController : ControllerBase
    {
        public const string RoutePrefix = "api/photos";
        private readonly IPhotoService _photoService;
        private readonly IAuthManager _authManager;
        private readonly IPhotoMapper _photoMapper;

        public PhotosController(IPhotoService photoService, IAuthManager authManager, IPhotoMapper photoMapper)
        {
            _authManager = authManager;
            _photoService = photoService;
            _photoMapper = photoMapper;
        }
        
        [HttpGet, Route("{shortKey}/{id:int}")]
        public IActionResult Get(string shortKey, int id)
        {
            var photo = _photoService.Read(id, shortKey);

            if (photo == null) return NotFound();
            
            var content = photo.Data ?? Array.Empty<byte>();
            return File(content, photo.Type);
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
        [HttpGet, Route("{photoId:int}/{organizationId:int}")]
        public IActionResult Get(int photoId, int organizationId)
        {
            var photo = _photoService.Read(photoId, organizationId);

            if (photo == null) return NotFound();

            var content = photo.Data ?? Array.Empty<byte>();
            return File(content, photo.Type);
        }

        [HttpPost, Route("/")]
        public IActionResult Post([FromBody] string base64EncodedPhoto)
        {
            var employee = _authManager.GetEmployeeByHeader(Request.Headers);

            var photo = _photoMapper.ParseBase64Photo(base64EncodedPhoto, employee.Organization);

            photo = _photoService.CreatePhoto(photo, employee);
            
            return Created($"{HttpContext.Request.Path}/{photo.Id}/{employee.Organization.Id}", photo.Data);
        }
    }
}