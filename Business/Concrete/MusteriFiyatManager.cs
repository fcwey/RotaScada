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
    public class MusteriFiyatManager:IMusteriFiyatService
    {
        private IMusteriFiyatDal _musteriFiyatDal;
        public MusteriFiyatManager(IMusteriFiyatDal musteriFiyatDal)
        { _musteriFiyatDal = musteriFiyatDal; }

        public IDataResult<MusteriFiyat> GetById(int musteriFiyatId)
        {
            return new SuccessDataResult<MusteriFiyat>(_musteriFiyatDal.Get(p => p.Id == musteriFiyatId));
        }

        public IDataResult<List<MusteriFiyat>> GetList()
        {
            return new SuccessDataResult<List<MusteriFiyat>>(_musteriFiyatDal.GetList().ToList());
        }

        public IResult Add(MusteriFiyat musteriFiyat)
        {
            _musteriFiyatDal.Add(musteriFiyat);
            return new SuccessResult(Messages.ServisAdded);
        }

        public IResult Delete(MusteriFiyat musteriFiyat)
        {
            _musteriFiyatDal.Delete(musteriFiyat);
            return new SuccessResult(Messages.ServisDeleted);
        }



        public IResult Update(MusteriFiyat musteriFiyat)
        {
            _musteriFiyatDal.Update(musteriFiyat);
            return new SuccessResult(Messages.ServisUpdated);
        }
    }
}
