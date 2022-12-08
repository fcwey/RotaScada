using Business.Abstract;
using Business.Constants;
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
    public class BolumManager:IBolumService
    {

        private IBolumDal _bolumDal;
        public BolumManager(IBolumDal bolumDal)
        { _bolumDal = bolumDal; }

        public IResult Add(Bolum bolum)
        {
            _bolumDal.Add(bolum);
            return new SuccessResult(Messages.ServisAdded);
        }

        public IResult Delete(Bolum bolum)
        {
            _bolumDal.Delete(bolum);
            return new SuccessResult(Messages.ServisDeleted);
        }

        public IDataResult<Bolum> GetById(int bolumId)
        {
            return new SuccessDataResult<Bolum>(_bolumDal.Get(p => p.Id == bolumId));
        }

        public IDataResult<List<Bolum>> GetList()
        {
            return new SuccessDataResult<List<Bolum>>(_bolumDal.GetList().ToList());
        }

        public Result ServisExist(Bolum bolum)
        {
            if (_bolumDal.Get(p => p.Id == bolum.Id) == null)
            {
                return new ErrorResult(Messages.ServisNotFound);
            }
            return new SuccessResult();
        }

        public IResult Update(Bolum bolum)
        {
            _bolumDal.Update(bolum);
            return new SuccessResult(Messages.ServisUpdated);
        }
    }
}
