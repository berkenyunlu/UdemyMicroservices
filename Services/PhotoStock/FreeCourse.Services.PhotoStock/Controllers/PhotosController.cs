﻿using FreeCourse.Services.PhotoStock.Dtos;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FreeCourse.Services.PhotoStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : CustomBaseController
    {
        [HttpPost]
        public async Task<IActionResult> PhotoSave(IFormFile photo, CancellationToken cancellationToken)
        {
            if (photo != null && photo.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photo.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await photo.CopyToAsync(stream, cancellationToken);
                    PhotoDto photoDto = new()
                    {
                        Url = "photos/" + photo.FileName
                    };
                    return CreateActionResultInstance(Response<PhotoDto>.Success(photoDto, 200));
                }
            }
            return CreateActionResultInstance(Response<PhotoDto>.Fail("photo is empty", 400));
        }

        public async Task<IActionResult> PhotoDelete(string photoUrl)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photoUrl);
            if (!System.IO.File.Exists(path))
            {
                return CreateActionResultInstance(Response<NoContent>.Fail("photo not found", 404));
            }
            System.IO.File.Delete(path);
            return CreateActionResultInstance(Response<NoContent>.Success(204));
        }
    }
}
