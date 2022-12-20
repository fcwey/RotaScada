using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IMusteriFiyatService
    {
        IDataResult<MusteriFiyat> GetById(int musteriFiyatId);
        IDataResult<List<MusteriFiyat>> GetList();
        IResult Add(MusteriFiyat musteriFiyat);
        IResult Delete(MusteriFiyat musteriFiyat);
        IResult Update(MusteriFiyat musteriFiyat);
    }
}
