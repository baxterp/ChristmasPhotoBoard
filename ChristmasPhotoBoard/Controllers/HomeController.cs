using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection.Metadata;
//using static System.Net.Mime.MediaTypeNames;

namespace ChristmasPhotoBoard.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}/";
            var relativeDir = "images";

            var files = Directory.GetFiles("wwwroot/images")
                        .OrderByDescending(f => new FileInfo(f).CreationTime);

            var fileURLs = files.Select(Path.GetFileName)
                            .Where(name => !string.IsNullOrEmpty(name))
                            .Select(name => $"{baseUrl}{relativeDir}/{name}")
                            .ToList();

            return View(fileURLs);
        }

        public IActionResult Carousel()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}/";
            var relativeDir = "images";

            var files = Directory.GetFiles("wwwroot/images")
                        .OrderByDescending(f => new FileInfo(f).CreationTime);

            var fileURLs = files.Select(Path.GetFileName)
                            .Where(name => !string.IsNullOrEmpty(name))
                            .Select(name => $"{baseUrl}{relativeDir}/{name}")
                            .ToList();

            return View(fileURLs);
        }

        [HttpPost]
        [Route("[controller]/ImageUpload")]
        public IActionResult ImageUpload(IFormFile file)
        {
            if (file == null)
                return BadRequest("No files were uploaded");

            var fileExtension = file.FileName.Split('.').Last().ToLower();

            if (fileExtension != "bmp" && fileExtension != "gif" && fileExtension != "jpeg" && fileExtension != "jpg" && fileExtension != "tiff" && fileExtension != "png")
            {
                return BadRequest("File format, " + fileExtension + " is not acceptable, use .bmp, .gif, .jpeg, .jpg, .tiff, .png");
            }

            var filename = file.FileName.Split('.').FirstOrDefault() + DateTime.Now.ToString("-HHmmssff") + "." + fileExtension;
            var filePath = "wwwroot/images/" + filename;

            Image img;
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                using (var memstream = new MemoryStream())
                {
                    reader.BaseStream.CopyTo(memstream);
                    img = Image.FromStream(memstream, true);
                }
            }

            var width = img.Width;
            var height = img.Height;

            if ((width > 1000 || height > 1000))
            {
                int newHeight = 0;
                int newWidth = 0;

                if (height > width)
                {
                    newWidth = 1000;
                    newHeight = (int)(((double)height / (double)width) * 1000);
                }
                else
                {
                    newHeight = 1000;
                    newWidth = (int)(((double)width / (double)height) * 1000);
                }

                Bitmap bitmap = new Bitmap(newWidth, newHeight);
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.DrawImage(img, newWidth, newHeight);
                }
                var newFile = new Bitmap(img, new Size(newWidth, newHeight));
                newFile.Save(filePath, ImageFormat.Png);
            }
            else
            {
                using (FileStream output = System.IO.File.Create(filePath))
                {
                    file.CopyTo(output);
                }
            }

            return Ok();
        }
    }
}
