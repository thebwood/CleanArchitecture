
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Abstractions
{
    public abstract class AggregateRoot
    {
        protected AggregateRoot()
        {
            this.DomainEvents = new List<DomainEvent>();
        }
    }
}
