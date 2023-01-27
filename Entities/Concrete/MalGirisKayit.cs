using Core.Entities;
using Core.Entities.Concrete;
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
        public int Status { get; set; }
    }
}
//CREATE TABLE[dbo].[MalGirisKayitlar] (
//    [Id]           INT IDENTITY(1, 1) NOT NULL,
//    [FirmaId]      INT      NULL,
//    [UserId]       INT      NULL,
//    [GirisTarihi]  DATETIME NULL,
//    [TeslimTarihi] DATE     NULL,
//    [Status] INT NULL,
//    PRIMARY KEY CLUSTERED ([Id] ASC)
//); ULL,



//Status: 0:Pasif 1:Aktif  2:Düzenlemede