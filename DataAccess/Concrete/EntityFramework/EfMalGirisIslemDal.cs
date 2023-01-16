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
    public class EfMalGirisIslemDal : EfEntityRepositoryBase<MalGirisIslem, Contexts.NorthwindContext>, IMalGirisIslemDal
    {
        public List<MalGirisIslemDto> GetListByBolumDtos(int BolumId, int MgirisId)
        {
            using (var context = new NorthwindContext())
            {
                var result = (from mgIslem in context.MalGirisIslemler
                              join mgMalzeme in context.MalGirisMalzemeler on mgIslem.MalGirisMalzemeId equals mgMalzeme.Id
                              join bolumislem in context.BolumIslemler on mgIslem.BolumIslemId equals bolumislem.Id
                              join bolum in context.Bolumler on bolumislem.BolumId equals bolum.Id
                              where mgMalzeme.BolumId == BolumId
                              where mgMalzeme.Id== MgirisId
                              select new MalGirisIslemDto()
                              {
                                  Id = mgIslem.Id,
                                  Agirlik = mgMalzeme.Agirlik,
                                  BolumIslemName = bolumislem.BolumIslemi,
                                  BirimFiyat = mgIslem.BirimFiyat,
                                  BolumName = bolum.Name
                              }).ToList();
                return result;
            }
        }
    }
}
