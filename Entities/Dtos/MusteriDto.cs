using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class MusteriDto : IDto
    {
        public int Id { get; set; }
        public string FirmaAd { get; set; }
        public string Yetkili { get; set; }
        public string Telefon { get; set; }
        public string Adres { get; set; }
    }
}
