using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class RecipeManager : IRecipeService
    {
        private readonly IRecipeDal _recipeDal;
        private readonly IS7DriverDal _s7DriverDal;

        public RecipeManager(IRecipeDal recipeDal, IS7DriverDal s7DriverDal)
        {
            _recipeDal = recipeDal;
            _s7DriverDal = s7DriverDal;
        }

        public async Task<IDataResult<List<Recipe>>> GetAllRecipesAsync()
        {
            var recipes = await _recipeDal.GetListAsync(r => r.IsActive);
            return new SuccessDataResult<List<Recipe>>(recipes.ToList());
        }
    
        public async Task<IDataResult<Recipe>> GetRecipeByIdAsync(int id)
        {
            var recipe = await _recipeDal.GetAsync(r => r.Id == id);
            if (recipe == null) return new ErrorDataResult<Recipe>("Reçete bulunamadı.");
            return new SuccessDataResult<Recipe>(recipe);
        }

        public async Task<IResult> AddRecipeAsync(Recipe recipe)
        {
            await _recipeDal.AddAsync(recipe);
            return new SuccessResult("Reçete başarıyla kaydedildi.");
        }

        public async Task<IResult> UpdateRecipeAsync(Recipe recipe)
        {
            await _recipeDal.UpdateAsync(recipe);
            return new SuccessResult("Reçete güncellendi.");
        }

        public async Task<IResult> DeleteRecipeAsync(int id)
        {
            var recipe = await _recipeDal.GetAsync(r => r.Id == id);
            if (recipe != null)
            {
                recipe.IsActive = false; // Soft-delete
                await _recipeDal.UpdateAsync(recipe);
            }
            return new SuccessResult("Reçete silindi.");
        }

        public async Task<IResult> DownloadRecipeToPlcAsync(int recipeId, int targetDbNumber = 2)
        {
            var recipe = await _recipeDal.GetAsync(r => r.Id == recipeId);
            if (recipe == null)
                return new ErrorResult("PLC'ye aktarılacak reçete bulunamadı.");

            // 1. Sınır Kontrolleri (Safety Validation)
            if (recipe.TargetTemperature > 1200)
                return new ErrorResult("Güvenlik İhlali: Hedef sıcaklık 1200°C üzerinde olamaz.");

            // 2. PLC DB2 Hazırlığı (Reçete Veri Yapısı)
            var plcRecipeStruct = new PlcRecipeStruct
            {
                TargetTemperature = (float)recipe.TargetTemperature,
                SoakTimeMinutes = recipe.SoakTimeMinutes,
                HeatingRampRate = (float)recipe.HeatingRampRate,
                GasFlowSetPoint = (float)recipe.GasFlowSetPoint
            };

            // 3. Thread-Safe WriteClassAsync ile PLC DB2'ye Yazma
            await _s7DriverDal.WriteClassAsync(plcRecipeStruct, targetDbNumber, startByte: 0);

            return new SuccessResult($"'${recipe.RecipeName}' reçetesi PLC DB${targetDbNumber} alanına başarıyla indirildi.");
        }

        // PLC DB2 Reçete Byte Hizalaması
        private class PlcRecipeStruct
        {
            public float TargetTemperature { get; set; }  // DB2.DBD0
            public int SoakTimeMinutes { get; set; }      // DB2.DBD4
            public float HeatingRampRate { get; set; }    // DB2.DBD8
            public float GasFlowSetPoint { get; set; }    // DB2.DBD12
        }
    }
}
