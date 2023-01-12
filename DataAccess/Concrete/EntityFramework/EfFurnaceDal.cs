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
    }
}
