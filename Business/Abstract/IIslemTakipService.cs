using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IIslemTakipService
    {
        IDataResult<IslemTakip> GetById(int islemTakipId);
        IDataResult<List<IslemTakip>> GetList();
        IResult Add(IslemTakip islem);
        IResult Delete(IslemTakip islem);
        IResult Update(IslemTakip islem);
    }
}
