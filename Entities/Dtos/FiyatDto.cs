using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class FiyatDto
    {
        public int Id { get; set; }
        public string? BolumName { get; set; }
        public string? BolumIslemName { get; set; }
        public string? MGrupName { get; set; }
        public double Fiyati { get; set; }
    }
}
