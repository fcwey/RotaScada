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
    public class BolumIslemManager : IBolumIslemService
    {
        private IBolumIslemDal _bolumIslemDal; 
        public BolumIslemManager(IBolumIslemDal bolumIslemDal)
        { _bolumIslemDal = bolumIslemDal; }

        public IResult Add(BolumIslem bolumIslem)
        {
            _bolumIslemDal.Add(bolumIslem);
            return new SuccessResult(Messages.ServisAdded);
        }

        public IResult Delete(BolumIslem bolumIslem)
        {
            _bolumIslemDal.Delete(bolumIslem);
            return new SuccessResult(Messages.ServisDeleted);
        }

        public IDataResult<BolumIslem> GetById(int bolumId)
        {
            return new SuccessDataResult<BolumIslem>(_bolumIslemDal.Get(p => p.Id == bolumId));
        }

        public IDataResult<List<BolumIslem>> GetList()
        {
            return new SuccessDataResult<List<BolumIslem>>(_bolumIslemDal.GetList().ToList());
        }

        public IDataResult<List<BolumIslemDto>> GetListBolumIslemDtos()
        {
            return new SuccessDataResult<List<BolumIslemDto>>(_bolumIslemDal.GetListBolumIslemDtos());
        }

        public DataResult<List<BolumIslem>> GetListByBolumId(int bolumId)
        {
            return new SuccessDataResult<List<BolumIslem>>(_bolumIslemDal.GetListByBolumId(bolumId));
        }
        
        public Result ServisExist(BolumIslem bolumIslem)
        {
            if (_bolumIslemDal.Get(p => p.Id == bolumIslem.Id) == null)
            {
                return new ErrorResult(Messages.ServisNotFound);
            }
            return new SuccessResult();
        }

        public IResult Update(BolumIslem bolumIslem)
        {
            _bolumIslemDal.Update(bolumIslem);
            return new SuccessResult(Messages.ServisUpdated);
        }

    }
}
