using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class MalGirisResim : IEntity
    {
        public int Id { get; set; }
        public int MalGirisMalzemeId { get; set; }
        public byte[] Resim { get; set; }
        public int ItemNumber { get; set; }

    }
}
//[Id] INT IDENTITY(1, 1) NOT NULL,

//[MalGirisMalzemeId] INT             NULL,

//[Resim]             VARBINARY (MAX) NULL,
//    [ItemNumber] INT NULL,