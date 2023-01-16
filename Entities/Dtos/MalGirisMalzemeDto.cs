using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class MalGirisMalzemeDto:IDto
    {
        public int Id { get; set;}
        public string? MalzemeGrupName { get; set; }
        public string? MalzemeStandartNo { get; set; }
        public string? Sertlik { get; set; }
        public double Agirlik { get; set; }
        public int Adet { get; set; }
    }
}
