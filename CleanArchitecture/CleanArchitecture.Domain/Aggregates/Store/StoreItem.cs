using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Aggregates.Store
{
    public class StoreItem
    {
        public Guid StoreItemId { get; set; } = Guid.NewGuid();
        public Guid StoreId { get; set; }
        public Guid ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
