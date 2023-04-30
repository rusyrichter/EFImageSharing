using HW_4_26.Models;
using ImageSharingEntityFramework.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace HW_4_26.Controllers
{
    public class HomeController : Controller
    {

        private string _connectionString;
        private IWebHostEnvironment _webHostEnvironment;

        public HomeController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var repo = new ImageRepository(_connectionString);
            ImageViewModel vm = new ImageViewModel
            {
                Images = repo.GetImages(),
            };
            return View(vm);
        }
        public IActionResult Upload()
        {          
            return View();
        }
        [HttpPost]
        public IActionResult Upload(Image Image, IFormFile imageFile)
        {
            var fileName = $"{Guid.NewGuid()}-{imageFile.FileName}";
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);
            using var fs = new FileStream(filePath, FileMode.CreateNew);
            imageFile.CopyTo(fs);

            var repo = new ImageRepository(_connectionString);
            Image.FileName = fileName;
            Image.DateUploaded = DateTime.Now;
            repo.AddImage(Image);
            return Redirect("/Home/Index");
        }

        public IActionResult ViewImage(int id)
        {       
            var repo = new ImageRepository(_connectionString);

            var liked = HttpContext.Session.GetInt32($"{id}-session") != null;
            var vm = new ImageViewModel
            {
                Image = repo.GetById(id),
                Liked = liked,
            };
            return View(vm);
        }
        public IActionResult GetLikes(int id)
        {
            var repo = new ImageRepository(_connectionString);
            
            Image image = repo.GetById(id);

            return Json(image.Likes);

        }
        [HttpPost]
        public IActionResult AddLike(int id)
        {           
            var repo = new ImageRepository(_connectionString);          
            var likesPerImage = HttpContext.Session.GetInt32($"{id}-session");
            if (likesPerImage == null)
            {
                HttpContext.Session.SetInt32($"{id}-session", id);
                repo.UpdateLikes(id);
            }

            return Redirect($"/Home/ViewImage?id={id}");

        }


    }
}