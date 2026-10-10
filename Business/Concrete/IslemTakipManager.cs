using Business.Abstract;
using Business.Constants;
using Core.Aspects.Autofac.Transaction;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.Constant;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Entities.Constant.TakipStatus;

namespace Business.Concrete
{
    public class IslemTakipManager : IIslemTakipService
    {

        private IIslemTakipDal _islemTakipDal;
        public IslemTakipManager(IIslemTakipDal islemTakipDal)
        { _islemTakipDal = islemTakipDal; }

        public IResult Add(IslemTakip islemTakip)
        {
            _islemTakipDal.Add(islemTakip);
            return new SuccessResult(Messages.ServisAdded);
        }

        public async Task AddFirstIslemTakipByMgKayit(MalGirisKayit malGirisKayit)
        {

            await _islemTakipDal.AddFirstIslemTakipByMgKayit(malGirisKayit);
        }

        public IResult Delete(IslemTakip islemTakip)
        {
            IResult result = BusinessRules.Run(CheckIfIslemTakipExists(islemTakip.Id));

            if (result != null)
            {
                return result;
            }
            _islemTakipDal.Delete(islemTakip);
            return new SuccessResult(Messages.ServisDeleted);
        }

        public IDataResult<IslemTakip> GetById(int islemTakipId)
        {
            return new SuccessDataResult<IslemTakip>(_islemTakipDal.Get(p => p.Id == islemTakipId));
        }


        public IDataResult<List<IslemTakip>> GetList()
        {
            return new SuccessDataResult<List<IslemTakip>>(_islemTakipDal.GetList().ToList());
        }

        public async Task<IDataResult<List<IslemTakip>>> GetListAsync()
        {
            var list = await _islemTakipDal.GetListAsync();
            return new SuccessDataResult<List<IslemTakip>>(list.ToList());
        }

        public async Task<IDataResult<List<IslemTakipDto>>> GetListDtoAsync()
        {
            var list = await _islemTakipDal.GetListDtoAsync();
            return new SuccessDataResult<List<IslemTakipDto>>(list.ToList());
        }

        public async Task<IDataResult<List<IslemTakipDto>>> GetListDtoAsync(int bolumId)
        {
            var list = await _islemTakipDal.GetListDtoAsync(bolumId);
            return new SuccessDataResult<List<IslemTakipDto>>(list.ToList());
        }

        public async Task<IDataResult<List<IslemTakipDto>>> GetListDtoAsync(int bolumId, string nerede)
        {
            var list = await _islemTakipDal.GetListDtoAsync(bolumId, nerede);
            return new SuccessDataResult<List<IslemTakipDto>>(list.ToList());
        }

        public async Task<IDataResult<List<IslemTakipDto>>> GetListDtoByFurnaceAsync(int furnaceId)
        {
            var list = await _islemTakipDal.GetListDtoByFurnaceAsync(furnaceId);
            return new SuccessDataResult<List<IslemTakipDto>>(list.ToList());
        }

        [TransactionScopeAspect] //iş kuralları yazılabilir. biri gerçekleşmezsee diğerleride gercekleşmez
        public async Task<IResult> SendToFurnaceAsync(List<IslemTakipDto> liste, int AktifIslemId)
        {
            await _islemTakipDal.SendToFurnaceAsync(liste, AktifIslemId);
            await _islemTakipDal.SendToHistoryUpdateAsync(liste);
            return new SuccessResult(Messages.ServisUpdated);
        }

        public Task<IResult> SendToUnitAsync(List<IslemTakipDto> liste, UnitType unitType)
        {
           
            throw new NotImplementedException();
        }

        public Result ServisExist(IslemTakip islemTakip)
        {
            if (_islemTakipDal.Get(p => p.Id == islemTakip.Id) == null)
            {
                return new ErrorResult(Messages.ServisNotFound);
            }
            return new SuccessResult();
        }

        public IResult Update(IslemTakip islemTakip)
        {
            _islemTakipDal.Update(islemTakip);
            return new SuccessResult(Messages.ServisUpdated);
        }


        //İş Kuralları
        //Mesajları Düzelt
        //girilen Id ye sahip servis gerçekten varmı?
        private IResult CheckIfIslemTakipExists(int Id)
        {
            var result = _islemTakipDal.GetList(p => p.Id == Id).Any();
            if (!result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return new SuccessResult();
        }
    }
}
