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
    public class EfAktifIslemDal : EfEntityRepositoryBase<AktifIslem, NorthwindContext>, IAktifIslemDal
    {
        public List<AktifIslemDto> GetListAktifIslemDtos()
        {

            using (var context = new NorthwindContext())
            {
                var result = (from aktifIslem in context.AktifIslemler
                              join bolum in context.Bolumler on aktifIslem.BolumId equals bolum.Id
                              join islem in context.Islemler on aktifIslem.IslemId equals islem.Id
                              orderby aktifIslem.IslemId
                              select new AktifIslemDto()
                              {
                                  Id= aktifIslem.Id,
                                  BolumName=bolum.Name,
                                  IslemName=islem.Name,
                                  Baslik=aktifIslem.Baslik,
                                  Dakika=aktifIslem.Dakika,
                                  Derece=aktifIslem.Derece
                              }).ToList();
                return result;


            }
        }
    }
}
