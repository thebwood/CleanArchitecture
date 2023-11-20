using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Aggregates.UserProfile
{
    public class UserProfile
    {
        public Guid UserProfileId { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Address { get; set; }
        public string EmailAddress { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; } = null;

    }
}
