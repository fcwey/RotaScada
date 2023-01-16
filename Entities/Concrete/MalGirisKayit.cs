using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class MalGirisKayit:IEntity
    {
        public int Id { get; set; }
        public int? FirmaId { get; set; }
        public int? UserId { get; set; }
        public DateTime? GirisTarihi { get; set; }
        public DateTime? TeslimTarihi { get; set; }

    }
}
//[Id] INT IDENTITY(1, 1) NOT NULL,
//    [FirmaId]      INT      NOT NULL,
//    [UserId]       INT      NOT NULL,
//    [GirisTarihi]  DATETIME NULL,
//    [TeslimTarihi] DATE     NULL,