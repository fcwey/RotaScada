using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface ITagService
    {
        Task<IDataResult<List<PlcTagConfig>>> GetAllTagsAsync();
        Task<IDataResult<PlcTagConfig>> GetTagByNameAsync(string tagName);
        Task<IResult> AddTagAsync(PlcTagConfig tagConfig);
        Task<IResult> UpdateTagAsync(PlcTagConfig tagConfig);
        Task<IResult> DeleteTagAsync(int tagId);
    }
}
