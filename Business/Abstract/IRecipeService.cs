using Core.Utilities.Results;
using Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IRecipeService
    {
        Task<IDataResult<List<Recipe>>> GetAllRecipesAsync();
        Task<IDataResult<Recipe>> GetRecipeByIdAsync(int id);
        Task<IResult> AddRecipeAsync(Recipe recipe);
        Task<IResult> UpdateRecipeAsync(Recipe recipe);
        Task<IResult> DeleteRecipeAsync(int id);

        Task<IResult> DownloadRecipeToPlcAsync(int recipeId, int targetDbNumber = 2, int? userId = null, string? userIp = null);
        Task<IResult> DownloadAndStartRecipeAsync(int recipeId, int? userId = null, string? userIp = null);
        Task<IResult> StopActiveBatchAsync(string completionNote = "Proses tamamlandı.", int? userId = null, string? userIp = null);
    }
}