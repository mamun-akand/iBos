using System;
using System.Collections.Generic;

namespace CRUD.Models
{
    public partial class EcomReview
    {
        public long ItemId { get; set; }
        public string CustomerName { get; set; } = null!;
        public string Comments { get; set; } = null!;
        public long Rating { get; set; }
        public bool IsActive { get; set; }
        public long AccountId { get; set; }
        public long BranchId { get; set; }
    }
}
