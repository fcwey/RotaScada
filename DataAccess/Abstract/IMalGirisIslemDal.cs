using Core.DataAccess;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IMalGirisIslemDal : IEntityRepository<MalGirisIslem>
    {
        List<MalGirisIslemDto> GetListByBolumDtos(int BolumId, int MgirisId);
    }
}
