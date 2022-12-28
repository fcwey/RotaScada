using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IMalGirisMalzemeService
    {
        IDataResult<MalGirisMalzeme> GetById(int malGirisMalzemeId);
        IDataResult<List<MalGirisMalzeme>> GetList();

        IResult Add(MalGirisMalzeme malGirisMalzeme);
        IResult Delete(MalGirisMalzeme malGirisMalzeme);
        IResult Update(MalGirisMalzeme malGirisMalzeme);
    }
}
