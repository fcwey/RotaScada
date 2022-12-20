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
    public class AktifIslemManager : IAktifIslemService
    {
        private IAktifIslemDal _aktifIslemDal;
        public AktifIslemManager(IAktifIslemDal aktifIslemDal)
        { _aktifIslemDal = aktifIslemDal; }

        public IResult Add(AktifIslem aktifIslem)
        {
            _aktifIslemDal.Add(aktifIslem);
            return new SuccessResult(Messages.ServisAdded);
        }

        public IResult Delete(AktifIslem aktifIslem)
        {
            _aktifIslemDal.Delete(aktifIslem);
            return new SuccessResult(Messages.ServisDeleted);
        }

        public IDataResult<AktifIslem> GetById(int aktifIslemId)
        {
            return new SuccessDataResult<AktifIslem>(_aktifIslemDal.Get(p => p.Id == aktifIslemId));
        }

        public IDataResult<List<AktifIslem>> GetList()
        {
            return new SuccessDataResult<List<AktifIslem>>(_aktifIslemDal.GetList().ToList());
        }

        public IDataResult<List<AktifIslemDto>> GetListDto()
        {
            return new SuccessDataResult<List<AktifIslemDto>>(_aktifIslemDal.GetListAktifIslemDtos());
        }

        public IResult Update(AktifIslem aktifIslem)
        {
            _aktifIslemDal.Update(aktifIslem);
            return new SuccessResult(Messages.ServisUpdated);
        }
    }
}
