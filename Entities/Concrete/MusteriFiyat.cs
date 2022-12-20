using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class MusteriFiyat:IEntity
    {
        public int Id { get; set; }
        public int? MusteriId { get; set; }
        public int? FiyatId { get; set; }
        public double? MusFiyat { get; set; }
        public DateTime? BasTarihi { get; set; }
        public DateTime? BitTarihi { get; set; }
        public int Active { get; set; }
    }
}
//CREATE TABLE[dbo].[MusteriFiyats]
//(

//    [Id] INT NOT NULL PRIMARY KEY IDENTITY,
//    [MusteriId] INT NULL,
//    [FiyatId] INT NOT NULL,
//    [MusFiyat] FLOAT NOT NULL,
//    [BasTarihi] DATE NULL,
//    [BitTarihi] DATE NULL,
//    [Active] INT NOT NULL DEFAULT 0
//)
