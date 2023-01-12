using Core.Entities;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class AktifIslem : IEntity
    {
        public int Id { get; set; }
        public int? IslemId { get; set; }
        public int? BolumId { get; set; }
        public int? FurnaceId { get; set; }
        public int? Durum { get; set; }
        public string? Baslik { get; set; }
        public int? Active { get; set; }
    }
}
//CREATE TABLE[dbo].[AktifIslemler] (
//    [Id]        INT IDENTITY(1, 1) NOT NULL,
//    [IslemId]   INT           NOT NULL,
//    [BolumId]   INT           NOT NULL,
//    [FurnaceId] INT           NULL,
//    [Durum]     INT           NULL,
//    [Baslik]    NVARCHAR (20) NULL,
//    [Active] INT NULL,
//    PRIMARY KEY CLUSTERED ([Id] ASC)
//);
