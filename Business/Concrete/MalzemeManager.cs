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
    public class MalzemeManager:IMalzemeService
    {
        private IMalzemeDal _malzemeDal;
        public MalzemeManager(IMalzemeDal malzemeDal)
        { _malzemeDal = malzemeDal; }

        public IResult Add(Malzeme malzeme)
        {
            _malzemeDal.Add(malzeme);
            return new SuccessResult(Messages.ServisAdded);
        }

        public IResult Delete(Malzeme malzeme)
        {
            IResult result = BusinessRules.Run(CheckIfMalzemeExists(malzeme.Id));

            if (result != null)
            {
                return result;
            }
            _malzemeDal.Delete(malzeme);
            return new SuccessResult(Messages.ServisDeleted);
        }

        public IDataResult<Malzeme> GetById(int malzemeId)
        {
            return new SuccessDataResult<Malzeme>(_malzemeDal.Get(p => p.Id == malzemeId));
        }

        public IDataResult<List<Malzeme>> GetList()
        {
            return new SuccessDataResult<List<Malzeme>>(_malzemeDal.GetList().ToList());
        }


        public Result ServisExist(Malzeme malzeme)
        {
            if (_malzemeDal.Get(p => p.Id == malzeme.Id) == null)
            {
                return new ErrorResult(Messages.ServisNotFound);
            }
            return new SuccessResult();
        }

        public IResult Update(Malzeme malzeme)
        {
            _malzemeDal.Update(malzeme);
            return new SuccessResult(Messages.ServisUpdated);
        }


        //İş Kuralları
        //Mesajları Düzelt
        //girilen Id ye sahip servis gerçekten varmı?
        private IResult CheckIfMalzemeExists(int Id)
        {
            var result = _malzemeDal.GetList(p => p.Id == Id).Any();
            if (!result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return new SuccessResult();
        }
    }
}
