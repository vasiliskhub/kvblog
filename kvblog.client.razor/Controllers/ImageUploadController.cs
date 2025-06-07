using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ImageUploadController : Controller
    {
        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            var photoId = Guid.NewGuid();

            if (file == null || file.Length == 0)
            {
                return BadRequest("Upload a valid image.");
            }

            var filePath = Path.Combine("wwwroot/uploads", photoId+file.FileName);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var imageUrl = Url.Content("~/uploads/" + photoId+file.FileName);
            return new JsonResult(new { location = imageUrl }); // TinyMCE expects JSON response with 'location' key
        }
    }
}
