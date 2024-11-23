using System;
using System.Collections.Generic;

namespace CRUD.Models
{
    public partial class ItemGroup
    {
        public long GroupId { get; set; }
        public string GroupName { get; set; } = null!;
        public long AccountId { get; set; }
        public long BranchId { get; set; }
    }
}
