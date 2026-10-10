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
    public interface IMalGirisKayitService
    {
        IDataResult<MalGirisKayit> GetById(int malGirisKayitId);
        IDataResult<List<MalGirisKayit>> GetList();
        Task<IResult> UpdateForTakip(MalGirisKayit malGirisKayit);
        IResult Add(MalGirisKayit malGirisKayit);
        IResult Delete(MalGirisKayit malGirisKayit);
        IResult Update(MalGirisKayit malGirisKayit);
    }
}
