using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IRecipeService
    {
        Task<IDataResult<List<Recipe>>> GetAllRecipesAsync();
        Task<IDataResult<Recipe>> GetRecipeByIdAsync(int id);
        Task<IResult> AddRecipeAsync(Recipe recipe);
        Task<IResult> UpdateRecipeAsync(Recipe recipe);
        Task<IResult> DeleteRecipeAsync(int id);

        // Reçete parametrelerini doğrulayıp PLC DB'sine yazar (Download)
        Task<IResult> DownloadRecipeToPlcAsync(int recipeId, int targetDbNumber = 2);
        Task<IResult> DownloadAndStartRecipeAsync(int recipeId);
    }
}
