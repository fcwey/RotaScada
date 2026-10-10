using Core.DataAccess.EntityFramework;
using Core.Entities;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Entities.Constant;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.SqlServer.Server;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Data.SqlTypes;
//using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfIslemTakipDal : EfEntityRepositoryBase<IslemTakip, NorthwindContext>, IIslemTakipDal
    {
        public async Task AddFirstIslemTakipByMgKayit(MalGirisKayit malGirisKayit)
        {

            //malzeme listele
            //listelenen malzemelerde işlem takip varmı diye bak
            //olanlara takip ekle
            using (var context = new NorthwindContext())
            {
                var result = await (from mgMalzemeler in context.MalGirisMalzemeler
                                    where mgMalzemeler.MalGirisId == malGirisKayit.Id
                                    where context.IslemTakips.Count(p => p.MalGirisMalzemeId == mgMalzemeler.Id) == 0
                                    select new MalGirisMalzeme()).ToListAsync();

                foreach (var item in result)
                {

                    IslemTakip islemTakip = new IslemTakip();
                    islemTakip.MalGirisMalzemeId = item.Id;
                    islemTakip.AktifIslemId = 0;
                    islemTakip.KulId = malGirisKayit.UserId == null ? 0 : (int)malGirisKayit.UserId;
                    islemTakip.Active = 1;
                    islemTakip.BaslamaTarihi = DateTime.Now;
                    islemTakip.ListeId = 0;
                    islemTakip.Durum = TakipStatus.Durum.Bekliyor;
                    islemTakip.Nerede = TakipStatus.Nerede.StokGiris;
                    await context.IslemTakips.AddAsync(islemTakip);
                }
                await context.SaveChangesAsync();
            }


        }

        //Asenkron dto listelemeler

        public async Task<List<IslemTakipDto>> GetListDtoAsync()
        {
            using (var context = new NorthwindContext())
            {
                var result = await (from islemTakip in context.IslemTakips
                                    join malGirisMalzeme in context.MalGirisMalzemeler on islemTakip.MalGirisMalzemeId equals malGirisMalzeme.Id
                                    join mgKayit in context.MalGirisKayitlar on malGirisMalzeme.MalGirisId equals mgKayit.Id
                                    join firma in context.Musteriler on mgKayit.FirmaId equals firma.Id
                                    join malzeme in context.Malzemeler on malGirisMalzeme.MalzemeId equals malzeme.Id
                                    join mgrup in context.MalzemeGrups on malzeme.MGrupId equals mgrup.Id
                                    where islemTakip.Active == 1
                                    select new IslemTakipDto()
                                    {
                                        Id = islemTakip.Id,
                                        Adet = malGirisMalzeme.Adet.ToString(),
                                        Agirlik = malGirisMalzeme.Agirlik.ToString(),
                                        Durum = islemTakip.Durum,
                                        FirmaAdi = firma.FirmaAd,
                                        GirisTarihi = DateTime.Now,//   (DateTime)mgKayit.GirisTarihi,
                                        Nerede = islemTakip.Nerede,
                                        MalzemeAdi = malzeme.MalzemeName,
                                        MGrupAdi = mgrup.GrupName,
                                        IstenenSertlik = malGirisMalzeme.Sertlik,
                                        IslemBaslik = "denemede",
                                        IsSelected=false
                                        
                                    }).ToListAsync();
                return result;


            }
        }
        public async Task<List<IslemTakipDto>> GetListDtoAsync(int bolumId)
        {
            using (var context = new NorthwindContext())
            {
                var result = await (from islemTakip in context.IslemTakips
                                    join malGirisMalzeme in context.MalGirisMalzemeler on islemTakip.MalGirisMalzemeId equals malGirisMalzeme.Id
                                    join mgKayit in context.MalGirisKayitlar on malGirisMalzeme.MalGirisId equals mgKayit.Id

                                    join firma in context.Musteriler on mgKayit.FirmaId equals firma.Id
                                    join malzeme in context.Malzemeler on malGirisMalzeme.MalzemeId equals malzeme.Id
                                    join mgrup in context.MalzemeGrups on malzeme.MGrupId equals mgrup.Id
                                    join bolum in context.Bolumler on malGirisMalzeme.BolumId equals bolum.Id
                                    where islemTakip.Active == 1
                                    where bolum.Id == bolumId
                                    select new IslemTakipDto()
                                    {
                                        Id = islemTakip.Id,
                                        Adet = malGirisMalzeme.Adet.ToString(),
                                        Agirlik = malGirisMalzeme.Agirlik.ToString(),
                                        Durum = islemTakip.Durum,
                                        FirmaAdi = firma.FirmaAd,
                                        GirisTarihi = (DateTime)mgKayit.GirisTarihi,
                                        Nerede = islemTakip.Nerede,
                                        MalzemeAdi = malzeme.MalzemeName,
                                        MGrupAdi = mgrup.GrupName,
                                        IstenenSertlik = malGirisMalzeme.Sertlik,
                                        IslemBaslik = "denemede"

                                    }).ToListAsync();
                return result;


            }
        }
        //bolüm id ve nerede ise listele,  
        public async Task<List<IslemTakipDto>> GetListDtoAsync(int bolumId, string nerede)
        {
            using (var context = new NorthwindContext())
            {
                var result = await (from islemTakip in context.IslemTakips
                                    join malGirisMalzeme in context.MalGirisMalzemeler on islemTakip.MalGirisMalzemeId equals malGirisMalzeme.Id
                                    join mgKayit in context.MalGirisKayitlar on malGirisMalzeme.MalGirisId equals mgKayit.Id

                                    join firma in context.Musteriler on mgKayit.FirmaId equals firma.Id
                                    join malzeme in context.Malzemeler on malGirisMalzeme.MalzemeId equals malzeme.Id
                                    join mgrup in context.MalzemeGrups on malzeme.MGrupId equals mgrup.Id
                                    join bolum in context.Bolumler on malGirisMalzeme.BolumId equals bolum.Id

                                    //join aktifislem in context.AktifIslemler on islemTakip.AktifIslemId equals aktifislem.Id
                                   // join furnace in context.Furnaces on aktifislem.FurnaceId equals furnace.Id

                                    where islemTakip.Active == 1
                                    where bolum.Id == bolumId
                                    where islemTakip.Nerede == nerede //liste olursa

                                    select new IslemTakipDto()
                                    {
                                        Id = islemTakip.Id,
                                        Adet = malGirisMalzeme.Adet.ToString(),
                                        Agirlik = malGirisMalzeme.Agirlik.ToString(),
                                        Durum = islemTakip.Durum,
                                        FirmaAdi = firma.FirmaAd,
                                        GirisTarihi = SqlDateTime.MinValue.Value,
                                        Nerede = islemTakip.Nerede == TakipStatus.Nerede.Islem ? (islemTakip.AktifIslemId > 0 ? "aktifislem.Baslik" : TakipStatus.Nerede.Liste) : islemTakip.Nerede,
                                        MalzemeAdi = malzeme.MalzemeName,
                                        MGrupAdi = mgrup.GrupName,
                                        IstenenSertlik = malGirisMalzeme.Sertlik,
                                        IslemBaslik = islemTakip.Nerede == TakipStatus.Nerede.Islem || islemTakip.AktifIslemId > 0 ? "furnace.Name ": "liste Id gir" //fırın adını yada liste adını yazacağız.
                                    }).ToListAsync();
                return result;


            }
        }



        //fırın id si ne gore liste al. fırın malzeme listesi için
        public async Task<List<IslemTakipDto>> GetListDtoByFurnaceAsync(int furnaceId)
        {
            using (var context = new NorthwindContext())
            {
                var result = await(from islemTakip in context.IslemTakips
                                   join malGirisMalzeme in context.MalGirisMalzemeler on islemTakip.MalGirisMalzemeId equals malGirisMalzeme.Id
                                   join mgKayit in context.MalGirisKayitlar on malGirisMalzeme.MalGirisId equals mgKayit.Id
                                   join aktif in context.AktifIslemler on islemTakip.AktifIslemId equals aktif.Id
                                   join firma in context.Musteriler on mgKayit.FirmaId equals firma.Id
                                   join malzeme in context.Malzemeler on malGirisMalzeme.MalzemeId equals malzeme.Id
                                   join mgrup in context.MalzemeGrups on malzeme.MGrupId equals mgrup.Id
                                   where islemTakip.Active == 1
                                   where aktif.FurnaceId== furnaceId
                                   select new IslemTakipDto()
                                   {
                                       Id = islemTakip.Id,
                                       Adet = malGirisMalzeme.Adet.ToString(),
                                       Agirlik = malGirisMalzeme.Agirlik.ToString(),
                                       Durum = islemTakip.Durum,
                                       FirmaAdi = firma.FirmaAd,
                                       GirisTarihi = DateTime.Now,
                                       Nerede = islemTakip.Nerede,
                                       MalzemeAdi = malzeme.MalzemeName,
                                       MGrupAdi = mgrup.GrupName,
                                       IstenenSertlik = malGirisMalzeme.Sertlik,
                                       IslemBaslik = "denemede"

                                   }).ToListAsync();
                return result;


            }
        }

        //Liste olarak alınan malları aktif işlem id sine eşitle, o fırına  ve işleme yolla
        public async Task SendToFurnaceAsync(List<IslemTakipDto> liste, int AktifIslemId)
        {
            // add range sadeliste  
            using (var context = new NorthwindContext())
            {

                foreach (var item in liste)
                {
                    var takip = await context.IslemTakips.Where(p => p.Id == item.Id).SingleOrDefaultAsync();
                    takip.Id = 0; 
                    takip.BaslamaTarihi = DateTime.Now;
                    takip.BitimTarihi = SqlDateTime.MinValue.Value;
                    takip.Active = 1;
                    takip.AktifIslemId = AktifIslemId;
                    takip.Nerede = TakipStatus.Nerede.Islem;
                    takip.Durum = TakipStatus.Durum.ProseseHazir;
                    context.IslemTakips.Add(takip);
                    await context.SaveChangesAsync();
                }
            }
        }

        public Task SendToFurnaceFinish(List<IslemTakipDto> liste, int AktifIslemId)
        {
            throw new NotImplementedException();
        }

        public Task SendToFurnaceStart(List<IslemTakipDto> liste)
        {
            throw new NotImplementedException();
        }

        public Task SendToFurnaceStop(List<IslemTakipDto> liste)
        {
            throw new NotImplementedException();
        }

        public async Task SendToHistoryUpdateAsync(List<IslemTakipDto> liste)
        {
            using (var context = new NorthwindContext())
            {

                foreach (var item in liste)
                {
                    var takip = await context.IslemTakips.Where(p => p.Id == item.Id).SingleOrDefaultAsync();
                    takip.BitimTarihi = DateTime.Now;
                    takip.Active = 0;
                    context.IslemTakips.Update(takip);
                    await context.SaveChangesAsync();
                }
            }
            //throw new NotImplementedException();

        }

        public Task SendToKaliteKontrol(List<IslemTakipDto> liste)
        {
            throw new NotImplementedException();
        }

        public Task SendToList(List<IslemTakipDto> liste, int ListeId)
        {
            throw new NotImplementedException();
        }

        public Task SendToStokCikis(List<IslemTakipDto> liste)
        {
            throw new NotImplementedException();
        }
    }
}