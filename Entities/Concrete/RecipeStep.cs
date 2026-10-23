using Core.Entities;

namespace Entities.Concrete
{
    public class RecipeStep : IEntity
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public int StepNo { get; set; }                     // Adım Sırası (1 - 20)
        public int StepType { get; set; }                   // 0: BOŞ, 1: VAKUM, 2: ISITMA, 3: SOĞUTMA

        // Vakum Parametreleri
        public float Vac_SetPoint { get; set; }             // Target Vakum Basıncı (mbar)
        public int Vac_TimeoutMs { get; set; }              // Vakum Zaman Aşımı (Milisaniye)

        // Sıcaklık Parametreleri
        public float TargetTemp { get; set; }               // Hedef Sıcaklık (°C)
        public int StepTimeMs { get; set; }                 // Adım Süresi (Milisaniye)
        public float GSoak { get; set; }                    // Guaranteed Soak Toleransı (°C)

        // Gaz ve Sıvı Debileri (L/h)
        public float N2_Flow { get; set; }                  // Azot Debisi (L/h)
        public float NH3_Flow { get; set; }                 // Amonyak Debisi (L/h)
        public float dNH3_Flow { get; set; }                // Ayrışmış Amonyak Debisi (L/h)
        public float Su_Flow { get; set; }                  // Su / Nemlendirme Debisi (L/h)
        public float Oxygen_Set { get; set; }               // Oksijen / Oksidasyon Set (%)

        // Navigation Property
        public virtual Recipe? Recipe { get; set; }
    }
}