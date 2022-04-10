using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pictures.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Pictures.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace Pictures.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HomeController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;

        }

 
        public IActionResult Index()
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new PictureDataRepo(connectionString);
            var pictures = repo.AllPictures();
            PicViewModel pv = new PicViewModel();
            pv.pictures = pictures;
            return View(pv);
        }
        public IActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Upload(IFormFile imageFile, string Name)
        {

            string fileName = $"{Guid.NewGuid()}-{imageFile.FileName}";
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);
            using var fs = new FileStream(filePath, FileMode.CreateNew);
            imageFile.CopyTo(fs);

            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new PictureDataRepo(connectionString);
            Picture pc = new Picture
            {
                ImageName = Name,
                FilePath = fileName,
                Date = DateTime.Now,
                NumberOfLikes=0
            };
            repo.Add(pc);
            return Redirect("/");
        }
        public IActionResult ViewImage(int id)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new PictureDataRepo(connectionString);
            var Picture = repo.GetById(id);
            List<int> imagesLiked = HttpContext.Session.Get<List<int>>("Ids");
            if (imagesLiked == null)
            {
                imagesLiked = new List<int>();
            }
            SingleView sv = new SingleView
            {
                picture = Picture,
                AlreadyLiked= imagesLiked.Contains(id)
            };
            return View(sv);
        }
        public IActionResult GetLikes(int id)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new PictureDataRepo(connectionString);
            var Picture = repo.GetById(id);
            return Json(Picture.NumberOfLikes);
        }
        [HttpPost]
        public IActionResult UpdateLikes(int id)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new PictureDataRepo(connectionString);
            List<int> imagesLiked = HttpContext.Session.Get<List<int>>("Ids");
            if (imagesLiked == null)
            {
                imagesLiked = new List<int>();
            }
            if (imagesLiked.Contains(id))
            {
                return Json(id);
            }
            imagesLiked.Add(id);
            HttpContext.Session.Set("Ids", imagesLiked);
            repo.Update(id);
            return Json(id);
        }
    }
}
