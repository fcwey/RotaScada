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
    public class FurnaceManager : IFurnaceService
    {
        private IFurnaceDal _furnaceDal;
        public FurnaceManager(IFurnaceDal furnaceDal)
        { _furnaceDal = furnaceDal; }

        public IResult Add(Furnace furnace)
        {
            _furnaceDal.Add(furnace);
            return new SuccessResult(Messages.ServisAdded);
        }

        public IResult Delete(Furnace furnace)
        {
            _furnaceDal.Delete(furnace);
            return new SuccessResult(Messages.ServisDeleted);
        }

        public IDataResult<Furnace> GetById(int furnaceId)
        {
            return new SuccessDataResult<Furnace>(_furnaceDal.Get(p => p.Id == furnaceId));
        }

        public IDataResult<List<Furnace>> GetList()
        {
            return new SuccessDataResult<List<Furnace>>(_furnaceDal.GetList().ToList());
        }



        public IDataResult<List<FurnaceDto>> GetListByBolumId(int bolumId)
        {
            return new SuccessDataResult<List<FurnaceDto>>(_furnaceDal.GetListByBolumId(bolumId).ToList());
        }

        public IDataResult<List<FurnaceDto>> GetListDto()
        {
            return new SuccessDataResult<List<FurnaceDto>>(_furnaceDal.GetListFurnaceDtos().ToList());
        }

        public async Task<IDataResult<List<FurnaceDetailDto>>> GetListFurnaceDetailDtosAsync()
        {
            var list = await _furnaceDal.GetListFurnaceDetailDtosAsync();
            return new SuccessDataResult<List<FurnaceDetailDto>>(list.ToList());
        }

        public async Task<IDataResult<List<FurnaceDetailDto>>> GetListFurnaceDetailDtosAsync(int bolumId)
        {
            var list = await _furnaceDal.GetListFurnaceDetailDtosAsync(bolumId);
            return new SuccessDataResult<List<FurnaceDetailDto>>(list.ToList());
        }

        //Asenkron metodlar

        public async Task< IDataResult<List<FurnaceDto>>> GetListFurnaceDtosAsync()
        {
            var list = await _furnaceDal.GetListFurnaceDtosAsync();
            return new  SuccessDataResult<List<FurnaceDto>>(list.ToList());
        }


        public IResult Update(Furnace furnace)
        {
            _furnaceDal.Update(furnace);
            return new SuccessResult(Messages.ServisUpdated);
        }

       
    }
}
