using Core.Entities;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Furnace : IEntity
    {
        public int Id { get; set; }
        public int BolumId { get; set; }
        public string? Name { get; set; }
        public int Derece { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Durum { get; set; }
        public int Aktive { get; set; }
    }
}
//CREATE TABLE[dbo].[Furnaces] (
//    [Id]      INT IDENTITY(1, 1) NOT NULL,
//    [BolumId] INT           NOT NULL,
//    [Name]    NVARCHAR (50) NULL,
//    [Derece] INT NULL,
//    [Left]    INT           NULL,
//    [Top]     INT           NULL,
//    [Durum]   INT           NULL,
//    [Aktive]  INT           NULL,
//    PRIMARY KEY CLUSTERED ([Id] ASC)
//);
