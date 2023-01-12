using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class FurnaceDto:IDto
    {
        public int Id { get; set; }
        public string? BolumAdi { get; set; }
        public string? Name { get; set; }
        public int Derece { get; set; }
    }
}
