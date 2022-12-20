using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Musteri:IEntity
    {
        public int Id { get; set; }
        public string? FirmaAd { get; set; }
        public string? Yetkili { get; set; }
        public string? Telefon { get; set; }
        public string? Fax { get; set; }
        public string? Email { get; set; }
        public string? Web { get; set; }
        public string? Adres { get; set; }
        public int? ServisId { get; set; }
        public int? Nakliye { get; set; }
        public int? OdemeTuru { get; set; }
        public string? Note { get; set; }
        public string? Adres2 { get; set; }

    }
}
//CREATE TABLE[dbo].[Musteriler] (
//    [Id]        INT NOT NULL,
//    [FirmaAd]   VARCHAR (150) NOT NULL,

//     [Yetkili]   VARCHAR (100) NULL,
//    [Telefon] VARCHAR (15)  NULL,
//    [Fax] VARCHAR (15)  NULL,
//    [Email] VARCHAR (100) NULL,
//    [Web] VARCHAR (100) NULL,
//    [Adres] VARCHAR (250) NULL,
//    [ServisId] INT NULL,
//    [Nakliye]   INT           NULL,
//    [OdemeTuru] INT           NULL,
//    [Note]      VARCHAR (250) NULL,
//    [Adres2] VARCHAR (250) NULL,
//    PRIMARY KEY CLUSTERED ([Id] ASC)
//);
