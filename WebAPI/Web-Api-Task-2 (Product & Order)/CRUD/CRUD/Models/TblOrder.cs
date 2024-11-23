using System;
using System.Collections.Generic;

namespace CRUD.Models
{
    public partial class TblOrder
    {
        public int IntOrderId { get; set; }
        public int IntProductId { get; set; }
        public string StrCustomerName { get; set; } = null!;
        public decimal NumQuantity { get; set; }
        public DateTime DteOrderDateTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime DteLastActionDateTime { get; set; }
    }
}
