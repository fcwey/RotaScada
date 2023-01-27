using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using FluentValidation;
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
    }
}
