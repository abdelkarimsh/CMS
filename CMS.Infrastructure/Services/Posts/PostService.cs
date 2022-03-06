using AutoMapper;
using CMS.Core.Dtos;
using CMS.Core.Enums;
using CMS.Core.Exceptions;
using CMS.Core.ViewModels;
using CMS.Data;
using CMS.Data.Models;
using CMS.Infrastructure.Services.Notifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Infrastructure.Services.Posts
{
    public class PostService : IPostService
    {
        private readonly CMSDbContext _db;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IFileService _fileService;
        private readonly INotificationService _notificationService;
        public PostService(INotificationService notificationService,IEmailService emailService,IFileService fileService, CMSDbContext db, IMapper mapper)
        {
            _db = db;
            _notificationService = notificationService;
            _mapper = mapper;
            _emailService = emailService;
            _fileService = fileService;
        }

        public async Task<ResponseDto> GetAll(Pagination pagination, Query query)
        {
            var queryString = _db.Posts
                .Include(x => x.Attachments)
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Where(x => !x.IsDelete).AsQueryable();

            var dataCount = queryString.Count();
            var skipValue = pagination.GetSkipValue();
            var dataList = await queryString.Skip(skipValue).Take(pagination.PerPage).ToListAsync();
            var posts = _mapper.Map<List<PostViewModel>>(dataList);
            var pages = pagination.GetPages(dataCount);
            var result = new ResponseDto
            {
                data = posts,
                meta = new Meta
                {
                    page = pagination.Page,
                    perpage = pagination.PerPage,
                    pages = pages,
                    total = dataCount,
                }
            };
            return result;
        }

        public async Task<int> Create(CreatePostDto dto)
        {
            var post = _mapper.Map<Post>(dto);
           
            await _db.Posts.AddAsync(post);
            await _db.SaveChangesAsync();

            if(dto.Attachments != null)
            {
                foreach(var a in dto.Attachments)
                {
                    var postAttachment = new PostAttachment();
                    postAttachment.AttachmentUrl = await _fileService.SaveFile(a, "Images");
                    postAttachment.PostId = post.Id;
                    await _db.PostAttachments.AddAsync(postAttachment);
                    await _db.SaveChangesAsync();
                }
            }

            return post.Id;
        }


        public async Task<int> Update(UpdatePostDto dto)
        {
            var post = await _db.Posts.SingleOrDefaultAsync(x => x.Id == dto.Id && !x.IsDelete);
            if (post == null)
            {
                throw new EntityNotFoundException();
            }

            var updatedPost = _mapper.Map(dto,post);


            _db.Posts.Update(updatedPost);
            await _db.SaveChangesAsync();

            if (dto.Attachments != null)
            {
                foreach (var a in dto.Attachments)
                {
                    var postAttachment = new PostAttachment();
                    postAttachment.AttachmentUrl = await _fileService.SaveFile(a, "Images");
                    postAttachment.PostId = post.Id;
                    await _db.PostAttachments.AddAsync(postAttachment);
                    await _db.SaveChangesAsync();
                }
            }

            return post.Id;
        }


        public async Task<UpdatePostDto> Get(int id)
        {
            var post = await _db.Posts.Include(x => x.Attachments).SingleOrDefaultAsync(x => x.Id == id && !x.IsDelete);
            if (post == null)
            {
                throw new EntityNotFoundException();
            }

            var dto = _mapper.Map<UpdatePostDto>(post);

            if(post.Attachments != null)
            {
                dto.PostAttachments = _mapper.Map<List<PostAttachmentViewModel>>(post.Attachments);
            }

            return dto;
        }

        public async Task<int> Delete(int id)
        {
            var post = await _db.Posts.SingleOrDefaultAsync(x => x.Id == id && !x.IsDelete);
            if (post == null)
            {
                throw new EntityNotFoundException();
            }
            post.IsDelete = true;
            _db.Posts.Update(post);
            await _db.SaveChangesAsync();
            return post.Id;
        }

        public async Task<int> RemoveAttachment(int id)
        {
            var post = await _db.PostAttachments.SingleOrDefaultAsync(x => x.Id == id);
            if (post == null)
            {
                throw new EntityNotFoundException();
            }
            _db.PostAttachments.Remove(post);
            await _db.SaveChangesAsync();
            return post.Id;
        }

        public async Task<List<ContentChangeLogViewModel>> GetLog(int id)
        {
            var changes = await _db.ContentChangeLogs.Where(x => x.ContentId == id && x.Type == ContentType.Post).ToListAsync();
            return _mapper.Map<List<ContentChangeLogViewModel>>(changes);
        }

        public async Task<int> UpdateStatus(int id,ContentStatus status)
        {
            var post = await _db.Posts.Include(x => x.Author).SingleOrDefaultAsync(x => x.Id == id && !x.IsDelete);
            if (post == null)
            {
                throw new EntityNotFoundException();
            }

            var changeLog = new ContentChangeLog();
            changeLog.ContentId = post.Id;
            changeLog.Type = ContentType.Post;
            changeLog.Old = post.Status;
            changeLog.New = status;
            changeLog.ChangeAt = DateTime.Now;

            await _db.ContentChangeLogs.AddAsync(changeLog);
            await _db.SaveChangesAsync();

            post.Status = status;
            _db.Posts.Update(post);
            await _db.SaveChangesAsync();

            if(post.Author.FCMToken != null)
            {
                await _notificationService.SendByFCM(post.Author.FCMToken, new NotificationDto()
                {
                    Title = "Update Post",
                    Body = "By Ahmed",
                    Action = NotificationAction.General,
                    ActionId = ""
                });
            }


            await _emailService.Send(post.Author.Email, "UPDATE POST STATUS !", $"YOUR POST NOW IS {status.ToString()}");

            return post.Id;
        }

    }
}
