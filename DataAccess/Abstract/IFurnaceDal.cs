using Core.DataAccess;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IFurnaceDal : IEntityRepository<Furnace>
    {
        Task<List<FurnaceDto>> GetListFurnaceDtosAsync();
        Task<List<FurnaceDetailDto>> GetListFurnaceDetailDtosAsync();
        Task<List<FurnaceDetailDto>> GetListFurnaceDetailDtosAsync(int bolumId);
        List<FurnaceDto> GetListFurnaceDtos();
        List<FurnaceDto> GetListByBolumId(int bolumId);
    }
}
