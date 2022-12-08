using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Malzeme : IEntity
    {
        public int Id { get; set; }
        public int MGrupId { get; set; }
        public string? MalzemeName { get; set; }
        public string? MaxSertlik { get; set; }

    }
}
//CREATE TABLE[dbo].[Malzemeler] (
//    [Id]          INT IDENTITY(1, 1) NOT NULL,
//    [MGrupId]     INT           NOT NULL,
//    [MalzemeName] NVARCHAR (50) NOT NULL,
//    [MaxSertlik]  NCHAR (10)    NULL,
//    PRIMARY KEY CLUSTERED ([Id] ASC)
//);
