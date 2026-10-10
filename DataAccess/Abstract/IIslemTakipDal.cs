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
    public interface IIslemTakipDal : IEntityRepository<IslemTakip>
    {
        Task AddFirstIslemTakipByMgKayit(MalGirisKayit malGirisKayit);
        Task<List<IslemTakipDto>> GetListDtoAsync();
        Task<List<IslemTakipDto>> GetListDtoAsync(int bolumId);
        Task<List<IslemTakipDto>> GetListDtoAsync(int bolumId,string TakipQuery);
        Task<List<IslemTakipDto>> GetListDtoByFurnaceAsync(int furnaceId);
        Task SendToFurnaceAsync(List<IslemTakipDto> liste,int AktifIslemId);
        Task SendToFurnaceStart(List<IslemTakipDto> liste); //durum: işlemde
        Task SendToFurnaceStop(List<IslemTakipDto> liste); //işlemi iptal et,geri al
        Task SendToFurnaceFinish(List<IslemTakipDto> liste, int AktifIslemId); //işlemi durum: işlem bitti
        Task SendToList(List<IslemTakipDto> liste, int ListeId); //listeye gönder.
        Task SendToKaliteKontrol(List<IslemTakipDto> liste);
        Task SendToStokCikis(List<IslemTakipDto> liste);
        Task SendToHistoryUpdateAsync(List<IslemTakipDto> liste);
    }
}
