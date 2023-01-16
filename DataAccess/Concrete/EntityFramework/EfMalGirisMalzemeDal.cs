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
    public class EfMalGirisMalzemeDal : EfEntityRepositoryBase<MalGirisMalzeme, Contexts.NorthwindContext>, IMalGirisMalzemeDal
    {
        public List<MalGirisMalzemeDto> GetListByBolumDtos(int BolumId, int MgirisId)
        {
            using (var context = new NorthwindContext())
            {
                var result = (from mgMalzemeler in context.MalGirisMalzemeler
                              join malzemeler in context.Malzemeler on mgMalzemeler.MalzemeId equals malzemeler.Id
                              join mGrup in context.MalzemeGrups on malzemeler.MGrupId equals mGrup.Id
                              where mgMalzemeler.BolumId == BolumId
                              where mgMalzemeler.MalGirisId== MgirisId
                              select new MalGirisMalzemeDto()
                              {
                                  Id= mgMalzemeler.Id,
                                  Adet=mgMalzemeler.Adet,
                                  Agirlik=mgMalzemeler.Agirlik,
                                  Sertlik = mgMalzemeler.Sertlik,
                                  MalzemeGrupName= mGrup.GrupName,
                                  MalzemeStandartNo= malzemeler.MalzemeName


                              }).ToList();
                return result;


            }
        }
    }
}
