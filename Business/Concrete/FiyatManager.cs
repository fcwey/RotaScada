using Business.Abstract;
using Business.Constants;
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
    public class FiyatManager:IFiyatService
    {
        private IFiyatDal _fiyatDal;
        public FiyatManager(IFiyatDal fiyatDal)
        { _fiyatDal = fiyatDal; }

        public IResult Add(Fiyat fiyat)
        {
            _fiyatDal.Add(fiyat);
            return new SuccessResult(Messages.ServisAdded);
        }

        public IResult Delete(Fiyat fiyat)
        {
            _fiyatDal.Delete(fiyat);
            return new SuccessResult(Messages.ServisDeleted);
        }

        public IDataResult<Fiyat> GetById(int fiyatId)
        {
            return new SuccessDataResult<Fiyat>(_fiyatDal.Get(p => p.Id == fiyatId));
        }

        public IDataResult<List<Fiyat>> GetList()
        {
            return new SuccessDataResult<List<Fiyat>>(_fiyatDal.GetList().ToList());
        }

        public IDataResult<List<FiyatDto>> GetListFiyatDtos()
        {
            return new SuccessDataResult<List<FiyatDto>>(_fiyatDal.GetListFiyatDtos());
        }

        public IResult Update(Fiyat fiyat)
        {
            _fiyatDal.Update(fiyat);
            return new SuccessResult(Messages.ServisUpdated);
        }
    }
}
