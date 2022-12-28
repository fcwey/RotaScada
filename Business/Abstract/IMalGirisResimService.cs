using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IMalGirisResimService
    {
        IDataResult<MalGirisResim> GetById(int malGirisResimId);
        IDataResult<List<MalGirisResim>> GetList();

        IResult Add(MalGirisResim malGirisResim);
        IResult Delete(MalGirisResim malGirisResim);
        IResult Update(MalGirisResim malGirisResim);
    }
}
