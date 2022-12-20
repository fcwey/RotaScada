using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class AktifIslem : IEntity
    {
        public int? Id { get; set; }
        public int? IslemId { get; set; }
        public int? BolumId { get; set; }
        public int? Derece { get; set; }
        public int? Dakika { get; set; }
        public int? Durum { get; set; }
        public int? Left { get; set; }
        public int? Top { get; set; }
        public string? Baslik { get; set; }
        public int? Active { get; set; }
    }
}
//CREATE TABLE[dbo].[AktifIslemler]
//(

//    [Id] INT NOT NULL PRIMARY KEY IDENTITY,
//    [IslemId] INT NOT NULL,
//    [BolumId] INT NOT NULL,
//    [Derece] INT NULL,
//    [Dakika] INT NULL,
//    [Durum] INT NULL,
//    [Left] INT NULL,
//    [Top] INT NULL,
//    [Baslik] NVARCHAR(20) NULL, 
//    [Active] TINYINT NULL
//)

