using Core.Entities;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static log4net.Appender.RollingFileAppender;

namespace Entities.Concrete
{
    public class IslemTakip : IEntity
    {
        public int Id { get; set; }
        public int MalGirisMalzemeId { get; set; }
        public int KulId { get; set; }
        public int AktifIslemId { get; set; }
        public int ListeId { get; set; }
        public int Active { get; set; }
        public string Durum { get; set; }
        public string Nerede { get; set; }
        public DateTime BaslamaTarihi { get; set; }
        public DateTime BitimTarihi { get; set; }
        public int Derece { get; set; }
        public int Dakika { get; set; }
    }
}
//CREATE TABLE[dbo].[IslemTakips] (
//    [Id]                INT NOT NULL,
//    [MalGirisMalzemeId] INT           NULL,
//    [KulId]             INT           NULL,
//    [AktifIslemId]      INT           NULL,
//    [ListeId]           INT           NULL,
//    [Active]            INT           NULL,
//    [Durum]             NVARCHAR (20) NULL,
//    [Nerede] NVARCHAR (50) NULL,
//    [BaslamaTarihi] DATETIME NULL,
//    [BitimTarihi]       DATETIME      NULL,
//    [Derece]            INT           NULL,
//    [Dakika]            INT           NULL,
//    PRIMARY KEY CLUSTERED ([Id] ASC)
//);
