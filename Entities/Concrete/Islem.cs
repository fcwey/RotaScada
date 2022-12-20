using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Islem : IEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
//CREATE TABLE[dbo].[Islemler]
//(

//    [Id] INT NOT NULL PRIMARY KEY IDENTITY,
//    [Name] NVARCHAR(50) NULL
//)
