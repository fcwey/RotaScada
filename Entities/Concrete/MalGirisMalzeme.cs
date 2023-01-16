using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class MalGirisMalzeme : IEntity
    {
        public int Id { get; set; }
        public int MalGirisId { get; set; }
        public int MalzemeId { get; set; }
        public int BolumId { get; set; }
        public double Agirlik { get; set; }
        public int Adet { get; set; }
        public string? Sertlik { get; set; }
        public string? SertlikType { get; set; }
        public string? BulunanSertlik { get; set; }
        public string? SertlikDerinlik { get; set; }
        public double Toplam { get; set; }
        public int Secilen { get; set; }
        public int FormNo { get; set; }
        public int Silindi { get; set; }
        public string? SilindiAciklama { get; set; }
        public int SilenKullanici { get; set; }

    }
}
//CREATE TABLE[dbo].[MalGirisMalzemeler] (
//    [Id]              INT IDENTITY(1, 1) NOT NULL,
//    [MalGirisId]      INT            NULL,
//    [MalzemeId]       INT            NULL,
//    [BolumId]         INT            NULL,
//    [Agirlik]         FLOAT (53)     NULL,
//    [Adet] INT NULL,
//    [Sertlik]         NVARCHAR (10)  NULL,
//    [SertlikType] NVARCHAR (10)  NULL,
//    [BulunanSertlik] NVARCHAR (10)  NULL,
//    [SertlikDerinlik] NVARCHAR (10)  NULL,
//    [Toplam] FLOAT (53)     NULL,
//    [Secilen] INT NULL,
//    [FormNo]          INT            NULL,
//    [Silindi]         INT            NULL,
//    [SilindiAciklama] NVARCHAR (250) NULL,
//    [SilenKullanici] INT NULL,
//    PRIMARY KEY CLUSTERED ([Id] ASC)
//);

