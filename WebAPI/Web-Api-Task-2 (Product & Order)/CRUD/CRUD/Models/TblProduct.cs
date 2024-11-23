using System;
using System.Collections.Generic;

namespace CRUD.Models
{
    public partial class TblProduct
    {
        public int IntProductId { get; set; }
        public string StrProductName { get; set; } = null!;
        public decimal NumUnitPrice { get; set; }
        public decimal NumStock { get; set; }
    }
}
