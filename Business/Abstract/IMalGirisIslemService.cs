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
    public interface IMalGirisIslemService
    {
        IDataResult<MalGirisIslem> GetById(int malGirisIslemId);
        IDataResult<List<MalGirisIslem>> GetList();
        IDataResult<List<MalGirisIslemDto>> GetListByDto(int BolumId, int MgirisId);
        IResult DeleteListByMgMalzemeId(int MalGMalzemeId);
        IResult DeleteListByMGirisMgKayitId(int mgKayitId);
        IResult Add(MalGirisIslem malGirisIslem);
        IResult Delete(MalGirisIslem malGirisIslem);
        IResult Update(MalGirisIslem malGirisIslem);
    }
}
