using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IServisService
    {
        IDataResult<Servis> GetById(int servisId);
        IDataResult<List<Servis>> GetList();
        // IDataResult<List<Category>> GetListByCotegory(int categoryId);
        IResult Add(Servis servis);
        IResult Delete(Servis servis);
        IResult Update(Servis servis);
    }
}
