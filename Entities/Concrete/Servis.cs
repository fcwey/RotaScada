using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Servis: IEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }
        public byte[]? Resim { get; set; }
    }
}
//CREATE TABLE[dbo].[Servis] (
//    [Id]    INT IDENTITY(1, 1) NOT NULL,
//    [Name]  NVARCHAR (50)   NULL,
//    [Top] INT NULL,
//    [Left]  INT             NULL,
//    [Resim] VARBINARY (MAX) NULL,
//    PRIMARY KEY CLUSTERED ([Id] ASC)
//);