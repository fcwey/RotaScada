using Business.Abstract;
using Business.Constants;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class MalGirisKayitManager : IMalGirisKayitService
    {
        private IMalGirisKayitDal _malGirisKayitDal;

        private IMalGirisMalzemeService _malGirisMalzemeService;
        public MalGirisKayitManager(IMalGirisKayitDal malGirisKayitDal, IMalGirisMalzemeService malGirisMalzemeService)
        {
            _malGirisKayitDal = malGirisKayitDal;
            _malGirisMalzemeService = malGirisMalzemeService;
        }

        public IResult Add(MalGirisKayit malGirisKayit)
        {
            //Boş kayıt varmı?
            //Bu kayıt malzemeye sahipmi
            //bu malzemelerin işlemleri varmmı?

            List<MalGirisKayit> liste = _malGirisKayitDal.GetList(p=>p.FirmaId==null && p.UserId==1).ToList();
            if (liste.Any())
            {
                var entity = liste.First();//ilk boş veriyi aldık.
                _malGirisMalzemeService.DeleteListByMgKayitId(entity.Id); //bu veriye ait malzeme ve işlemleri sildik.
                malGirisKayit.Id = entity.Id;

            }
            else
            {
                //Burada iş kuralları olabilir.
                _malGirisKayitDal.Add(malGirisKayit);
            }

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

        public async Task<IResult> UpdateForTakip(MalGirisKayit malGirisKayit)
        {
            IResult result = BusinessRules.Run( CheckIfMalGirisKayitIsCorrect(malGirisKayit.Id));

            if (result != null)
            {
                return result;
            }
            await _malGirisKayitDal.UpdateForTakip(malGirisKayit);
            return new SuccessResult(Messages.MalGirisKayitUpdated);
        }


        public IResult Update(MalGirisKayit malGirisKayit)
        {
            IResult result = BusinessRules.Run( CheckIfMalGirisKayitIsCorrect(malGirisKayit.Id));

            if (result != null)
            {
                return result;
            }
         


            _malGirisKayitDal.Update(malGirisKayit);

            return new SuccessResult(Messages.MalGirisKayitUpdated);
        }
        //Update + adım ekler
        //sadece update yapar


        //İş Kuralları
        //Mesajları Düzelt
        //girilen Id ye sahip servis gerçekten varmı?

        //Kayıt zaten varmı? add için
        private IResult CheckIfMalGirisKayitExists(int Id)
        {
            var result = _malGirisKayitDal.GetList(p => p.Id == Id).Any();
            if (!result)
            {
                return new ErrorResult(Messages.MalGirisKayitAlreadyExists);
            }
            return new SuccessResult();
        }

        //kayıt yokmu? Update için
        private IResult CheckIfMalGirisKayitNotFound(int Id)
        {
            var result = _malGirisKayitDal.GetList(p => p.Id == Id).Any();
            if (!result)
            {
                return new ErrorResult(Messages.MalKayitNotFound);
            }
            return new SuccessResult();
        }

        //malzeme kayıt için uygunmu , malzeme ve işlemleri girilmişmi?
        private IResult CheckIfMalGirisKayitIsCorrect(int Id)
        {
            var malzemeresult = _malGirisKayitDal.MalzemeIsCorrect(Id);
            if (!malzemeresult)
            {
                return new ErrorResult(Messages.MalKayitMalzemeYok);
            }
            var islemresult = _malGirisKayitDal.IslemIsCorrect(Id);
            if (!islemresult)
            {
                return new ErrorResult(Messages.MalKayitIslemYok);
            }
            return new SuccessResult();
        }
        

    }
}
