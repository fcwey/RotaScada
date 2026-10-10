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
    public interface IBolumIslemDal : IEntityRepository<BolumIslem>
    {
        List<BolumIslemDto> GetListBolumIslemDtos();
        Task<List<BolumIslemDto>> GetListBolumIslemDtosAsync();
        Task<List<BolumIslemDto>> GetListBolumIslemDtosAsync(int bolumId);

        List<BolumIslem> GetListByBolumId(int bolumId );
    }
}
