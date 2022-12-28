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
    public class MalGirisMalzemeManager:IMalGirisMalzemeService
    {

        private IMalGirisMalzemeDal _malGirisMalzemeDal;
        public MalGirisMalzemeManager(IMalGirisMalzemeDal malGirisMalzemeDal)
        { _malGirisMalzemeDal = malGirisMalzemeDal; }

        public IResult Add(MalGirisMalzeme malGirisMalzeme)
        {
            _malGirisMalzemeDal.Add(malGirisMalzeme);
            return new SuccessResult(Messages.ServisAdded);
        }

        public IResult Delete(MalGirisMalzeme malGirisMalzeme)
        {
            IResult result = BusinessRules.Run(CheckIfMalGirisMalzemeExists(malGirisMalzeme.Id));

            if (result != null)
            {
                return result;
            }
            _malGirisMalzemeDal.Delete(malGirisMalzeme);
            return new SuccessResult(Messages.ServisDeleted);
        }

        public IDataResult<MalGirisMalzeme> GetById(int malGirisKayiId)
        {
            return new SuccessDataResult<MalGirisMalzeme>(_malGirisMalzemeDal.Get(p => p.Id == malGirisKayiId));
        }


        public IDataResult<List<MalGirisMalzeme>> GetList()
        {
            return new SuccessDataResult<List<MalGirisMalzeme>>(_malGirisMalzemeDal.GetList().ToList());
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
