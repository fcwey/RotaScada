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
    public class MusteriManager : IMusteriService
    {
        private IMusteriDal _musteriDal;
        public MusteriManager(IMusteriDal musteriDal)
        { _musteriDal = musteriDal; }

        public IDataResult<Musteri> GetById(int musteriId)
        {
            return new SuccessDataResult<Musteri>(_musteriDal.Get(p => p.Id == musteriId));
        }

        public IDataResult<List<Musteri>> GetList()
        {
            return new SuccessDataResult<List<Musteri>>(_musteriDal.GetList().ToList());
        }

        public IResult Add(Musteri musteri)
        {
            _musteriDal.Add(musteri);
            return new SuccessResult(Messages.ServisAdded);
        }

        public IResult Delete(Musteri musteri)
        {
            _musteriDal.Delete(musteri);
            return new SuccessResult(Messages.ServisDeleted);
        }



        public IResult Update(Musteri musteri)
        {
            _musteriDal.Update(musteri);
            return new SuccessResult(Messages.ServisUpdated);
        }

        public IDataResult<List<MusteriDto>> GetListDto()
        {
            return new SuccessDataResult<List<MusteriDto>>(_musteriDal.GetListMusteriDtos());
        }
    }
}
