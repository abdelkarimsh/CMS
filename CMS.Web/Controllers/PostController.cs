using CMS.Core.Constants;
using CMS.Core.Dtos;
using CMS.Core.Enums;
using CMS.Infrastructure.Services.Advertisements;
using CMS.Infrastructure.Services.Categories;
using CMS.Infrastructure.Services.Posts;
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
    public class PostController : BaseController
    {

        private readonly IPostService _postService;
        private readonly ICategoryService _categoryService;

        

        public PostController(IUserService userService,IPostService postService, ICategoryService categoryService) : base(userService)
        {
            _postService = postService;
            _categoryService = categoryService;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetPostData(Pagination pagination,Query query)
        {
            var result = await _postService.GetAll(pagination, query);
            return  Json(result);
        }


        public async Task<IActionResult> GetLog(int Id)
        {
            var logs = await _postService.GetLog(Id);
            return View(logs);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["categories"] = new SelectList(await _categoryService.GetCategoryList(),"Id","Name");
            ViewData["authores"] = new SelectList(await _userService.GetAuthorList(), "Id", "FullName");
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreatePostDto dto)
        {
           
            if (ModelState.IsValid)
            {
                await _postService.Create(dto);
                return Ok(Results.AddSuccessResult());
            }

            ViewData["categories"] = new SelectList(await _categoryService.GetCategoryList(), "Id", "Name");
            ViewData["authores"] = new SelectList(await _userService.GetAuthorList(), "Id", "FullName");
            return View(dto);
        }


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var track = await _postService.Get(id);
            ViewData["categories"] = new SelectList(await _categoryService.GetCategoryList(), "Id", "Name");
            ViewData["authores"] = new SelectList(await _userService.GetAuthorList(), "Id", "FullName");
            return View(track);
        }


        [HttpPost]
        public async Task<IActionResult> Update(UpdatePostDto dto)
        {
            if (ModelState.IsValid)
            {
                await _postService.Update(dto);
                return Ok(Results.EditSuccessResult());
            }
            ViewData["categories"] = new SelectList(await _categoryService.GetCategoryList(), "Id", "Name");
            ViewData["authores"] = new SelectList(await _userService.GetAuthorList(), "Id", "FullName");
            return View(dto);
        }



        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _postService.Delete(id);
            return Ok(Results.DeleteSuccessResult());
        }


        [HttpGet]
        public async Task<IActionResult> RemoveAttachment(int id)
        {
            await _postService.RemoveAttachment(id);
            return Ok(Results.DeleteSuccessResult());
        }



        [HttpGet]
        public async Task<IActionResult> UpdateStatus(int id,ContentStatus status)
        {
            await _postService.UpdateStatus(id, status);
            return Ok(Results.UpdateStatusSuccessResult());
        }


    }
}
