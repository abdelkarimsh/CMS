using CMS.Core.Constants;
using CMS.Core.Dtos;
using CMS.Core.Enums;
using CMS.Infrastructure.Services.Advertisements;
using CMS.Infrastructure.Services.Categories;
using CMS.Infrastructure.Services.Tracks;
using CMS.Infrastructure.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Web.Controllers
{
    public class TrackController : BaseController
    {

        private readonly ITrackService _trackService;
        private readonly ICategoryService _categoryService;


        public TrackController(ITrackService trackService, ICategoryService categoryService, IUserService userService) : base(userService)
        {
            _trackService = trackService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetTracksData(Pagination pagination,Query query)
        {
            var result = await _trackService.GetAll(pagination, query);
            return  Json(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetLog(int Id)
        {
            var logs = await _trackService.GetLog(Id);
            return View(logs);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["categories"] = new SelectList(await _categoryService.GetCategoryList(),"Id","Name");
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> UpdateStatus(int id, ContentStatus status)
        {
            await _trackService.UpdateStatus(id, status);
            return Ok(Results.UpdateStatusSuccessResult());
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateTrackDto dto)
        {
           
            if (ModelState.IsValid)
            {
                await _trackService.Create(dto);
                return Ok(Results.AddSuccessResult());
            }

            ViewData["categories"] = new SelectList(await _categoryService.GetCategoryList(), "Id", "Name");
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var track = await _trackService.Get(id);
            ViewData["categories"] = new SelectList(await _categoryService.GetCategoryList(), "Id", "Name");
            return View(track);
        }


        [HttpPost]
        public async Task<IActionResult> Update(UpdateTrackDto dto)
        {
            if (ModelState.IsValid)
            {
                await _trackService.Update(dto);
                return Ok(Results.EditSuccessResult());
            }

            ViewData["categories"] = new SelectList(await _categoryService.GetCategoryList(), "Id", "Name");
            return View(dto);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _trackService.Delete(id);
            return Ok(Results.DeleteSuccessResult());
        }

    }
}
