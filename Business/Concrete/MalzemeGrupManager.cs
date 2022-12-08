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
    public class MalzemeGrupManager:IMalzemeGrupService
    {
        private IMalzemeGrupDal _malzemeGrupDal;
        public MalzemeGrupManager(IMalzemeGrupDal malzemeGrupDal)
        { _malzemeGrupDal = malzemeGrupDal; }

        public IResult Add(MalzemeGrup malzemeGrup)
        {
            _malzemeGrupDal.Add(malzemeGrup);
            return new SuccessResult(Messages.ServisAdded);
        }

        public IResult Delete(MalzemeGrup malzemeGrup)
        {
            IResult result = BusinessRules.Run(CheckIfMalzemeGrupExists(malzemeGrup.Id));

            if (result != null)
            {
                return result;
            }
            _malzemeGrupDal.Delete(malzemeGrup);
            return new SuccessResult(Messages.ServisDeleted);
        }

        public IDataResult<MalzemeGrup> GetById(int malzemeGrupId)
        {
            return new SuccessDataResult<MalzemeGrup>(_malzemeGrupDal.Get(p => p.Id == malzemeGrupId));
        }

        public IDataResult<List<MalzemeGrup>> GetList()
        {
            return new SuccessDataResult<List<MalzemeGrup>>(_malzemeGrupDal.GetList().ToList());
        }


        public Result ServisExist(MalzemeGrup malzemeGrup)
        {
            if (_malzemeGrupDal.Get(p => p.Id == malzemeGrup.Id) == null)
            {
                return new ErrorResult(Messages.ServisNotFound);
            }
            return new SuccessResult();
        }

        public IResult Update(MalzemeGrup malzemeGrup)
        {
            _malzemeGrupDal.Update(malzemeGrup);
            return new SuccessResult(Messages.ServisUpdated);
        }


        //İş Kuralları
        //Mesajları Düzelt
        //girilen Id ye sahip servis gerçekten varmı?
        private IResult CheckIfMalzemeGrupExists(int Id)
        {
            var result = _malzemeGrupDal.GetList(p => p.Id == Id).Any();
            if (!result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return new SuccessResult();
        }
    }
}
