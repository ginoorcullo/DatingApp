using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp2.API.Data.BaseRepository;
using DatingApp2.API.DTO;
using DatingApp2.API.Helpers;
using DatingApp2.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DatingApp2.API.Controllers
{
    [Route("api/users/{userId}/photos")]
    [ApiController]
    [Authorize]
    public class PhotosController : ControllerBase
    {
        private readonly IDatingRepository repo;
        private readonly IMapper mapper;
        private readonly IOptions<CloudinarySettings> cloudinaryConfig;
        private readonly Cloudinary cloudinary;

        public PhotosController(IDatingRepository repo,
            IMapper mapper,
            IOptions<CloudinarySettings> cloudinaryConfig)
        {
            this.repo = repo;
            this.mapper = mapper;
            this.cloudinaryConfig = cloudinaryConfig;

            Account acc = new Account(
                cloud: this.cloudinaryConfig.Value.CloudName,
                apiKey: this.cloudinaryConfig.Value.ApiKey,
                apiSecret: this.cloudinaryConfig.Value.ApiSecret
            );

            this.cloudinary = new Cloudinary(acc);
        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoEntity = await repo.GetPhoto(id);

            var photo = mapper.Map<PhotoForReturnDTO>(photoEntity);
            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserPhoto(int userId, [FromForm]PhotoForCreationDTO photoForCreationDTO)
        {

            var result = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (userId != int.Parse(result))
                return Unauthorized();
            var userEntity = await repo.GetUser(userId);

            var file = photoForCreationDTO.File;

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation()
                                            .Width(500)
                                            .Height(500)
                                            .Crop("fill")
                                            .Gravity("face")
                    };

                    uploadResult = cloudinary.Upload(uploadParams);
                }
            }

            photoForCreationDTO.Url = uploadResult.Uri.ToString();
            photoForCreationDTO.PublicId = uploadResult.PublicId;

            var photo = mapper.Map<Photo>(photoForCreationDTO);

            if (!userEntity.Photos.Any(x => x.IsMain))
                photo.IsMain = true;

            userEntity.Photos.Add(photo);

            if (await repo.SaveAll())
            {
                var photoForReturn = mapper.Map<PhotoForReturnDTO>(photo);
                return CreatedAtRoute(routeName: "GetPhoto", routeValues: new { userId = userId, id = photo.Id }, value: photoForReturn);
            }

            return BadRequest("Could not upload photo.");

        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int userId, int id)
        {
            var result = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (userId != int.Parse(result))
                return Unauthorized();

            var userEntity = await repo.GetUser(userId);

            if (!userEntity.Photos.Any(x => x.Id == id))
                return Unauthorized();

            var photoFromRepo = await repo.GetPhoto(id);

            if (photoFromRepo.IsMain)
                return BadRequest("This is already the main photo.");

            var currentMainPhoto = await repo.GetUserMainPhoto(userId);
            currentMainPhoto.IsMain = false;
            photoFromRepo.IsMain = true;

            if (await repo.SaveAll())
                return NoContent();

            return BadRequest("Could not set photo to main");

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemovePhoto(int userId, int id)
        {
            var result = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (userId != int.Parse(result))
                return Unauthorized();

            var userEntity = await repo.GetUser(userId);

            if (!userEntity.Photos.Any(x => x.Id == id))
                return Unauthorized();

            var photoFromRepo = await repo.GetPhoto(id);

            if (photoFromRepo.IsMain)
                return BadRequest("You cannot delete your main photo.");

            if (photoFromRepo.PublicId != null)
            {
                var deleteParams = new DeletionParams(photoFromRepo.PublicId);
                var onDeleteResult = cloudinary.Destroy(deleteParams);
                if (onDeleteResult.Result == "ok")
                {
                    repo.Delete(photoFromRepo);
                }
            }
            else
            {
                repo.Delete(photoFromRepo);
            }


            if (await repo.SaveAll())
                return Ok();

            return BadRequest("Error occurred on deleting photo");
        }

    }
}