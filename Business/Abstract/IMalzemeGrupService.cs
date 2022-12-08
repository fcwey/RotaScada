using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IMalzemeGrupService
    {
        IDataResult<MalzemeGrup> GetById(int malzemeGrupId);
        IDataResult<List<MalzemeGrup>> GetList();
        IResult Add(MalzemeGrup malzemeGrup);
        IResult Delete(MalzemeGrup malzemeGrup);
        IResult Update(MalzemeGrup malzemeGrup);
    }
}
