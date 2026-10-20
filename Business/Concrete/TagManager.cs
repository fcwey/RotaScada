using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Concrete
{

    public class TagManager : ITagService
    {
        // Tag bilgileri DB'den veya in-memory konfigürasyondan yüklenebilir
        private readonly List<PlcTagConfig> _tags = new List<PlcTagConfig>();

        public async Task<IDataResult<List<PlcTagConfig>>> GetAllTagsAsync()
        {
            return await Task.FromResult(new SuccessDataResult<List<PlcTagConfig>>(_tags));
        }

        public async Task<IDataResult<PlcTagConfig>> GetTagByNameAsync(string tagName)
        {
            var tag = _tags.Find(t => t.TagName == tagName);
            if (tag == null)
                return await Task.FromResult(new ErrorDataResult<PlcTagConfig>("Tag bulunamadı."));

            return await Task.FromResult(new SuccessDataResult<PlcTagConfig>(tag));
        }

        public async Task<IResult> AddTagAsync(PlcTagConfig tagConfig)
        {
            _tags.Add(tagConfig);
            return await Task.FromResult(new SuccessResult("Tag başarıyla eklendi."));
        }

        public async Task<IResult> UpdateTagAsync(PlcTagConfig tagConfig)
        {
            var index = _tags.FindIndex(t => t.Id == tagConfig.Id);
            if (index == -1)
                return await Task.FromResult(new ErrorResult("Güncellenecek Tag bulunamadı."));

            _tags[index] = tagConfig;
            return await Task.FromResult(new SuccessResult("Tag güncellendi."));
        }

        public async Task<IResult> DeleteTagAsync(int tagId)
        {
            _tags.RemoveAll(t => t.Id == tagId);
            return await Task.FromResult(new SuccessResult("Tag silindi."));
        }
    }
}