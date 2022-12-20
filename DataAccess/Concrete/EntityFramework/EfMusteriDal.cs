using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfMusteriDal : EfEntityRepositoryBase<Musteri, NorthwindContext>, IMusteriDal
    {
        public List<MusteriDto> GetListMusteriDtos()
        {


            using (var context = new NorthwindContext())
            {
                var result = (from musteri in context.Musteriler
                              orderby musteri.FirmaAd
                              select new MusteriDto()
                              {
                                  Id = musteri.Id,
                                  FirmaAd = musteri.FirmaAd,
                                  Adres=musteri.Adres,
                                  Telefon=musteri.Telefon,
                                  Yetkili=musteri.Yetkili,
                              }).ToList();
                return result;


            }

        }
    }
}
