using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class MalzemeDto: IDto
    {
        public int Id { get; set; }
        public string? MGrupName { get; set; }
        public string? MalzemeName { get; set; }
        public string? MaxSertlik { get; set; }
    }
}
