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
    public interface IFiyatService
    {
        IDataResult<Fiyat> GetById(int fiyatId);
        IDataResult<List<Fiyat>> GetList();
        IDataResult<List<FiyatDto>> GetListFiyatDtos();
        IResult Add(Fiyat fiyat);
        IResult Delete(Fiyat fiyat);
        IResult Update(Fiyat fiyat);
    }
}
