using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class Recipe : IEntity
    {
        public int Id { get; set; }
        public int RecipeID { get; set; }                   // PLC Reçete Numarası
        public int RecipeType { get; set; }                 // 0: BOŞ, 1: TEMPER, 2: NİTRASYON, 3: OKSİDASYON
        public string RecipeName { get; set; } = string.Empty; // Reçete Adı (Max 30 Karakter)
        public int TotalStepCount { get; set; }             // Aktif Adım Sayısı (Max 20)
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;

        // 20 Adımlık İlişkisel Dizi
        public virtual ICollection<RecipeStep> Steps { get; set; } = new List<RecipeStep>();
    }
}
