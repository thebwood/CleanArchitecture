using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Aggregates.Store
{
    public class Store
    {

        public Guid StoreId { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Address { get; set; }



    }
}
