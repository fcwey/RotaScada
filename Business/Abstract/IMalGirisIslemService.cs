using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IMalGirisIslemService
    {
        IDataResult<MalGirisIslem> GetById(int malGirisIslemId);
        IDataResult<List<MalGirisIslem>> GetList();

        IResult Add(MalGirisIslem malGirisIslem);
        IResult Delete(MalGirisIslem malGirisIslem);
        IResult Update(MalGirisIslem malGirisIslem);
    }
}
