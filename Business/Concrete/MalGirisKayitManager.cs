using Business.Abstract;
using Business.Constants;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class MalGirisKayitManager:IMalGirisKayitService
    {
        private IMalGirisKayitDal _malGirisKayitDal;
        public MalGirisKayitManager(IMalGirisKayitDal malGirisKayitDal)
        { _malGirisKayitDal = malGirisKayitDal; }

        public IResult Add(MalGirisKayit malGirisKayit)
        {
            _malGirisKayitDal.Add(malGirisKayit);
            return new SuccessResult(Messages.ServisAdded);
        }

        public IResult Delete(MalGirisKayit malGirisKayit)
        {
            IResult result = BusinessRules.Run(CheckIfMalGirisKayitExists(malGirisKayit.Id));
            
            if (result != null)
            {
                return result;
            }
            _malGirisKayitDal.Delete(malGirisKayit);
            return new SuccessResult(Messages.ServisDeleted);
        }

        public IDataResult<MalGirisKayit> GetById(int malGirisKayiId)
        {
            return new SuccessDataResult<MalGirisKayit>(_malGirisKayitDal.Get(p => p.Id == malGirisKayiId));
        }


        public IDataResult<List<MalGirisKayit>> GetList()
        {
            return new SuccessDataResult<List<MalGirisKayit>>(_malGirisKayitDal.GetList().ToList());
        }

        public Result ServisExist(Malzeme malzeme)
        {
            if (_malGirisKayitDal.Get(p => p.Id == malzeme.Id) == null)
            {
                return new ErrorResult(Messages.ServisNotFound);
            }
            return new SuccessResult();
        }

        public IResult Update(MalGirisKayit malGirisKayit)
        {
            _malGirisKayitDal.Update(malGirisKayit);
            return new SuccessResult(Messages.ServisUpdated);
        }


        //İş Kuralları
        //Mesajları Düzelt
        //girilen Id ye sahip servis gerçekten varmı?
        private IResult CheckIfMalGirisKayitExists(int Id)
        {
            var result = _malGirisKayitDal.GetList(p => p.Id == Id).Any();
            if (!result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return new SuccessResult();
        }
    }
}
