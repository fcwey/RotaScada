using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class IslemManager :IIslemService
    {
        private IIslemDal _islemDal;
        public IslemManager(IIslemDal islemDal)
        { _islemDal = islemDal; }

        public IResult Add(Islem islem)
        {
            _islemDal.Add(islem);
            return new SuccessResult(Messages.ServisAdded);
        }

        public IResult Delete(Islem islem)
        {
            _islemDal.Delete(islem);
            return new SuccessResult(Messages.ServisDeleted);
        }

        public IDataResult<Islem> GetById(int islemId)
        {
            return new SuccessDataResult<Islem>(_islemDal.Get(p => p.Id == islemId));
        }

        public IDataResult<List<Islem>> GetList()
        {
            return new SuccessDataResult<List<Islem>>(_islemDal.GetList().ToList());
        }


        public IResult Update(Islem islem)
        {
            _islemDal.Update(islem);
            return new SuccessResult(Messages.ServisUpdated);
        }
    }
}
