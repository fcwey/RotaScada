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
    public class EfMalzemeDal : EfEntityRepositoryBase<Malzeme, Contexts.NorthwindContext>, IMalzemeDal
    {
        public MalzemeDto GetDtoById(int malzemeId)
        {
            using (var context = new NorthwindContext())
            {
                var result = (from Malzeme in context.Malzemeler
                              join MalzemeGrup in context.MalzemeGrups on Malzeme.MGrupId equals MalzemeGrup.Id
                              where Malzeme.Id == malzemeId
                              select new MalzemeDto()
                              {
                                  Id = Malzeme.Id,
                                  MGrupName = MalzemeGrup.GrupName,
                                  MalzemeName = Malzeme.MalzemeName,
                                  MaxSertlik = Malzeme.MaxSertlik

                              }).First();
                return result;


            }
        }

        public List<MalzemeDto> GetListMalzemeDtos()
        {

            using (var context = new NorthwindContext())
            {
                var result = (from Malzeme in context.Malzemeler
                              join MalzemeGrup in context.MalzemeGrups on Malzeme.MGrupId equals MalzemeGrup.Id
                              select new MalzemeDto()
                              {
                                  Id = Malzeme.Id,
                                  MGrupName= MalzemeGrup.GrupName,
                                  MalzemeName= Malzeme.MalzemeName,
                                  MaxSertlik=Malzeme.MaxSertlik

                              }).ToList();
                return result;


            }
            
        }
    }
}
