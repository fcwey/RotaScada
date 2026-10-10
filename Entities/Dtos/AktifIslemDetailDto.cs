using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class AktifIslemDetailDto
    {
        public int Id { get; set; }
       // public string? BolumName { get; set; }
        public string? IslemName { get; set; }
        public string? FurnaceName { get; set; }
        public double Agirlik { get; set; }
        public int Adet { get; set; }
        public int Status { get; set; }
    }
}
