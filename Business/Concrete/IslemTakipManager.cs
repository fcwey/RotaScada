using Business.Abstract;
using Business.Constants;
using Core.Utilities.Business;
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
    public class IslemTakipManager:IIslemTakipService
    {

        private IIslemTakipDal _islemTakipDal;
        public IslemTakipManager(IIslemTakipDal islemTakipDal)
        { _islemTakipDal = islemTakipDal; }

        public IResult Add(IslemTakip islemTakip)
        {
            _islemTakipDal.Add(islemTakip);
            return new SuccessResult(Messages.ServisAdded);
        }

        public IResult Delete(IslemTakip islemTakip)
        {
            IResult result = BusinessRules.Run(CheckIfIslemTakipExists(islemTakip.Id));

            if (result != null)
            {
                return result;
            }
            _islemTakipDal.Delete(islemTakip);
            return new SuccessResult(Messages.ServisDeleted);
        }

        public IDataResult<IslemTakip> GetById(int islemTakipId)
        {
            return new SuccessDataResult<IslemTakip>(_islemTakipDal.Get(p => p.Id == islemTakipId));
        }


        public IDataResult<List<IslemTakip>> GetList()
        {
            return new SuccessDataResult<List<IslemTakip>>(_islemTakipDal.GetList().ToList());
        }

        public Result ServisExist(IslemTakip islemTakip)
        {
            if (_islemTakipDal.Get(p => p.Id == islemTakip.Id) == null)
            {
                return new ErrorResult(Messages.ServisNotFound);
            }
            return new SuccessResult();
        }

        public IResult Update(IslemTakip islemTakip)
        {
            _islemTakipDal.Update(islemTakip);
            return new SuccessResult(Messages.ServisUpdated);
        }


        //İş Kuralları
        //Mesajları Düzelt
        //girilen Id ye sahip servis gerçekten varmı?
        private IResult CheckIfIslemTakipExists(int Id)
        {
            var result = _islemTakipDal.GetList(p => p.Id == Id).Any();
            if (!result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return new SuccessResult();
        }
    }
}
