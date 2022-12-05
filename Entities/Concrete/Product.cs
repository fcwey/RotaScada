using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Product : IEntity
    {
        //public: Diger librarylerden erişirsin demek
        //internal: sadece bu liraryde
        //private: tanımlandığı yerde
        //Class içinde class pek kullanılmaz


        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int SupplierID { get; set; }
        public int CategoryId { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public short UnitsInStock { get; set; }
        public short UnitsOnOrder { get; set; }
        public short ReorderLevel { get; set; }
        public bool Discontinued { get; set; }



    }
}