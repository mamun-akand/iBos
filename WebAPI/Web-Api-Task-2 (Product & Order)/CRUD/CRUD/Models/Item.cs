using System;
using System.Collections.Generic;

namespace CRUD.Models
{
    public partial class Item
    {
        public long ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public long Price { get; set; }
        public bool IsActive { get; set; }
        public bool IsVariant { get; set; }
        public long GroupId { get; set; }
        public long AccountId { get; set; }
        public long BranchId { get; set; }
    }
}
