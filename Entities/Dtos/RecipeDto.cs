using Core.Entities;
using System.Collections.Generic;

namespace Entities.Dtos
{
    public class RecipeDto : IDto
    {
        public int Id { get; set; }
        public int RecipeID { get; set; }
        public int RecipeType { get; set; }
        public string RecipeName { get; set; } = string.Empty;
        public int TotalStepCount { get; set; }
        public List<RecipeStepDto> Steps { get; set; } = new List<RecipeStepDto>();
    }
}