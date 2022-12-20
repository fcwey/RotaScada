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
    public interface IMusteriService
    {
        IDataResult<Musteri> GetById(int musteriId);
        IDataResult<List<Musteri>> GetList();
        IDataResult<List<MusteriDto>> GetListDto();
        IResult Add(Musteri musteri);
        IResult Delete(Musteri musteri);
        IResult Update(Musteri musteri);
    }
}
