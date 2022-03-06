using CMS.Core.Dtos;
using CMS.Core.Enums;
using CMS.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Infrastructure.Services.Tracks
{
    public interface ITrackService
    {
        Task<ResponseDto> GetAll(Pagination pagination, Query query);
        Task<int> Delete(int id);
        Task<UpdateTrackDto> Get(int id);
        Task<int> Create(CreateTrackDto dto);
        Task<int> UpdateStatus(int id, ContentStatus status);
        Task<int> Update(UpdateTrackDto dto);
        Task<List<ContentChangeLogViewModel>> GetLog(int id);
    }

}
