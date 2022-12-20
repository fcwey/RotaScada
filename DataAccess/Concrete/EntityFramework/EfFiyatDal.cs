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
    public class EfFiyatDal : EfEntityRepositoryBase<Fiyat, NorthwindContext>, IFiyatDal
    {
        public List<FiyatDto> GetListFiyatDtos()
        {
            using (var context = new NorthwindContext())
            {
                var result = (from fiyat in context.Fiyatlar
                              join bolumislem in context.BolumIslemler on fiyat.BolumIslemId equals bolumislem.Id
                              join bolum in context.Bolumler on bolumislem.BolumId equals bolum.Id
                              join mgrup in context.MalzemeGrups on fiyat.MGrupId equals mgrup.Id
                              select new FiyatDto()
                              {

                                  Id = fiyat.Id,
                                  BolumName = bolum.Name,
                                  BolumIslemName = bolumislem.BolumIslemi,
                                  MGrupName = mgrup.GrupName,
                                  Fiyati = fiyat.Fiyati

                              }).ToList();
                return result;


            }
        }
    }
}
