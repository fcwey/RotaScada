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
    public class ServisManager : IServisService
    {

        private IServisDal _servisDal;
        public ServisManager(IServisDal servisDal)
        { _servisDal = servisDal; }

        public IResult Add(Servis servis)
        {
            _servisDal.Add(servis);
            return new SuccessResult(Messages.ServisAdded);
        }

        public IResult Delete(Servis servis)
        {
            IResult result = BusinessRules.Run(CheckIfServisExists(servis.Id));

            if (result != null)
            {
                return result;
            }
            _servisDal.Delete(servis);
            return new SuccessResult(Messages.ServisDeleted);
        }

        public IDataResult<Servis> GetById(int serviceId)
        {
            return new SuccessDataResult<Servis>(_servisDal.Get(p => p.Id == serviceId));
        }

        public IDataResult<List<Servis>> GetList()
        {
            return new SuccessDataResult<List<Servis>>(_servisDal.GetList().ToList());
        }

        public IDataResult<List<ServisDto>> GetListDto()
        {
            return new SuccessDataResult<List<ServisDto>>(_servisDal.GetListServisDtos());
        }

        public Result ServisExist(Servis servis)
        {
            if (_servisDal.Get(p => p.Id == servis.Id) == null)
            {
                return new ErrorResult(Messages.ServisNotFound);
            }
            return new SuccessResult();
        }

        public IResult Update(Servis servis)
        {
            _servisDal.Update(servis);
            return new SuccessResult(Messages.ServisUpdated);
        }

        //girilen Id ye sahip servis gerçekten varmı?
        private IResult CheckIfServisExists(int Id)
        {
            var result = _servisDal.GetList(p => p.Id == Id).Any();
            if (!result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return new SuccessResult();
        }
    }
}
