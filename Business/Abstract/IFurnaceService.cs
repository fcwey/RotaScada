using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IFurnaceService
    {
        IDataResult<Furnace> GetById(int furnaceId);
        IDataResult<List<Furnace>> GetList();
        IDataResult<List<FurnaceDto>> GetListByBolumId(int bolumId);
        IDataResult<List<FurnaceDto>> GetListDto();
        IResult Add(Furnace aktifIslem);
        IResult Delete(Furnace aktifIslem);
        IResult Update(Furnace aktifIslem);
    }
}
