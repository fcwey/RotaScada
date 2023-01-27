using Business.Abstract;
using Business.Constants;
using Core.Aspects.Autofac.Transaction;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class MalGirisMalzemeManager : IMalGirisMalzemeService
    {

        private IMalGirisMalzemeDal _malGirisMalzemeDal;
        private IMalGirisIslemService _malGirisIslemService;
        public MalGirisMalzemeManager(IMalGirisMalzemeDal malGirisMalzemeDal, IMalGirisIslemService malGirisIslemService)
        {
            _malGirisMalzemeDal = malGirisMalzemeDal;
            _malGirisIslemService = malGirisIslemService;
        }

        public IResult Add(MalGirisMalzeme malGirisMalzeme)
        {
            _malGirisMalzemeDal.Add(malGirisMalzeme);
            return new SuccessResult(Messages.ServisAdded);
        }



        //seçili malzemeyi ve işlemlerini siler
        [TransactionScopeAspect]
        public IResult Delete(MalGirisMalzeme malGirisMalzeme)
        {

            IResult result = BusinessRules.Run(CheckIfMalGirisMalzemeExists(malGirisMalzeme.Id));

            if (result != null)
            {
                return result;
            }

            _malGirisIslemService.DeleteListByMgMalzemeId(malGirisMalzeme.Id);
            _malGirisMalzemeDal.Delete(malGirisMalzeme);
            return new SuccessResult(Messages.MgMalzemeDeleted);

        }

        //seçili kayıt id sine ait tüm malzeme ve işlemleri siler
        [TransactionScopeAspect]
        public IResult DeleteListByMgKayitId(int mgKayitId)
        {
            _malGirisIslemService.DeleteListByMGirisMgKayitId(mgKayitId);
            _malGirisMalzemeDal.DeleteListByMGirisMgKayitId(mgKayitId);
            return new SuccessResult(Messages.MgMalzemeDeleted);

        }

        public IDataResult<MalGirisMalzeme> GetById(int malGirisKayiId)
        {
            return new SuccessDataResult<MalGirisMalzeme>(_malGirisMalzemeDal.Get(p => p.Id == malGirisKayiId));
        }


        public IDataResult<List<MalGirisMalzeme>> GetList()
        {
            return new SuccessDataResult<List<MalGirisMalzeme>>(_malGirisMalzemeDal.GetList().ToList());
        }

        public IDataResult<List<MalGirisMalzemeDto>> GetListByDto(int BolumId, int MgirisId)
        {
            return new SuccessDataResult<List<MalGirisMalzemeDto>>(_malGirisMalzemeDal.GetListByBolumDtos(BolumId, MgirisId));
        }

        public IDataResult<List<MalGirisMalzeme>> GetListByMgKayitId(int MgirisId)
        {
            return new SuccessDataResult<List<MalGirisMalzeme>>(_malGirisMalzemeDal.GetListByMgKayitId(MgirisId));
        }

        public Result ServisExist(MalGirisMalzeme malGirisMalzeme)
        {
            if (_malGirisMalzemeDal.Get(p => p.Id == malGirisMalzeme.Id) == null)
            {
                return new ErrorResult(Messages.ServisNotFound);
            }
            return new SuccessResult();
        }

        public IResult Update(MalGirisMalzeme malGirisMalzeme)
        {
            _malGirisMalzemeDal.Update(malGirisMalzeme);
            return new SuccessResult(Messages.ServisUpdated);
        }


        //İş Kuralları
        //Mesajları Düzelt
        //girilen Id ye sahip servis gerçekten varmı?
        private IResult CheckIfMalGirisMalzemeExists(int Id)
        {
            var result = _malGirisMalzemeDal.GetList(p => p.Id == Id).Any();
            if (!result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return new SuccessResult();
        }
    }
}
