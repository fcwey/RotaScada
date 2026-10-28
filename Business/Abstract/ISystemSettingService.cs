using Core.Utilities.Results;
using Entities.Dtos;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ISystemSettingService
    {
        // Kritik parametre güncelleme ve yetki kontrolü metodu
        Task<IResult> UpdateCriticalParameterAsync(ParameterUpdateDto dto);
    }
}