using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfServisDal : EfEntityRepositoryBase<Servis, NorthwindContext>, IServisDal
    {
        public async Task<List<ServisDto>> GetListServisDtoAsync()
        {
            using (var context = new NorthwindContext())
            {
                var result = await (from Servis in context.Servisler
                                    orderby Servis.Name
                                    select new ServisDto()
                                    {
                                        Id = Servis.Id,
                                        Name = Servis.Name
                                    }).ToListAsync();
                return result;


            }
        }

        public List<ServisDto> GetListServisDtos()
        {
          
            using (var context = new NorthwindContext())
            {
                var result = (from Servis in context.Servisler
                              orderby Servis.Name
                             select new ServisDto()
                             {
                                 Id = Servis.Id,
                                 Name = Servis.Name
                             }).ToList();
                return result;


            }
        }
    }
}