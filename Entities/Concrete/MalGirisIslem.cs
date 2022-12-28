using Core.Entities;
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
        public int Adet { get; set; }
        public double Agirlik { get; set; }
        public string Not { get; set; }

    }
}
//CREATE TABLE[dbo].[MalGirisIslemler] (
//    [Id]                INT IDENTITY(1, 1) NOT NULL,
//    [MalGirisMalzemeId] INT            NULL,
//    [BolumIslemId]      INT            NULL,
//    [BirimFiyat]        FLOAT (53)     NULL,
//    [Adet] INT NULL,
//    [Agirlik]           FLOAT (53)     NULL,
//    [Not] NVARCHAR (250) NULL,
//    PRIMARY KEY CLUSTERED ([Id] ASC)
//);

