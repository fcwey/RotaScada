using Core.Entities;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class MalGirisIslem : IEntity
    {
        public int Id { get; set; }
        public int MalGirisMalzemeId { get; set; }
        public int BolumIslemId { get; set; }
        public double BirimFiyat { get; set; }
        public int FiyatChange { get; set; }
        public string? Not { get; set; }

    }
}
//CREATE TABLE[dbo].[MalGirisIslemler] (
//    [Id]                INT IDENTITY(1, 1) NOT NULL,
//    [MalGirisMalzemeId] INT            NOT NULL,
//    [BolumIslemId]      INT            NOT NULL,
//    [BirimFiyat]        FLOAT (53)     NOT NULL,
//    [FiyatChange]              INT            NULL,
//    [Not]               NVARCHAR (250) NULL,
//    PRIMARY KEY CLUSTERED ([Id] ASC)
//);


