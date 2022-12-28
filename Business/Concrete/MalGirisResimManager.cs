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
    public class MalGirisResimManager:IMalGirisResimService
    {

        private IMalGirisResimDal _malGirisResimDal;
        public MalGirisResimManager(IMalGirisResimDal malGirisResimDal)
        { _malGirisResimDal = malGirisResimDal; }

        public IResult Add(MalGirisResim malGirisResim)
        {
            _malGirisResimDal.Add(malGirisResim);
            return new SuccessResult(Messages.ServisAdded);
        }

        public IResult Delete(MalGirisResim malGirisResim)
        {
            IResult result = BusinessRules.Run(CheckIfMalGirisResimExists(malGirisResim.Id));

            if (result != null)
            {
                return result;
            }
            _malGirisResimDal.Delete(malGirisResim);
            return new SuccessResult(Messages.ServisDeleted);
        }

        public IDataResult<MalGirisResim> GetById(int malGirisKayiId)
        {
            return new SuccessDataResult<MalGirisResim>(_malGirisResimDal.Get(p => p.Id == malGirisKayiId));
        }


        public IDataResult<List<MalGirisResim>> GetList()
        {
            return new SuccessDataResult<List<MalGirisResim>>(_malGirisResimDal.GetList().ToList());
        }

        public Result ServisExist(MalGirisResim malGirisResim)
        {
            if (_malGirisResimDal.Get(p => p.Id == malGirisResim.Id) == null)
            {
                return new ErrorResult(Messages.ServisNotFound);
            }
            return new SuccessResult();
        }

        public IResult Update(MalGirisResim malGirisResim)
        {
            _malGirisResimDal.Update(malGirisResim);
            return new SuccessResult(Messages.ServisUpdated);
        }


        //İş Kuralları
        //Mesajları Düzelt
        //girilen Id ye sahip servis gerçekten varmı?
        private IResult CheckIfMalGirisResimExists(int Id)
        {
            var result = _malGirisResimDal.GetList(p => p.Id == Id).Any();
            if (!result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return new SuccessResult();
        }
    }
}
