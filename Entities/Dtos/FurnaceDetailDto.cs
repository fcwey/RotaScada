using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class FurnaceDetailDto:IDto
    {
        public int Id { get; set; }
        public string? FurnaceName { get; set; }
        public string? IslemAdı { get; set; }
        public int Status { get; set; }
        public int Adet { get; set; }
        public double Kg { get; set; }
    }
}
