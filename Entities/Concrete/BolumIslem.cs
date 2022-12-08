using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class BolumIslem: IEntity
    {
        public int Id { get; set; }
        public int  BolumId { get; set; }
        public string BolumIslemi { get; set; }
        public int Gun  { get; set; }

    }
}

//CREATE TABLE[dbo].[BolumIslem] (
//    [Id]         INT IDENTITY(1, 1) NOT NULL,
//    [BolumId]    INT           NOT NULL,
//    [BolumIslemi] NVARCHAR (50) NOT NULL,
//    [Gun]        INT           NOT NULL,
//    PRIMARY KEY CLUSTERED ([Id] ASC)
//);


