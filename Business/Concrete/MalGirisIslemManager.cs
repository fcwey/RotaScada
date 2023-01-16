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
    public class MalGirisIslemManager:IMalGirisIslemService
    {
        private IMalGirisIslemDal _malGirisIslemDal;
        public MalGirisIslemManager(IMalGirisIslemDal malGirisIslemDal)
        { _malGirisIslemDal = malGirisIslemDal; }

        public IResult Add(MalGirisIslem malGirisIslem)
        {
            _malGirisIslemDal.Add(malGirisIslem);
            return new SuccessResult(Messages.ServisAdded);
        }

        public IResult Delete(MalGirisIslem malGirisIslem)
        {
            IResult result = BusinessRules.Run(CheckIfMalGirisIslemExists(malGirisIslem.Id));

            if (result != null)
            {
                return result;
            }
            _malGirisIslemDal.Delete(malGirisIslem);
            return new SuccessResult(Messages.ServisDeleted);
        }

        public IDataResult<MalGirisIslem> GetById(int malGirisIslemId)
        {
            return new SuccessDataResult<MalGirisIslem>(_malGirisIslemDal.Get(p => p.Id == malGirisIslemId));
        }


        public IDataResult<List<MalGirisIslem>> GetList()
        {
            return new SuccessDataResult<List<MalGirisIslem>>(_malGirisIslemDal.GetList().ToList());
        }

        public IDataResult<List<MalGirisIslemDto>> GetListByDto(int BolumId, int MgirisId)
        {
            return new SuccessDataResult<List<MalGirisIslemDto>>(_malGirisIslemDal.GetListByBolumDtos(BolumId,MgirisId).ToList());
        }

        public IResult Update(MalGirisIslem malGirisIslem)
        {
            _malGirisIslemDal.Update(malGirisIslem);
            return new SuccessResult(Messages.ServisUpdated);
        }


        //İş Kuralları
        //Mesajları Düzelt
        //girilen Id ye sahip servis gerçekten varmı?
        private IResult CheckIfMalGirisIslemExists(int Id)
        {
            var result = _malGirisIslemDal.GetList(p => p.Id == Id).Any();
            if (!result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return new SuccessResult();
        }

    }
}
