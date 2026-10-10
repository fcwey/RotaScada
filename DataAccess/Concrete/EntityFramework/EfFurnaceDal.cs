using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfFurnaceDal : EfEntityRepositoryBase<Furnace, NorthwindContext>, IFurnaceDal
    {
        public List<FurnaceDto> GetListByBolumId(int bolumId)
        {
            using (var context = new NorthwindContext())
            {
                var result = (from furnace in context.Furnaces
                              join bolum in context.Bolumler on furnace.BolumId equals bolum.Id
                              where furnace.BolumId == bolumId
                              select new FurnaceDto()
                              {
                                  Id = furnace.Id,
                                  BolumAdi = bolum.Name,
                                  Derece = furnace.Derece,
                                  Name = furnace.Name


                              }).ToList();
                return result;


            }
        }

        async Task<List<FurnaceDto>> IFurnaceDal.GetListFurnaceDtosAsync()
        {

            using (var context = new NorthwindContext())
            {
                var result = await (from furnace in context.Furnaces
                                    join bolum in context.Bolumler on furnace.BolumId equals bolum.Id
                                    orderby bolum.Id
                                    select new FurnaceDto()
                                    {
                                        Id = furnace.Id,
                                        Name = furnace.Name,
                                        BolumAdi = bolum.Name,
                                        Derece = furnace.Derece
                                    }).ToListAsync();

                return result;


            }
        }
        List<FurnaceDto> IFurnaceDal.GetListFurnaceDtos()
        {

            using (var context = new NorthwindContext())
            {
                var result = (from furnace in context.Furnaces
                              join bolum in context.Bolumler on furnace.BolumId equals bolum.Id
                              orderby bolum.Id
                              select new FurnaceDto()
                              {
                                  Id = furnace.Id,
                                  Name = furnace.Name,
                                  BolumAdi = bolum.Name,
                                  Derece = furnace.Derece
                              }).ToList();
                return result;


            }
        }

        public async Task<List<FurnaceDetailDto>> GetListFurnaceDetailDtosAsync()
        {
            using (var context = new NorthwindContext())
            {
                var result = await (from furnace in context.Furnaces
                                        //join bolum in context.Bolumler on furnace.BolumId equals bolum.Id
                                        //join aktifislem in context.AktifIslemler on furnace.Id equals aktifislem.FurnaceId
                                        //join islemTakip in context.IslemTakips on aktifislem.Id equals islemTakip.AktifIslemId
                                        //join islem in context.bo
                                    select new FurnaceDetailDto()
                                    {
                                        Id = furnace.Id,
                                        FurnaceName = furnace.Name,
                                        //Adet = context.IslemTakips.Count(p => p.Active == 1 && p.AktifIslemId == aktifislem.Id),
                                        //Status= context.IslemTakips.Count(p=>p.Active==1 && p.AktifIslemId == aktifislem.Id)>0 ? 1:0


                                    }).ToListAsync();

                return result;


            }
        }

        public async Task<List<FurnaceDetailDto>> GetListFurnaceDetailDtosAsync(int bolumId)
        {
            using (var context = new NorthwindContext())
            {

                var result = await (from furnace in context.Furnaces
                                    
                                        //join bolum in context.Bolumler on furnace.BolumId equals bolum.Id
                                        //join aktifislem in context.AktifIslemler on furnace.Id equals aktifislem.FurnaceId
                                        //join islemTakip in context.IslemTakips on aktifislem.Id equals islemTakip.AktifIslemId
                                        //join islem in context.bo
                                    where furnace.BolumId == bolumId
                                    select new FurnaceDetailDto()
                                    {
                                        Id = furnace.Id,
                                        FurnaceName = furnace.Name,
                                        Adet =  context.IslemTakips.Join(context.AktifIslemler,
                                                                            takip => takip.AktifIslemId,
                                                                            akt => akt.Id,
                                                                            (takip, akt) => new { Takip = takip, Akt = akt })
                                                                             .Where(p => p.Akt.FurnaceId == furnace.Id ).Where(p=>p.Takip.Active==1).Count(),
                                        /// Status= context.IslemTakips.Count(p=>p.Active==1 && p.AktifIslemId>0)>0 ? 1:2
                                        Status=context.IslemTakips.Join(context.AktifIslemler,
                                                                            takip => takip.AktifIslemId, 
                                                                            akt => akt.Id,
                                                                            (takip, akt) => new { Takip = takip, Akt = akt })
                                                                             .Where(p => p.Akt.FurnaceId == furnace.Id).Where(p => p.Takip.Active == 1).Count() >0 ? 1:0,
                                        Kg= context.IslemTakips.Join(context.AktifIslemler,
                                                                            takip => takip.AktifIslemId,
                                                                            akt => akt.Id,
                                                                            (takip, akt) => new { Takip = takip, Akt = akt })
                                                                  .Where(p => p.Akt.FurnaceId == furnace.Id)
                                                                .Where(p => p.Takip.Active == 1)
                                                                .Join(context.MalGirisMalzemeler,
                                                                            tak => tak.Takip.MalGirisMalzemeId,
                                                                            mal => mal.Id,
                                                                            (tak, mal) => new { Tak = tak, Mal = mal })
                                                                
                                                                .Sum(i=>i.Mal.Agirlik),
                                        IslemAdı= context.IslemTakips.Join(context.AktifIslemler,
                                                                            takip => takip.AktifIslemId,
                                                                            akt => akt.Id,
                                                                            (takip, akt) => new { Takip = takip, Akt = akt })
                                                                             .Where(p => p.Akt.FurnaceId == furnace.Id).Where(p => p.Takip.Active == 1).Count()>0

                                                                             ? context.IslemTakips.Join(context.AktifIslemler,
                                                                            takip => takip.AktifIslemId,
                                                                            akt => akt.Id,
                                                                            (takip, akt) => new { Takip = takip, Akt = akt })
                                                                             .Where(p => p.Akt.FurnaceId == furnace.Id).Where(p => p.Takip.Active == 1)
                                                                             .Join(context.Islemler,
                                                                            akt =>akt.Akt.IslemId,
                                                                            isl => isl.Id,
                                                                            (akt, isl) => new { Aktif = akt, Islem = isl }).First().Islem.Name

                                                                             : "BOŞ"






                                    }).ToListAsync();

                return result;
               
            }
        }
        
    }
    
}
