using Core.Entities;

namespace Entities.Dtos
{
    public class RecipeStepDto : IDto
    {
        public int StepNo { get; set; }
        public int StepType { get; set; }
        public float Vac_SetPoint { get; set; }
        public int Vac_TimeoutMs { get; set; }
        public float TargetTemp { get; set; }
        public int StepTimeMs { get; set; }
        public float GSoak { get; set; }
        public float N2_Flow { get; set; }
        public float NH3_Flow { get; set; }
        public float dNH3_Flow { get; set; }
        public float Su_Flow { get; set; }
        public float Oxygen_Set { get; set; }
    }
}