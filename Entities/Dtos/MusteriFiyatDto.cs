using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class MusteriFiyatDto : IDto
    {
        public int Id { get; set; }
        public string? BolumName { get; set; }
        public string? BolumIslemName { get; set; }
        public string? MGrupName { get; set; }
        public double BirimFiyati { get; set; }
        public double MusteriFiyati { get; set; }    
    }
}
