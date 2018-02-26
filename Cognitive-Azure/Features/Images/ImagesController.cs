﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Cognitive_Azure.Features.Images
{
    public class ImagesController : Controller
    {
        public ICloudStorageService CloudStorageService { get; set; }

        public ImagesController(ICloudStorageService cloudStorageService)
        {
            CloudStorageService = cloudStorageService;
        }

        public IActionResult Index()
        {
            var items = CloudStorageService.RetrieveImages();

            return View(items.ToList());
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Upload(Upload model)
        {
            if (ModelState.IsValid)
            {
                CloudStorageService.UploadImage(model.Image);
            }

            return View(model);
        }
    }
}