using CMS.Core.Dtos;
using CMS.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Infrastructure.Services.Users
{
    public interface IUserService
    {
        Task<ResponseDto> GetAll(Pagination pagination, Query query);
        Task<string> Create(CreateUserDto dto);
        Task<List<UserViewModel>> GetAuthorList();
        Task<string> Update(UpdateUserDto dto);
        UserViewModel GetUserByUsername(string username);
        Task<string> Delete(string Id);
        Task<byte[]> ExportToExcel();
        Task<string> SetFCMToUser(string userId, string fcmToken);
        Task<UpdateUserDto> Get(string Id);

    }
}
