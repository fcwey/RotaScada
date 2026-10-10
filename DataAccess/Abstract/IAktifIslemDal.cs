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
    public interface IAktifIslemDal : IEntityRepository<AktifIslem>
    {
        List<AktifIslemDto> GetListAktifIslemDtos();
        Task<List<AktifIslemDetailDto>> GetListAktifIslemDetailDtosAsync(int bolumId);
    }
}
