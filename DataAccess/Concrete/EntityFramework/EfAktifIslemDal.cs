using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfAktifIslemDal : EfEntityRepositoryBase<AktifIslem, NorthwindContext>, IAktifIslemDal
    {
        public async Task<List<AktifIslemDetailDto>> GetListAktifIslemDetailDtosAsync(int bolumId)
        {
            using (var context = new NorthwindContext())
            {
                var result = await (from aktifIslem in context.AktifIslemler
                              join bolum in context.Bolumler on aktifIslem.BolumId equals bolum.Id
                              join islem in context.Islemler on aktifIslem.IslemId equals islem.Id
                              join furnace in context.Furnaces on aktifIslem.FurnaceId equals furnace.Id
                              where bolum.Id==bolumId
                              where context.IslemTakips.Join(context.AktifIslemler,
                                                                            takip => takip.AktifIslemId,
                                                                            akt => akt.Id,
                                                                            (takip, akt) => new { Takip = takip, Akt = akt })
                                                                             .Where(p => p.Akt.FurnaceId == furnace.Id).Where(p => p.Takip.Active == 1).Count()>0
                                                                             ? context.IslemTakips.Join(context.AktifIslemler,
                                                                                         takip => takip.AktifIslemId,
                                                                                             akt => akt.Id,
                                                                                            (takip, akt) => new { Takip = takip, Akt = akt }) 
                                                                                            .Where(p => p.Akt.FurnaceId == furnace.Id).Where(p => p.Takip.Active == 1).First().Akt.IslemId == islem.Id
                                                                                        ? true 
                                                                                        : false
                                                                             : true
                                    select new AktifIslemDetailDto()
                              {
                                  Id = aktifIslem.Id,
                                  IslemName = islem.Name,
                                  FurnaceName = furnace.Name,
                                  Adet=1,
                                  Agirlik=2,
                                  Status=1
                                  
                                  
                              }).ToListAsync();
                return result;


            }
        }

        public List<AktifIslemDto> GetListAktifIslemDtos()
        {

            using (var context = new NorthwindContext())
            {
                var result = (from aktifIslem in context.AktifIslemler
                              join bolum in context.Bolumler on aktifIslem.BolumId equals bolum.Id
                              join islem in context.Islemler on aktifIslem.IslemId equals islem.Id
                              join furnace in context.Furnaces on aktifIslem.FurnaceId equals furnace.Id
                              orderby aktifIslem.IslemId
                              select new AktifIslemDto()
                              {
                                  Id= aktifIslem.Id,
                                  BolumName=bolum.Name,
                                  IslemName=islem.Name,
                                  FurnaceName=furnace.Name
                              }).ToList();
                return result;


            }
        }
    }
}
