using Core.DataAccess;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IMalzemeDal : IEntityRepository<Malzeme>
    {
        List<MalzemeDto> GetListMalzemeDtos();
        MalzemeDto GetDtoById(int malzemeId);
    }

}