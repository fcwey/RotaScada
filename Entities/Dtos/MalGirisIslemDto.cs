using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class MalGirisIslemDto:IDto
    {
        public int Id { get; set; }
        public string? BolumName { get; set; }
        public string? BolumIslemName { get; set; }
        public double BirimFiyat { get; set; }
        public double Agirlik { get; set; }

    }
}
