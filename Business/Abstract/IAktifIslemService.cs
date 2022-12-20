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
    public interface IAktifIslemService
    {
        IDataResult<AktifIslem> GetById(int islemId);
        IDataResult<List<AktifIslem>> GetList();
        IDataResult<List<AktifIslemDto>> GetListDto();
        IResult Add(AktifIslem aktifIslem);
        IResult Delete(AktifIslem aktifIslem);
        IResult Update(AktifIslem aktifIslem);
    }
}
