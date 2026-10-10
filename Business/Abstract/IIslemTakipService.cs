using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Constant;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IIslemTakipService
    {
        IDataResult<IslemTakip> GetById(int islemTakipId);
        IDataResult<List<IslemTakip>> GetList();
        Task<IDataResult<List<IslemTakip>>> GetListAsync();

        Task<IResult> SendToFurnaceAsync(List<IslemTakipDto> liste, int AktifIslemId);
        Task<IResult> SendToUnitAsync(List<IslemTakipDto> liste, UnitType unitType);

        Task AddFirstIslemTakipByMgKayit(MalGirisKayit malGirisKayit);
        Task<IDataResult<List<IslemTakipDto>>> GetListDtoAsync();
        Task<IDataResult<List<IslemTakipDto>>> GetListDtoAsync(int bolumId);
        Task<IDataResult<List<IslemTakipDto>>> GetListDtoAsync(int bolumId, string nerede);
        Task<IDataResult<List<IslemTakipDto>>> GetListDtoByFurnaceAsync(int furnaceId);
        IResult Add(IslemTakip islem);
        IResult Delete(IslemTakip islem);
        IResult Update(IslemTakip islem);
    }
}
