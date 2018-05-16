﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Cognitive_Azure.Features.Images
{
    public class ImagesController : Controller
    {
        public ICloudStorageService CloudStorageService { get; set; }

        public ICloudTableService CloudTableService { get; set; }

        public ITextService TextService { get; set; }

        public ImagesController(ICloudStorageService cloudStorageService, ICloudTableService cloudTableService, ITextService textService)
        {
            CloudStorageService = cloudStorageService;
            CloudTableService = cloudTableService;
            TextService = textService;
        }

        public async Task<IActionResult> Index()
        {
            var items = await CloudTableService.RetrieveImages();

            return View(items.ToList());
        }

        public async Task<IActionResult> View(Guid id)
        {
            var item = await CloudTableService.RetrieveImage(id);

            return View(item);
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

                return RedirectToAction("Index", "Images");
            }

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> AnalyseComment(string comment)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(comment)) 
            {
                var score = await TextService.GetScore(comment);

                return new JsonResult(new { score });
            }

            return new JsonResult(new { score = 0 });
        }

        public async Task<IActionResult> AddComment(Guid id, string comment)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(comment))
            {
                var score = await TextService.GetScore(comment);

                await CloudTableService.AddComment(id, comment, score);
            }

            return RedirectToAction("View", new { id });
        }
    }
}