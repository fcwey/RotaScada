using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IBolumService
    {

        IDataResult<Bolum> GetById(int bolumId);
        IDataResult<List<Bolum>> GetList();
        Result ServisExist(Bolum bolum);
        IResult Add(Bolum bolum);
        IResult Delete(Bolum bolum);
        IResult Update(Bolum bolum);
    }
}
