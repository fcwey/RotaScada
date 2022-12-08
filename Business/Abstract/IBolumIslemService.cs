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
    public interface IBolumIslemService
    {
        IDataResult<BolumIslem> GetById(int bolumIslemId);
        IDataResult<List<BolumIslem>> GetList();
        IDataResult<List<BolumIslemDto>> GetListBolumIslemDtos();
        Result ServisExist(BolumIslem bolumIslem);
        IResult Add(BolumIslem bolumIslem);
        IResult Delete(BolumIslem bolumIslem);
        IResult Update(BolumIslem bolumIslem);
    }
}
