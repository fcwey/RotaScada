using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Fiyat:IEntity
    {
        public int Id { get; set; }
        public int BolumIslemId { get; set; }
        public int MGrupId { get; set; }
        public double Fiyati { get; set; }
    }
}
//CREATE TABLE[dbo].[Fiyatlar]
//(

//    [Id] INT NOT NULL PRIMARY KEY IDENTITY,
//    [BolumId] INT NOT NULL,
//    [IslemId] INT NOT NULL,
//    [MGrupId] INT NOT NULL,
//    [Fiyat] FLOAT NOT NULL
//)
