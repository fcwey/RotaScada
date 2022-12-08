using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class MalzemeGrup: IEntity
    {
        public int Id { get; set; }
        public string GrupName { get; set; }

    }
}
//CREATE TABLE[dbo].[MalzemeGrups] (
//    [Id]       INT IDENTITY(1, 1) NOT NULL,
//    [GrupName] NVARCHAR (50) NOT NULL,
//    PRIMARY KEY CLUSTERED ([Id] ASC)
//);

