using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class IslemTakipDto
    {
        public int Id { get; set; }
        public string? FirmaAdi { get; set; }
        public DateTime GirisTarihi { get; set; }
        public string? MGrupAdi { get; set; }
        public string? MalzemeAdi { get; set; }
        public string? IstenenSertlik { get; set; }
        public string? Agirlik { get; set; }
        public string? Adet { get; set; }
        public string? Durum { get; set; }
        public string? Nerede { get; set; }
        public string? IslemBaslik { get; set; } //Nerede ya fırın adı, yada liste adı
        public bool IsSelected { get; set; }

    }
}
