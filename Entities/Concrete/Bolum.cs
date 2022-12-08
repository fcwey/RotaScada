using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Bolum: IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

//CREATE TABLE[dbo].[Bolum] (
//    [Id]        INT IDENTITY(1, 1) NOT NULL,
//    [Name] NVARCHAR (50) NOT NULL,
//    PRIMARY KEY CLUSTERED ([Id] ASC)
//);

