using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IIslemService
    {
        IDataResult<Islem> GetById(int islemId);
        IDataResult<List<Islem>> GetList();
        IResult Add(Islem islem);
        IResult Delete(Islem islem);
        IResult Update(Islem islem);
    }
}
