using Core.DataAccess.EntityFramework;
using Core.Entities;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Entities.Constant;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfMalGirisKayitDal : EfEntityRepositoryBase<MalGirisKayit, Contexts.NorthwindContext>, IMalGirisKayitDal
    {
        public bool IslemIsCorrect(int malGirisKayitId)
        {

            //Her malzemenin işlemi varmı?
            //Önce malzemeleri listele
            //sonra listeli malzemelerin işlemlerini
            using (var context = new NorthwindContext())
            {

                var List = (from mgMalzeme in context.MalGirisMalzemeler
                            where mgMalzeme.MalGirisId == malGirisKayitId
                            select new MalGirisMalzeme()
                            {
                                Id = mgMalzeme.Id,


                            }).ToList();
                foreach (var item in List)
                {
                    using (var context2 = new NorthwindContext())
                    {
                        var islemAny = (from mgislem in context2.MalGirisIslemler
                                        where mgislem.MalGirisMalzemeId == item.Id
                                        select new MalGirisIslem()
                                        {
                                            Id = mgislem.Id

                                        }).ToList().Any();
                        if (islemAny)
                        {
                            return islemAny;
                        }


                    }
                }

                return false;


            }
        }

        public bool MalzemeIsCorrect(int malGirisKayitId)
        {
            using (var context = new NorthwindContext())
            {
                var result = (from mgMalzemeler in context.MalGirisMalzemeler
                              where mgMalzemeler.MalGirisId == malGirisKayitId
                              select new MalGirisMalzeme()
                              {
                                  Id = mgMalzemeler.Id


                              }).ToList().Any();
                return result;


            }
        }
        public async Task UpdateForTakip(MalGirisKayit malGirisKayit)
        {

            //malzeme listele
            //listelenen malzemelerde işlem takip varmı diye bak
            //olanlara takip ekle
            using (var context = new NorthwindContext())
            {
                //var updatedEntity = context.Entry(malGirisKayit);
                //updatedEntity.State = EntityState.Modified;
                //await context.SaveChangesAsync();
                context.Update(malGirisKayit);
                await context.SaveChangesAsync();
                var result = await (from mgMalzemeler in context.MalGirisMalzemeler
                                    where mgMalzemeler.MalGirisId == malGirisKayit.Id
                                    where context.IslemTakips.Count(p => p.MalGirisMalzemeId == mgMalzemeler.Id) == 0
                                    select mgMalzemeler).ToListAsync();

                foreach (var item in result)
                {

                    IslemTakip islemTakip = new IslemTakip();
                    islemTakip.MalGirisMalzemeId = item.Id;
                    islemTakip.AktifIslemId = 0;
                    islemTakip.KulId = malGirisKayit.UserId == null ? 0 : (int)malGirisKayit.UserId;
                    islemTakip.Active = 1;
                    islemTakip.BaslamaTarihi = DateTime.Now;
                    islemTakip.BitimTarihi= DateTime.Now;
                    islemTakip.ListeId = 0;
                    islemTakip.Durum = TakipStatus.Durum.Bekliyor;
                    islemTakip.Nerede = TakipStatus.Nerede.StokGiris;
                    //var deletedEntity = context.Entry(islemTakip);
                    //deletedEntity.State = EntityState.Added;
                    //context.SaveChanges();
                    await context.AddAsync(islemTakip);
                    await context.SaveChangesAsync();
                }

                //context.IslemTakips.AddRange(list);
                //context.SaveChanges();
            }
        }
    }
}
