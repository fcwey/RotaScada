using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IMalzemeService
    {
        IDataResult<Malzeme> GetById(int malzemeId);
        IDataResult<MalzemeDto> GetDtoById(int malzemeId);
        IDataResult<List<Malzeme>> GetList();

        IDataResult<List<MalzemeDto>> GetListMalzemeDtos();
        IResult Add(Malzeme malzeme);
        IResult Delete(Malzeme malzeme);
        IResult Update(Malzeme malzeme);
    }
}
