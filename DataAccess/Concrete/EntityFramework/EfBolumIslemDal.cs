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
    public class EfBolumIslemDal : EfEntityRepositoryBase<BolumIslem, Contexts.NorthwindContext>, IBolumIslemDal
    {
        public List<BolumIslemDto> GetListBolumIslemDtos()
        {
            using (var context = new NorthwindContext())
            {
                var result = (from BolumIslem in context.BolumIslemler
                              join bolum in context.Bolumler on BolumIslem.BolumId equals bolum.Id
                              select new BolumIslemDto()
                              {
                                  Id = BolumIslem.Id,
                                  BolumName = bolum.Name,
                                  BolumIslemi = BolumIslem.BolumIslemi,
                                  Gun = BolumIslem.Gun

                              }).ToList();
                return result;


            }
        }

        public List<BolumIslem> GetListByBolumId(int bolumId)
        {
            using (var context = new NorthwindContext())
            {
                var result = (from BolumIslem in context.BolumIslemler
                              where BolumIslem.BolumId == bolumId
                              select new BolumIslem()
                              {
                                  Id = BolumIslem.Id,
                                  BolumId= BolumIslem.BolumId,
                                  BolumIslemi = BolumIslem.BolumIslemi,
                                  Gun= BolumIslem.Gun


                              }).ToList();
                return result;


            }
        }
    }
}
//using (var context = new NorthwindContext())
//{
//    var result = (from BolumIslem in context.BolumIslemler
//                  join bolum in context.Bolumler on BolumIslem.Id equals bolum.Id
//                  select new BolumIslemDto()
//                  {
//                      Id = BolumIslem.Id,
//                      BolumName = bolum.Name,
//                      BolumIslemi = BolumIslem.BolumIslemi,
//                      Gun = BolumIslem.Gun

//                  }).ToList();
//    return result;